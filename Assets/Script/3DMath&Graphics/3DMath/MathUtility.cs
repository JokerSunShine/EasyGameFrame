using UnityEngine;

namespace _3DMath
{
    public static class MathUtility
    {
        public const float k2PI = Mathf.PI * 2;
        public const float k1Over2PI = 1 / k2PI;
        public const float kPIOver2 = Mathf.PI / 2;
        public const int MaxValue = 214783647;
        public const int AlphabetSize = 256;
        
        /// <summary>
        /// 边界安全的反余弦
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float SafeACos(float x)
        {
            if(x <= -1)
            {
                return Mathf.PI;
            }
            if(x >= 1)
            {
                return 0;
            }

            return Mathf.Acos(x);
        }
    }
}