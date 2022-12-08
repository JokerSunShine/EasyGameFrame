using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class EditorUtil {
    public static bool EditorIsPlaying()
    {
        return EditorApplication.isPlaying == true || EditorApplication.isPaused == true || EditorApplication.isPlayingOrWillChangePlaymode == true;
    }
    
    #region 宏
    /// <summary>
    /// 设置宏
    /// </summary>
    /// <param name="macro">宏</param>
    /// <param name="isAdd">添加或移除</param>
    /// <returns>设置结果</returns>
    public static bool SetMacro(string macro, bool isAdd)
    {
        if (string.IsNullOrEmpty(macro) || EditorIsPlaying())
        {
            CSDebug.LogError("请不要在运行游戏的时候修改宏");
            return false;
        }
        CSDebug.Log(EditorUserBuildSettings.selectedBuildTargetGroup);
        string macroStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> defineMacroList = new List<string>(macroStr.Split(';'));
        if (isAdd)
            defineMacroList.Add_NoSame(macro);
        else
            defineMacroList.RemoveAll(macro);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", defineMacroList.ToArray()));
        return true;
    }

    /// <summary>
    /// 是否包含宏
    /// </summary>
    /// <param name="macro">宏</param>
    /// <returns>是否包含</returns>
    public static bool HaveMacro(string macro)
    {
        if (string.IsNullOrEmpty(macro))
        {
            return true;
        }
        string macroStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> defineMacroList = new List<string>(macroStr.Split(';'));
        return defineMacroList.Contains(macro);
    }
    #endregion
    
    #region 屏幕点击

    private static bool mouseDown = false;
    public static void AddSpliterLineEvent(Rect rect,MouseCursor mouseCursor,Action<Vector3> mounseDownCallBack = null,Action<Vector3> mouseUpCallBack = null)
    {
        EditorGUIUtility.AddCursorRect(rect, mouseCursor);
        if(Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            mouseDown = true;
        }
        if(mouseDown)
        {
            if(mounseDownCallBack != null)
            {
                mounseDownCallBack(Event.current.mousePosition);
            }
        }
        if(Event.current.type == EventType.MouseUp && rect.Contains(Event.current.mousePosition))
        {
            mouseDown = false;
            if(mouseUpCallBack != null)
            {
                mouseUpCallBack(Event.current.mousePosition);
            }
        }
    }
    #endregion
}