public static partial class CommonUtility {
    public static bool IsNull(UnityEngine.Object o)
    {
        return o == null;
    }

    public static bool IsNullOrEmpty(string txt)
    {
        return string.IsNullOrEmpty(txt);
    }
}