using UnityEngine;

public class CSAndroidPlatformInfo : AbstractPlatformInfo
{
    public override string LuaRoot
    {
        get
        {
            return Application.persistentDataPath + "/luaRes";
        }
    }
    public override string XLuaMainPath {
        get {
            return Application.persistentDataPath + "/luaRes/main.lua";
        }
    }
}
