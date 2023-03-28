using XLua;

public static class LuaConfigGen
{
    [LuaCallCSharp]
    public static System.Type[] LuaCallCSharp = {
        typeof(Main),
        typeof(Framework.ResourceManager)
    };
}