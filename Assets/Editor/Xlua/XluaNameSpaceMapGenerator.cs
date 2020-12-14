using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class XluaNameSpaceMapGenerator
{
    #region 数据
    private const string NameSpaceZipName = "CSNameSpace";
    /// <summary>
    /// 需要忽略生成语法提示的类
    /// </summary>

    private static List<Type> IgnoreTypeList = new List<Type>() {

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
        EditorUtility.DisplayDialog("完成","完成lua语法提示","确定");
    }

    private static IEnumerator GenAllNameSpace()
    {
        yield return null;
        List<string> noNameSpaceList = new List<string>();
        List<string> nameSpaceList = new List<string>();
        List<Type> typeList = CommonUtility.GetAllTypes();
        for (int i = 0;i < typeList.Count;i++)
        {
            Type type = typeList[i];
            if (i % 100 == 0)
            {
                yield return null;
                EditorUtility.DisplayProgressBar("Dealing",i + "/" + typeList.Count,i * 1f / typeList.Count);
            }
            if (string.IsNullOrEmpty(type.Namespace))
            {
                string typeName = type.FullName.Split('+')[0];
                if (!noNameSpaceList.Contains(typeName))
                {
                    noNameSpaceList.Add(typeName);
                }
            }
            else {
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
            stringBuilder.AppendLine();
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
            stringBuilder.AppendLine();
        }
        stringBuilder.Append("}");

        string luaScriptFileName = NameSpaceZipName + ".lua";
        byte[] luaScriptsBytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
        Dictionary<string, byte[]> zipFileDict = new Dictionary<string, byte[]>() { { luaScriptFileName,luaScriptsBytes} };
        string zipFileName = CommonLoadPath.CreateLuaGrammarTipPath + "/" + NameSpaceZipName + ".zip";
        ZipManager.CreateZip(zipFileName, zipFileDict);
        CSDebug.Log("成功创建 " + zipFileName);
    }

    private static void GenAllMethod()
    {
        List<Type> typeList = CommonUtility.GetAllTypes();
        for (int i = 0;i < typeList.Count;i++)
        {
            Type type = typeList[i];
            if (IgnoreTypeList.Contains(type))
            {
                continue;
            }
            string fileName = CommonUtility.GetTypeFileName(type) + "_Wrap.lua";
            EditorUtility.DisplayProgressBar("生成中" , "生成" + fileName,(float)(i+1) / typeList.Count);

        }
    }
}
