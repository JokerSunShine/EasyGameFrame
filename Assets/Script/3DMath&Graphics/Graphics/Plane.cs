using _3DMath;

namespace Graphics
{
    public class Plane
    {
        #region 数据
        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 normal;
        /// <summary>
        /// 平面到坐标原点的距离
        /// </summary>
        public float constant;
        #endregion
        
        #region 静态
        /// <summary>
        /// 与球是否相交
        /// </summary>
        /// <returns>1:不相交，在正方向，-1：不相交，在反方向 0：相交</returns>
        public static int CrossForSphere(Plane plane,Sphere sphere)
        {
            float d = plane.normal * sphere.center - plane.constant;
            
            if(d > sphere.radius)
            {
                return 1;
            }
            else if(d < -sphere.radius)
            {
                return -1;
            }

            return 0;
        }
        #endregion
    }
}