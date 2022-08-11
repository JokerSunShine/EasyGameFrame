using System.Diagnostics;
using _3DMath;

namespace Graphics
{
    public enum AABBPointType
    {
        FRONT_LEFT_BELOW = 0,
        FRONT_RIGHT_BELOW = 1,
        FRONT_LEFT_ABOVE = 2,
        FRONT_RIGHT_ABOVE = 3,
        BEHIND_LEFT_BELOW = 4,
        BEHIND_RIGHT_BELOW = 5,
        BEHIND_LEFT_ABOVE = 6,
        BEHIND_RIGHT_ABOVE = 7,
    }
    
    //Y     Z
    //|    /
    //|   /
    //|  /
    //| /
    //|/
    //--------------- X
    
    /// <summary>
    /// 轴对齐矩形边界框
    /// </summary>
    public class AABB
    {
        #region
        public Vector3 min;
        public Vector3 max;
        #endregion
        
        #region 内部
        public Vector3 Size() => max - min;
        public float xSize() => max.x - min.x;
        public float YSize() => max.y - min.y;
        public float ZSize() => max.z - min.z;
        public Vector3 Center() => (min + max) * 0.5f;
        
        /// <summary>
        /// 获取坐标点
        /// </summary>
        /// <param name="type">aabb点类型</param>
        /// <returns></returns>
        public Vector3 GetCoordPoint(AABBPointType type)
        {
            Vector3 vec = Vector3.zero;
            switch(type)
            {
                case AABBPointType.FRONT_LEFT_BELOW:
                    vec = new Vector3(min.x, min.y, min.z);
                    break;
                case AABBPointType.FRONT_RIGHT_BELOW:
                    vec = new Vector3(max.x, min.y, min.z);
                    break;
                case AABBPointType.FRONT_LEFT_ABOVE:
                    vec = new Vector3(min.x, max.y, min.z);
                    break;
                case AABBPointType.FRONT_RIGHT_ABOVE:
                    vec = new Vector3(max.x, max.y, min.z);
                    break;
                case AABBPointType.BEHIND_LEFT_BELOW:
                    vec = new Vector3(min.x, min.y, max.z);
                    break;
                case AABBPointType.BEHIND_RIGHT_BELOW:
                    vec = new Vector3(max.x, min.y, max.z);
                    break;
                case AABBPointType.BEHIND_LEFT_ABOVE:
                    vec = new Vector3(min.x, max.y, max.z);
                    break;
                case AABBPointType.BEHIND_RIGHT_ABOVE:
                    vec = new Vector3(max.x, max.y, max.z);
                    break;
            }
            return vec;
        }
        
        /// <summary>
        /// 清空
        /// </summary>
        public void Empty()
        {
            //最小向量大于最大向量，则轴对齐矩形边界框就没有意义
            const float bigNumber = 1e37f;
            min.x = min.y = min.z = bigNumber;
            max.x = max.y = max.z = -bigNumber;
        }
        
        /// <summary>
        /// 是否是无效的
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return max.x > min.x && max.y > min.y && max.z > min.z;
        }
        
        /// <summary>
        /// 点扩展
        /// </summary>
        /// <param name="v"></param>
        public void Add(Vector3 v)
        {
            if (v.x < min.x) min.x = v.x;
            if (v.x > max.x) max.x = v.x;
            if (v.y < min.y) min.y = v.y;
            if (v.y > max.y) max.y = v.y;
            if (v.z < min.z) min.z = v.z;
            if (v.z > max.z) max.z = v.z;
        }
        
        /// <summary>
        /// 矩形扩展
        /// </summary>
        /// <param name="aabb"></param>
        public void Add(AABB aabb)
        {
            if (aabb.min.x < min.x) min.x = aabb.min.x;
            if (aabb.max.x > max.x) max.x = aabb.max.x;
            if (aabb.min.y < min.y) min.y = aabb.min.y;
            if (aabb.max.y > max.y) max.y = aabb.max.y;
            if (aabb.min.z < min.z) min.z = aabb.min.z;
            if (aabb.max.z > max.z) max.z = aabb.max.z;
        }
        
