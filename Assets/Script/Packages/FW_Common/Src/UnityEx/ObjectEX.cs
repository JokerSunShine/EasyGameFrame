using UnityEngine;

namespace Script.Packages.FW_Common.Src.UnityEx
{
    public static class ObjectEX
    {
        public static void Destroy(Object obj)
        {
            if(Application.isPlaying)
                Object.Destroy(obj);
            else
                Object.DestroyImmediate(obj);
        }
        
        public static bool IsNull(Object obj)
        {
            return obj == null;
        }
    }
}