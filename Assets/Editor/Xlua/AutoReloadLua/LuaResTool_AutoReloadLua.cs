﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using XLua;

[InitializeOnLoad]
public class LuaResTool_AutoReloadLua
{
    #region 字段
    /// <summary>
    /// 开启自动重载lua的宏名
    /// </summary>
    public static string Macro_OpenAutoReloadLua = "AutoReloadLuaThread";
    private static string ReloadLuaFileArrayFunctionName = "ReloadLuaFileArray";
    private static FileSystemWatcher fileSystemWatcher;
    private static List<string> changedFileNameList = new List<string>();
    private static StringBuilder stringBuilder = new StringBuilder();
    private static LuaTable luaFileReloadManager = null;
    public static LuaTable LuaFileReloadManager {
        get {
            if (luaFileReloadManager != null)
            {
                return luaFileReloadManager;
            }
            if(XluaMgr.Instance.luaEnv != null)
            {
                luaFileReloadManager = XluaMgr.Instance.luaEnv.Global.Get<LuaTable>("LuaFileReloadManager");
            }
            return luaFileReloadManager;
        }
    }

    private static Action<object> reloadLuaFileArrayFunction = null;
    public static Action<object> ReloadLuaFileArrayFunction {
        get {
            if (reloadLuaFileArrayFunction != null)
            {
                return reloadLuaFileArrayFunction;
            }
            if (LuaFileReloadManager != null)
            {
                reloadLuaFileArrayFunction = LuaFileReloadManager.Get<Action<object>>(ReloadLuaFileArrayFunctionName);
            }
            return reloadLuaFileArrayFunction;
        }
    }
    #endregion

    #region 构造
    static LuaResTool_AutoReloadLua()
    {
        //在启动编辑器时添加监听
        if (!(EditorApplication.isPlaying == false && EditorApplication.isPaused == false && EditorApplication.isPlayingOrWillChangePlaymode == true))
        {
            return;
        }
        EditorApplication.playModeStateChanged += EidtorApplication_PlayModeStateChange;
    }

    private static void EidtorApplication_PlayModeStateChange(PlayModeStateChange stateChange)
    {
        switch (stateChange)
        {
            case PlayModeStateChange.EnteredPlayMode:
                if (Application.isPlaying)
                {
#if AutoReloadLuaThread
                            StartAutoReloadLua();
#endif
                }
                break;
            case PlayModeStateChange.ExitingPlayMode:
#if AutoReloadLuaThread
               StopAutoReloadLua();  
#endif
                break;
            default:
                break;
        }
    }
    #endregion


    #region 功能入口
    [MenuItem("XLua/查看自动更新状态")]
    public static void CheckAutoUpdateLuaState()
    {
        CSDebug.LogError(EditorUtil.HaveMacro(Macro_OpenAutoReloadLua));
    }

    [MenuItem("XLua/开启自动更新Lua")]
    public static void OpenAutoReloadLua()
    {
        if (EditorUtil.SetMacro(Macro_OpenAutoReloadLua,true))
        {
            StartAutoReloadLua();
        }
    }

    [MenuItem("XLua/关闭自动更新Lua")]
    public static void CloseAutoReloadLua()
    {
        if (EditorUtil.SetMacro(Macro_OpenAutoReloadLua,false))
        {
            StopAutoReloadLua();
        }
    }
#endregion

#region 自动更新开关
    public static void StartAutoReloadLua()
    {
        CSDebug.Log("开始监听LuaRes文件夹" + CSEditorPlatformInfo.LuaRoot);
        StopAutoReloadLua();
        fileSystemWatcher = new FileSystemWatcher();
        fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
        fileSystemWatcher.Path = CSEditorPlatformInfo.LuaRoot;
        fileSystemWatcher.Filter = "*.lua";
        fileSystemWatcher.IncludeSubdirectories = true;
        fileSystemWatcher.Changed += FileSystemWatcher_Changed;
        fileSystemWatcher.EnableRaisingEvents = true;
        fileSystemWatcher.EndInit();
        EditorApplication.update += OnUpdate;
        changedFileNameList.Clear();
    }

    public static void StopAutoReloadLua()
    {
        if (fileSystemWatcher != null)
        {
            CSDebug.Log("停止监听luaRes文件夹");
            fileSystemWatcher.Dispose();
            fileSystemWatcher = null;
            EditorApplication.update -= OnUpdate;
            changedFileNameList.Clear();
        }
    }

    private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        string changedFilePath = e.FullPath;
        string extension = Path.GetExtension(changedFilePath);
        if (extension == ".lua" && !string.IsNullOrEmpty(changedFilePath))
        {
            int index = changedFilePath.IndexOf("luaRes\\");
            if (index < 0)
            {
                return;
            }
            string newPath = changedFilePath.Substring(index).Replace(".lua", string.Empty).Replace('\\', '.');
            if (!changedFileNameList.Contains(newPath))
            {
                changedFileNameList.Add(newPath);
            }
        }
    }

    /// <summary>
    /// 编辑器脚本的刷新
    /// </summary>
    private static void OnUpdate()
    {
        if (fileSystemWatcher != null && changedFileNameList.Count > 0)
        {
            ReloadAllModifiedLuaFilesDuringPlaying(changedFileNameList.ToArray());
            changedFileNameList.Clear();
        }
    }

    /// <summary>
    /// 运行过程中重新加载所有修改过的lua文件
    /// </summary>
    /// <param name="allModifiedLuaFiles">所有修改过的lua文件数组</param>
    public static void ReloadAllModifiedLuaFilesDuringPlaying(string[] allModifiedLuaFiles)
    {
        if (EditorApplication.isPlaying == false)
        {
            return;
        }
        if (XluaMgr.Instance != null && changedFileNameList != null && changedFileNameList.Count > 0 && LuaFileReloadManager != null)
        {
            if (ReloadLuaFileArrayFunction != null)
            {
                ReloadLuaFileArrayFunction(allModifiedLuaFiles);
                stringBuilder.Clear();
                stringBuilder.Append(allModifiedLuaFiles.Length);
                stringBuilder.Append("个lua文件有修改,重新载入\r\n");
                for (int i = 0; i < allModifiedLuaFiles.Length; i++)
                {
                    stringBuilder.Append(i);
                    stringBuilder.Append("  ");
                    stringBuilder.Append(allModifiedLuaFiles[i]);
                    if (i != allModifiedLuaFiles.Length - 1)
                    {
                        stringBuilder.Append("\r\n");
                    }
                }
                CSDebug.Log(stringBuilder.ToString());
                stringBuilder.Clear();
            }
        }
    }
    #endregion
}
