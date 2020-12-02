using System.Collections.Generic;
using UnityEditor;

public partial class EditorUtility {
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
        string macroStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(CSPlatformManager.Instance.BuildTargetGroup);
        List<string> defineMacroList = new List<string>(macroStr.Split(';'));
        if (isAdd)
            defineMacroList.Add_NoSame(macro);
        else
            defineMacroList.RemoveAll(macro);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(CSPlatformManager.Instance.BuildTargetGroup, string.Join(";", defineMacroList.ToArray()));
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
        string macroStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(CSPlatformManager.Instance.BuildTargetGroup);
        List<string> defineMacroList = new List<string>(macroStr.Split(';'));
        return defineMacroList.Contains(macro);
    }
    #endregion
}