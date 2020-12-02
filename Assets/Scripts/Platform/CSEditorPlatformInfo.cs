public class CSEditorPlatformInfo : AbstractPlatformInfo
{
    public override string LuaRoot
    {
        get
        {
            return CommonLoadPath.LocalProjectName;
        }
    }
    public override string XLuaMainPath {
        get {
            return CommonLoadPath.LocalProjectName + "main.lua";
        }
    }
}
