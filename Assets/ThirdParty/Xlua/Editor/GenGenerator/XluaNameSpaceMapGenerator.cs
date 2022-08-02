using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class XluaNameSpaceMapGenerator
{
    #region 数据
    //命名空间提示包
    private const string NameSpaceZipName = "CSNameSpace";
    private const string CSClient = "CSClient";
    /// <summary>
    /// 需要忽略生成语法提示的类
    /// </summary>
    private static List<Type> IgnoreTypeList = new List<Type>() {

    };
    /// <summary>
    /// 需要忽略生成语法提示的子类
    /// </summary>
    private static List<Type> IgnoreSubClassTypeList = new List<Type>() {
        typeof(Attribute),
    };
    #endregion

    [MenuItem("XLua/Gen C#语法提示")]
    public static void GenAll()
    {
        EditorCoroutineRunner.StartEditorCoroutine(GenAllSync());
    }

    private static IEnumerator GenAllSync()
    {
        if (!Directory.Exists(CommonLoadPath.CreateLuaGrammarTipPath))
        {
            Directory.CreateDirectory(CommonLoadPath.CreateLuaGrammarTipPath);
        }
        yield return GenAllNameSpace();
        GenAllMethod();
        EditorUtility.DisplayDialog("完成","完成lua语法提示","确定");
    }

    #region 生成命名空间提示
    private static IEnumerator GenAllNameSpace()
    {
        yield return null;
        List<string> noNameSpaceList = new List<string>();
        List<string> nameSpaceList = new List<string>();
        List<Type> typeList = CommonUtility.GetAllTypes();
        for (int i = 0; i < typeList.Count; i++)
        {
            Type type = typeList[i];
            if (i % 100 == 0)
            {
                yield return null;
                EditorUtility.DisplayProgressBar("Dealing", i + "/" + typeList.Count, i * 1f / typeList.Count);
            }
            if (string.IsNullOrEmpty(type.Namespace))
            {
                string typeName = type.FullName.Split('+')[0];
                if (!noNameSpaceList.Contains(typeName))
                {
                    noNameSpaceList.Add(typeName);
                }
            }
            else
            {
                try
                {
                    string typeName = type.FullName.Split('.')[0];
                    if (!nameSpaceList.Contains(typeName))
                    {
                        nameSpaceList.Add(typeName);
                    }
                }
                catch (System.Exception ex)
                { }
            }
        }
        EditorUtility.ClearProgressBar();
        nameSpaceList.Sort();
        noNameSpaceList.Sort();

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("CS = {");
        foreach (string nameSpace in nameSpaceList)
        {
            stringBuilder.Append("\t");
            stringBuilder.Append(nameSpace);
            stringBuilder.Append(" = ");
            stringBuilder.Append(nameSpace);
            stringBuilder.AppendLine(";");
        }
        stringBuilder.AppendLine();
        foreach (string noNameSpace in noNameSpaceList)
        {
            if (noNameSpace.Contains("AnonType") || noNameSpace.Contains("PrivateImplementationDetails") || noNameSpace.Contains("AnonymousType"))
            {
                continue;
            }
            string curNoNameSpace = noNameSpace;
            if (curNoNameSpace.Contains("`"))
            {
                int i = curNoNameSpace.LastIndexOf("`");
                curNoNameSpace = curNoNameSpace.Substring(0, i);
            }
            stringBuilder.Append("\t");
            stringBuilder.Append(curNoNameSpace);
            stringBuilder.Append(" = ");
            stringBuilder.Append(curNoNameSpace);
            stringBuilder.AppendLine(";");
        }
        stringBuilder.Append("}");

        string luaScriptFileName = NameSpaceZipName + ".lua";
        byte[] luaScriptsBytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
        Dictionary<string, byte[]> zipFileDict = new Dictionary<string, byte[]>() { { luaScriptFileName, luaScriptsBytes } };
        string zipFileName = CommonLoadPath.CreateLuaGrammarTipPath + "/" + NameSpaceZipName + ".zip";
        ZipManager.CreateZip(zipFileName, zipFileDict);
        CSDebug.Log("成功创建 " + zipFileName);
    }
    #endregion

    #region 生成脚本提示
    private static void GenAllMethod()
    {
        if (!Directory.Exists(CommonLoadPath.CreateCSClientTipPath))
        {
            Directory.CreateDirectory(CommonLoadPath.CreateCSClientTipPath);
        }
        List<Type> typeList = CommonUtility.GetAllTypes();
        for (int i = 0; i < typeList.Count; i++)
        {
            Type type = typeList[i];
            if (IgnoreTypeList.Contains(type))
            {
                continue;
            }
            string fileName = CommonUtility.GetTypeFileName(type) + "_Wrap.lua";
            EditorUtility.DisplayProgressBar("生成中", "生成" + fileName, (float)(i + 1) / typeList.Count);
            List<string> NoTipsGenericList = new List<string>();
            if (IsContainsIgnoreSubClass(type))
            {
                if (NoTipsGenericList.Contains(type.Name) == false)
                {
                    NoTipsGenericList.Add(type.Name);
                }
                continue;
            }
            try
            {
                GenNameSpaceFile(type);
                using (StreamWriter writer = new StreamWriter(CommonLoadPath.CreateCSClientTipPath + fileName, false, System.Text.Encoding.UTF8))
                {
                    writer.Write("---@class ");
                    writer.Write(CommonUtility.GetTypeTagName(type));
                    MethodInfo methodInfo = null;
                    if (type.IsEnum)
                    {
                        writer.Write("\r\n");
                        foreach (var item in Enum.GetValues(type))
                        {
                            writer.Write("---@field ");
                            writer.Write(item);
                            writer.Write(" @");
                            writer.WriteLine((int)item);
                        }
                    }
                    else
                    {
                        //基类
                        var baseType = type.BaseType;
                        if (baseType != null && baseType != typeof(object))
                        {
                            writer.Write(" : ");
                            writer.WriteLine(CommonUtility.GetTypeTagName(baseType));
                        }
                        else
                        {
                            writer.WriteLine();
                        }
                        if (baseType != null)
                        {
                            if (baseType.Name == "Singleton`1")
                            {
                                writer.WriteLine("---@field public Instance " + type.FullName);
                            }
                            else if (baseType.Name.Contains("`"))
                            {
                                if (!NoTipsGenericList.Contains(baseType.Name))
                                {
                                    NoTipsGenericList.Add(baseType.Name);
                                }
                            }
                            else if (baseType.Name == "TableManager`4")
                            {
                                methodInfo = type.GetMethod("TryGetValue");//TryGetValue在基类，后面的WriteMethods没有处理它
                                writer.WriteLine("---@field public Instance " + type.FullName);
                            }
                        }
                        WriteFields(writer, type);
                        WriteProperties(writer, type);
                        writer.WriteLine("local m = {};");
                        WriteMethods(writer, type);
                        if (methodInfo != null)
                        {
                            WriteMethods(writer, methodInfo);
                        }
                    }
                    writer.WriteLine(type.GetTypeTagName() + "=m");
                    writer.Write("return m;");
                }
            }
            catch (System.Exception ex)
            {
                CSDebug.Log(fileName + " is not Gen");
            }
        }
        EditorUtility.DisplayProgressBar("正在生成zip", "", 1);

        DirectoryInfo wrapDir = new DirectoryInfo(CommonLoadPath.CreateCSClientTipPath);
        FileInfo[] files = wrapDir.GetFiles("*", SearchOption.AllDirectories);
        Dictionary<string, byte[]> fileDict = new Dictionary<string, byte[]>();
        foreach (var cur in files)
        {
            string fileName = CommonUtility_Filed.GetFileName(cur.FullName) + ".lua";
            if (fileDict.ContainsKey(fileName)) continue;
            string content = File.ReadAllText(cur.FullName, Encoding.UTF8);
            byte[] luaScriptBytes = Encoding.UTF8.GetBytes(content);
            fileDict.Add(fileName, luaScriptBytes);
        }
        ZipManager.CreateZip(CommonLoadPath.CreateLuaGrammarTipPath + "/CSClient.zip", fileDict);
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 是否包含需要忽略的子类
    /// </summary>
    /// <returns></returns>
    private static bool IsContainsIgnoreSubClass(Type type)
    {
        foreach (Type ignoreSubClass in IgnoreSubClassTypeList)
        {
            if (type.IsSubclassOf(ignoreSubClass))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 写入命名空间语法提示
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static void GenNameSpaceFile(Type type)
    {
        if (string.IsNullOrEmpty(type.Namespace)) return;
        string fileName = type.GetTypeTagName();
        string wrapPath = fileName + "_Wrap.lua";
        using (StreamWriter writer = new StreamWriter(CommonLoadPath.CreateCSClientTipPath + wrapPath, false, System.Text.Encoding.UTF8))
        {
            writer.WriteLine(type.Namespace + " = {};");
        }
    }

    #region 类名
    /// <summary>
    /// 写入类名
    /// </summary>
    private static void WriteFields(StreamWriter writer, Type type)
    {
        var staticFieldArray = type.GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static).Where(_ =>
        {
            return _.IsDefined(typeof(System.ObsoleteAttribute)) == false && GenBlackList.isMemberInBlackList(_) == false;
        });
        var instanceFieldArray = type.GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance).Where(_=> {
            return _.IsDefined(typeof(System.ObsoleteAttribute)) == false && GenBlackList.isMemberInBlackList(_) == false;
        });
        WriteFields(writer, staticFieldArray,true);
        WriteFields(writer, instanceFieldArray, false);
    }

    /// <summary>
    /// 写入类名
    /// </summary>
    private static void WriteFields(StreamWriter writer, IEnumerable<FieldInfo> fields, bool isStatic)
    {
        bool writedHead = false;
        foreach (var item in fields)
        {
            if (!writedHead)
            {
                writer.WriteLine(isStatic ? "---fields" : "---instance fields");
                writedHead = true;
            }
            writer.Write("---@field public ");
            writer.Write(item.Name);
            writer.Write(' ');
            writer.WriteLine(item.FieldType.GetTypeTagName());
        }
    }
    #endregion

    #region 属性
    /// <summary>
    /// 写入属性
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="type"></param>
    private static void WriteProperties(StreamWriter writer,Type type)
    {
        var staticFieldArray = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static).Where(_ =>
        {
            return _.IsDefined(typeof(System.ObsoleteAttribute)) == false && GenBlackList.isMemberInBlackList(_) == false;
        });
        var instanceFieldArray = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance).Where(_ =>
        {
            return _.IsDefined(typeof(System.ObsoleteAttribute)) == false && GenBlackList.isMemberInBlackList(_) == false;
        });
        WriteProperties(writer, staticFieldArray, true);
        WriteProperties(writer, instanceFieldArray, false);
    }

    /// <summary>
    /// 写入属性
    /// </summary>
    private static void WriteProperties(StreamWriter writer, IEnumerable<PropertyInfo> properties, bool isStatic)
    {
        bool writedHead = false;
        foreach (var item in properties)
        {
            if (!writedHead)
            {
                writer.WriteLine(isStatic ? "---properties" : "---instance properties");
                writedHead = true;
            }
            writer.Write("---@field public ");
            writer.Write(item.Name);
            writer.Write(' ');
            writer.WriteLine(item.PropertyType.GetTypeTagName());
        }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 写入方法
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="type"></param>
    private static void WriteMethods(StreamWriter writer, Type type)
    {
        var staticFieldArray = type.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static).Where(_ =>
        {
            return _.IsDefined(typeof(System.ObsoleteAttribute)) == false && GenBlackList.isMemberInBlackList(_) == false;
        });
        var instanceFieldArray = type.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance).Where(_ =>
        {
            return _.IsDefined(typeof(System.ObsoleteAttribute)) == false && GenBlackList.isMemberInBlackList(_) == false;
        });
        WriteMethods(writer, staticFieldArray, GenMethodType.Static);
        WriteMethods(writer, instanceFieldArray, GenMethodType.Instance);
    }

    /// <summary>
    /// 写入方法
    /// </summary>
    private static void WriteMethods(StreamWriter writer, IEnumerable<MethodInfo> methods, GenMethodType methodType)
    {
        bool wroteHead = false;
        foreach (var item in methods)
        {
            var methodName = item.Name;
            if (methodName.StartsWith("get_") || methodName.StartsWith("set_"))
            {
                continue;
            }
            if (item.IsGenericMethod && !isSupportedGenericMethod(item))
            {
                continue;
            }
            if (methodType == GenMethodType.Extension)
            {
                if (!wroteHead)
                {
                    writer.WriteLine();
                    writer.WriteLine("---extension methods");
                    wroteHead = true;
                }
            }
            var paramArray = item.GetParameters();
            for (int i = 0, iMax = paramArray.Length; i < iMax; i++)
            {
                if (i == 0 && methodType == GenMethodType.Extension)
                {
                    continue;
                }
                var param = paramArray[i];
                var paramName = param.Name;
                paramName = GetNewParamName(paramName);
                writer.Write("---@param ");
                writer.Write(paramName);
                writer.Write(' ');
                writer.Write(param.ParameterType.GetTypeTagName());
                if ((param.Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault)
                {
                    writer.Write(" @default_value:");
                    writer.WriteLine(param.DefaultValue);
                }
                else if (param.IsOut)
                {
                    writer.WriteLine(" @out");
                }
                else if (param.IsIn)
                {
                    writer.WriteLine(" @in");
                }
                else
                {
                    writer.WriteLine();
                }
            }
            if (item.ReturnType != typeof(void))
            {
                writer.Write("---@return ");
                writer.WriteLine(item.ReturnType.GetTypeTagName());
            }
            writer.Write("function m");
            writer.Write(methodType == GenMethodType.Static ? "." : ":");
            writer.Write(item.Name);
            writer.Write("(");
            for (int i = 0, iMax = paramArray.Length; i < iMax; i++)
            {
                if (i == 0 && methodType == GenMethodType.Extension)
                {
                    continue;
                }
                string paramName = paramArray[i].Name;
                paramName = GetNewParamName(paramName);
                writer.Write(paramName);
                if (i < iMax - 1)
                {
                    writer.Write(", ");
                }
            }
            writer.WriteLine(") end");
        }
    }

    //单写一个函数，可以用其他类中的函数写到当前类中
    private static void WriteMethods(StreamWriter writer, MethodInfo item, GenMethodType methodType = GenMethodType.Instance)
    {
        var methodName = item.Name;
        if (methodName.StartsWith("get_") || methodName.StartsWith("set_"))
        {
            return;
        }
        if (item.IsGenericMethod && !isSupportedGenericMethod(item))
        {
            return;
        }
        var paramArray = item.GetParameters();
        for (int i = 0, iMax = paramArray.Length; i < iMax; i++)
        {
            var param = paramArray[i];
            var paramName = param.Name;
            paramName = GetNewParamName(paramName);
            writer.Write("---@param ");
            writer.Write(paramName);
            writer.Write(' ');
            writer.Write(param.ParameterType.GetTypeTagName());
            if ((param.Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault)
            {
                writer.Write(" @default_value:");
                writer.WriteLine(param.DefaultValue);
            }
            else if (param.IsOut)
            {
                writer.WriteLine(" @out");
            }
            else if (param.IsIn)
            {
                writer.WriteLine(" @in");
            }
            else
            {
                writer.WriteLine();
            }
        }
        if (item.ReturnType != typeof(void))
        {
            writer.Write("---@return ");
            writer.WriteLine(item.ReturnType.GetTypeTagName());
        }
        writer.Write("function m");
        writer.Write(methodType == GenMethodType.Static ? "." : ":");
        writer.Write(item.Name);
        writer.Write("(");
        for (int i = 0, iMax = paramArray.Length; i < iMax; i++)
        {
            string paramName = paramArray[i].Name;
            paramName = GetNewParamName(paramName);
            writer.Write(paramName);
            if (i < iMax - 1)
            {
                writer.Write(", ");
            }
        }
        writer.WriteLine(") end");
    }

    private static bool isSupportedGenericMethod(MethodInfo method)
    {
        if (!method.ContainsGenericParameters)
            return true;
        var methodParameters = method.GetParameters();
        var hasValidGenericParameter = false;
        for (var i = 0; i < methodParameters.Length; i++)
        {
            var parameterType = methodParameters[i].ParameterType;
            if (parameterType.IsGenericParameter)
            {
                var parameterConstraints = parameterType.GetGenericParameterConstraints();
                if (parameterConstraints.Length == 0 || !parameterConstraints[0].IsClass)
                    return false;
                hasValidGenericParameter = true;
            }
        }
        return hasValidGenericParameter;
    }

    private static string GetNewParamName(string paramName)
    {
        if (paramName == "end") paramName = "endParam";
        if (paramName == "and") paramName = "andParam";
        else if (paramName == "local") paramName = "localParam";
        return paramName;
    }
    #endregion
    #endregion
}
