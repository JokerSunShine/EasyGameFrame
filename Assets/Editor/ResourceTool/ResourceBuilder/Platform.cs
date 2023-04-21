namespace ResourceTool
{
    public enum Platform : byte
    {
        Undefined = 0,
        Windows = 1 << 0,
        Windows64 = 1 << 1,
        MacOS = 1 << 2,
        Linux = 1 << 3,
        IOS = 1 << 4,
        Android = 1 << 5,
        WindowsStore = 1 << 6,
        WebGL = 1 << 7,
    }
}