        /// <summary>
        /// 查询点是否在矩形内
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsContain(Vector3 v)
        {
            return v.x >= min.x && v.x <= max.x &&
                   v.y >= min.y && v.y <= max.y &&
                   v.z >= min.y && v.z <= max.y;
        }
        
        /// <summary>
        /// 设置矩阵变化的边界框
        /// </summary>
        public void SetToTransformBox(AABB aabb,Matrix4x3 matrix)
        {
            if(aabb.IsEmpty())
            {
                aabb.Empty();
                return;
            }

            min = max = Matrix4x3.GetTranslation(matrix);
            
            if(matrix.m11 > 0)
            {
                min.x += matrix.m11 * aabb.min.x;
                max.x += matrix.m11 * aabb.max.x;
            }
            else
            {
                min.x += matrix.m11 * aabb.max.x;
                max.x += matrix.m11 * aabb.min.x;
            }
            
            if(matrix.m21 > 0)
            {
                min.x += matrix.m21 * aabb.min.y;
                max.x += matrix.m21 * aabb.max.y;
            }
            else
            {
                min.x += matrix.m21 * aabb.max.y;
                max.x += matrix.m21 * aabb.min.y;
            }
            
            if(matrix.m31 > 0)
            {
                min.x += matrix.m31 * aabb.min.z;
                max.x += matrix.m31 * aabb.max.z;
            }
            else
            {
                min.x += matrix.m31 * aabb.max.z;
                max.x += matrix.m31 * aabb.min.z;
            }
            
            if(matrix.m12 > 0)
            {
                min.y += matrix.m12 * aabb.min.x;
                max.y += matrix.m12 * aabb.max.x;
            }
            else
            {
                min.y += matrix.m12 * aabb.max.x;
                max.y += matrix.m12 * aabb.min.x;
            }
            
            if(matrix.m22 > 0)
            {
                min.y += matrix.m22 * aabb.min.y;
                max.y += matrix.m22 * aabb.max.y;
            }
            else
            {
                min.y += matrix.m22 * aabb.max.y;
                max.y += matrix.m22 * aabb.min.y;
            }
            
            if(matrix.m32 > 0)
            {
                min.y += matrix.m32 * aabb.min.z;
                max.y += matrix.m32 * aabb.max.z;
            }
            else
            {
                min.y += matrix.m32 * aabb.max.z;
                max.y += matrix.m32 * aabb.min.z;
            }
            
            if(matrix.m13 > 0)
            {
                min.z += matrix.m13 * aabb.min.x;
                max.z += matrix.m13 * aabb.max.x;
            }
            else
            {
                min.z += matrix.m13 * aabb.max.x;
                max.z += matrix.m13 * aabb.min.x;
            }
            
            if(matrix.m23 > 0)
            {
                min.z += matrix.m23 * aabb.min.y;
                max.z += matrix.m23 * aabb.max.y;
            }
            else
            {
                min.z += matrix.m23 * aabb.max.y;
                max.z += matrix.m23 * aabb.min.y;
            }
            
            if(matrix.m33 > 0)
            {
                min.z += matrix.m33 * aabb.min.z;
                max.z += matrix.m33 * aabb.max.z;
            }
            else
            {
                min.z += matrix.m33 * aabb.max.z;
                max.z += matrix.m33 * aabb.min.z;
            }
        }
        
        /// <summary>
        /// 获取最近点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 GetClosestPoint(Vector3 v)
        {
            Vector3 closestPoint = Vector3.zero;
            closestPoint.x = v.x < min.x ? min.x : v.x > max.x ? max.x : v.x;
            closestPoint.y = v.y < min.y ? min.y : v.y > max.y ? max.y : v.y;
            closestPoint.z = v.z < min.z ? min.z : v.z > max.z ? max.z : v.z;
            return closestPoint;
        }
        
