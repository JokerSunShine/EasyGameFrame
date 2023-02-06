﻿using UnityEngine;

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
    
    private static string loadOriginRootPath;
    public static string LoadOriginRootPath {
        get {
            if (string.IsNullOrEmpty(loadOriginRootPath))
            {
                loadOriginRootPath = (Application.dataPath.Substring(0, Application.dataPath.Length - 7));
            }
            return loadOriginRootPath;
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

    private static string resourcePath;
    public static string ResourcePath
    {
        get
        {
            if(string.IsNullOrEmpty(resourcePath))
            {
                resourcePath = CommonUtility.RemoveBackDir(Application.dataPath, 1) + "/Data";
            }
            return resourcePath;
        }
    }

    private static string serverXmlPath;
    public static string ServerXmlPath
    {
        get
        {
            if(string.IsNullOrEmpty(serverXmlPath))
            {
                serverXmlPath = ResourcePath + "/server_message";
            }
            return serverXmlPath;
        }
    }

    public static string artResourcePath;
    public static string ArtResourcePath
    {
        get
        {
            if(string.IsNullOrEmpty(artResourcePath))
            {
                artResourcePath = Application.dataPath + "/Art";
            }
            return artResourcePath;
        }
    }
    
    public static string artScenePath;
    public static string ArtScenePath
    {
        get
        {
            if(string.IsNullOrEmpty(artScenePath))
            {
                artScenePath = ArtResourcePath + "/Scene";
            }
            return artScenePath;
        }
    }
    
    public static string clientResourcePath;
    public static string ClientResourcePath
    {
        get
        {
            if(string.IsNullOrEmpty(clientResourcePath))
            {
                clientResourcePath = Application.dataPath + "/Res";
            }
            return clientResourcePath;
        }
    }
    
    public static string cilentScenePath;
    public static string CilentScenePath
    {
        get
        {
            if(string.IsNullOrEmpty(cilentScenePath))
            {
                cilentScenePath = ClientResourcePath + "/Scene";
            }
            return cilentScenePath;
        }
    }
}