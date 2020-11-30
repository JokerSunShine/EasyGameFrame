public class CSEditorPlatformInfo : AbstractPlatformInfo
{
    public override string XLuaMainPath {
        get {
            return CommonLoadPath.LocalProjectName + "main.lua";
        }
    }
}
