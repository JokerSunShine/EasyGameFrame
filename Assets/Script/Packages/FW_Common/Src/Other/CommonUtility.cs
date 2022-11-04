using System;

public static partial class CommonUtility {
    public static bool IsNull(UnityEngine.Object o)
    {
        return o == null;
    }

    public static bool IsNullOrEmpty(string txt)
    {
        return string.IsNullOrEmpty(txt);
    }
    
    public static void Swap(float a,float b)
    {
        float swap = a;
        a = b;
        b = swap;
    }
}