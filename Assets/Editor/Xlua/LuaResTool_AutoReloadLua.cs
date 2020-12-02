using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class LuaResTool_AutoReloadLua
{
    #region 字段
    /// <summary>
    /// 开启自动重载lua的宏名
    /// </summary>
    public static string Macro_OpenAutoReloadLua = "AutoReloadLuaThread";
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
#if AutoReloadLuaThread
        
#endif
    }

    private static void EidtorApplication_PlayModeStateChange(PlayModeStateChange stateChange)
    {
        switch (stateChange)
        {
            case PlayModeStateChange.EnteredPlayMode:
                if (Application.isPlaying)
                {
                    StartAutoReloadLua();
                }
                break;
            case PlayModeStateChange.ExitingPlayMode:
                StopAutoReloadLua();
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

    }

    [MenuItem("XLua/开启自动更新Lua")]
    public static void OpenAutoReloadLua()
    {

    }

    [MenuItem("XLua/关闭自动更新Lua")]
    public static void CloseAutoReloadLua()
    {

    }
#endregion

#region 自动更新开关
    public static void StartAutoReloadLua()
    {

    }

    public static void StopAutoReloadLua()
    {

    }
#endregion

}
