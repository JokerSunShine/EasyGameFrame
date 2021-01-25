using System;
using System.Collections.Generic;
using System.Reflection;
using XLua;

public static partial class CommonUtility {
    private static string[] IgnoreFileNameChars = new string[] { "+",".", "`","&","[","]","," };
    private static string[] IgnoreTagNameChars = new string[] { "+", ".", "`", "&", "[", "]", "," };

    /// <summary>
    /// 获取当前所有程序域下所有程序集的Type
    /// </summary>
    public static List<Type> GetAllTypes()
    {
        List<Type> typeList = new List<Type>();
        try
        {
            List<Assembly> assemblyList = new List<Assembly>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            assemblyList.AddRange(assemblies);
            for (int i = 0; i < assemblyList.Count; i++)
            {
                CSDebug.Log("dll = " + assemblyList[i].Location);

                Type[] types = assemblyList[i].GetTypes();
                foreach (var type in types)
                {
                    if ((type.Namespace != null && type.Namespace.StartsWith("UnityEditor")))
                    {
                        continue;
                    }
                    if (IsNameCurSpace(type, "UnityEditor"))
                    {
                        continue;
                    }
                    typeList.Add(type);
                }
            }
        }
        catch(System.Exception ex) {

        }
        return typeList;
    }

    /// <summary>
    /// 类是否继承或包含指定的命名空间
    /// </summary>
    /// <param name="type"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNameCurSpace(Type type,string str)
    {
        if (type == null) return false;
        if (type.Namespace == str) return true;
        type = type.BaseType;
        return IsNameCurSpace(type,str);
    }

    /// <summary>
    /// 获取类型的文件名
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetTypeFileName(Type type)
    {
        return GetTypeFileName(type.ToString());
    }

    /// <summary>
    /// 获取类型名字文件名
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static string GetTypeFileName(string typeName)
    {
        if (IgnoreFileNameChars == null || IgnoreFileNameChars.Length == 0)
        {
            return typeName;
        }
        string curTypeName = typeName;
        foreach (string ignoreChar in IgnoreFileNameChars)
        {
            curTypeName = curTypeName.Replace(ignoreChar,"");
        }
        return curTypeName;
    }

    /// <summary>
    /// 获取类型的标签名
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetTypeTagName(this Type type)
    {
        return GetTypeTagName(type.ToString());
    }

    /// <summary>
    /// 获取类型的标签名
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static string GetTypeTagName(string typeName)
    {
        if (IgnoreTagNameChars == null || IgnoreTagNameChars.Length == 0)
        {
            return typeName;
        }
        foreach (string ignoreChar in IgnoreTagNameChars)
        {
            typeName = typeName.Replace(ignoreChar, "");
        }
        return typeName;
    }

    /// <summary>
    /// 是否是指定的类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="curType"></param>
    /// <returns></returns>
    public static bool IsDefined(this Type type, Type curType)
    {
        if (type == null)
        {
            return false;
        }
        return type.IsDefined(curType,false);
    }

    public static bool IsDefined(this MemberInfo memberInfo,Type curType)
    {
        if (memberInfo == null)
        {
            return false;
        }
        return memberInfo.IsDefined(curType,false);
    }
}