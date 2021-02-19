public partial class InterfaceSingleton
{
    private static I_CSPlatformManager iPlatformManager;
    public static I_CSPlatformManager IPlatformManager
    {
        get
        {return iPlatformManager;}
        set
        {iPlatformManager = value;}
    }
}