        /// <summary>
        /// 与射线相交
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="returnNormal">相交点</param>
        public static float CrossForRay(AABB aabb,Ray ray,Vector3 returnNormal)
        {
             //如果未相交则返回这个最大数
             const float kNoIntersection = 1e30f;
             //检查点在矩形边界框内的情况，并计算每个面的距离
             bool inside = true;
             float xt = 0, xn = 0;
             if(ray.Origin.x > aabb.min.x)
             {
                 xt = aabb.min.x - ray.Origin.x;
                 if (xt > ray.Delta.x) return kNoIntersection;
                 xt /= ray.Delta.x;
                 inside = false;
                 xn = -1;
             }
             else if(ray.Origin.x > aabb.max.x)
             {
                 xt = aabb.max.x - ray.Origin.x;
                 if (xt < ray.Delta.x) return kNoIntersection;
                 xt /= ray.Delta.x;
                 inside = false;
                 xn = 1;
             }
             else
             {
                 xt = -1;
             }

             float yt = 0, yn = 0;
             if(ray.Origin.y < aabb.min.y)
             {
                 yt = aabb.min.y - ray.Origin.y;
                 if (yt > ray.Delta.y) return kNoIntersection;
                 yt /= ray.Delta.y;
                 inside = false;
                 yn = -1;
             }
             else if(ray.Origin.y > aabb.max.y)
             {
                 yt = aabb.max.y - ray.Origin.y;
                 if (yt < ray.Delta.y) return kNoIntersection;
                 yt /= ray.Delta.y;
                 inside = false;
                 yn = 1;
             }
             else
             {
                 yt = -1;
             }
                      
             float zt = 0, zn = 0;
             if(ray.Origin.z < aabb.min.z)
             {
                 zt = aabb.min.z - ray.Origin.z;
                 if (zt > ray.Delta.z) return kNoIntersection;
                 zt /= ray.Delta.z;
                 inside = false;
                 zn = -1;
             }
             else if(ray.Origin.z > aabb.max.z)
             {
                 zt = aabb.max.z - ray.Origin.z;
                 if (zt < ray.Delta.z) return kNoIntersection;
                 zt /= ray.Delta.z;
                 inside = false;
                 zn = 1;
             }
             else
             {
                 zt = -1;
             } 
             
             //是否在矩形边界框内
             if(inside)
             {
                 if(returnNormal != null)
                 {
                     returnNormal = ray.Delta;
                     returnNormal.Normalize();
                 }

                 return 0;
             }
             
             //选择最远平面 -- 发生相交的地方
             int which = 0;
             float t = xt;
             if(yt > t)
             {
                 which = 1;
                 t = yt;
             }
             if(zt > t)
             {
                 which = 2;
                 t = zt;
             }

             switch (which)
             {
                 case 0: // 和yz平面相交
                     float y = ray.Origin.y + ray.Delta.y * t;
                     if (y < aabb.min.y || y > aabb.max.y) return kNoIntersection;
                     float z = ray.Origin.z + ray.Delta.z * t;
                     if (z < aabb.min.z || z > aabb.max.z) return kNoIntersection;
                     
                     if(returnNormal != null)
                     {
                         returnNormal.x = xn;
                         returnNormal.y = 0;
                         returnNormal.z = 0;
                     }
                     break;
                 case 1: //和xz平面相交
                     float x = ray.Origin.x + ray.Delta.x * t;
                     if (x < aabb.min.x || x > aabb.max.x) return kNoIntersection;
                     z = ray.Origin.z + ray.Delta.z * t;
                     if (z < aabb.min.z || z > aabb.max.z) return kNoIntersection;
                     
                     if(returnNormal != null)
                     {
                         returnNormal.x = 0;
                         returnNormal.y = yn;
                         returnNormal.z = 0;
                     }
                     break;
                 case 2: //和xy平面相交
                     x = ray.Origin.x + ray.Delta.x * t;
                     if (x < aabb.min.x || x > aabb.max.x) return kNoIntersection;
                     y = ray.Origin.y + ray.Delta.y * t;
                     if (y < aabb.min.y || y > aabb.max.y) return kNoIntersection;
                     
                     if(returnNormal != null)
                     {
                         returnNormal.x = 0;
                         returnNormal.y = 0;
                         returnNormal.z = zn;
                     }
                     break;
             }
            
             //返回交点参数值
             return t;
        }
        #endregion
    }
}