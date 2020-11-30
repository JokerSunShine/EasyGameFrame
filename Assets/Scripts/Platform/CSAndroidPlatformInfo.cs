using UnityEngine;

public class CSAndroidPlatformInfo : AbstractPlatformInfo
{
    public override string XLuaMainPath {
        get {
            return Application.persistentDataPath + "/luaRes/main.lua";
        }
    }
}
