using UnityEngine;

public class CommonLoadPath {
    private static string localProjectName;
    public static string LocalProjectName {
        get {
            if (string.IsNullOrEmpty(localProjectName))
            {
                localProjectName = Application.dataPath.Replace("/Assets", "");
                localProjectName = localProjectName.Substring(localProjectName.LastIndexOf("/") + 1);
            }
            return localProjectName;
        }
    }

    private static string loadRootPath;
    public static string LoadRootPath {
        get {
            if (string.IsNullOrEmpty(loadRootPath))
            {
                loadRootPath = (Application.dataPath.Substring(0, Application.dataPath.Length - 7)).Replace("/", "\\");
            }
            return loadRootPath;
        }
    }

    private static string createLuaGrammarTipPath;
    public static string CreateLuaGrammarTipPath {
        get {
            if (string.IsNullOrEmpty(createLuaGrammarTipPath))
            {
                createLuaGrammarTipPath = CommonUtility.RemoveBackDir(Application.dataPath, 1) + "/LuaGrammarTips";
            }
            return createLuaGrammarTipPath;
        }
    }

    private static string createCSClientTipPath;
    public static string CreateCSClientTipPath
    {
        get
        {
            if (string.IsNullOrEmpty(createCSClientTipPath))
            {
                createCSClientTipPath = CreateLuaGrammarTipPath + "/CSClient/";
            }
            return createCSClientTipPath;
        }
    }

    private static string loadVideoPath;
    public static string LoadVideoPath
    {
        get
        {
            if (string.IsNullOrEmpty(loadVideoPath))
            {
                loadVideoPath = CommonUtility.RemoveBackDir(Application.dataPath, 1) + "/Resource/Video";
            }
            return loadVideoPath;
        }
    }
}