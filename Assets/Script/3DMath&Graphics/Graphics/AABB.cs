using System.Diagnostics;
using _3DMath;
using UnityEngine;
using Vector3 = _3DMath.Vector3;
using Framework;

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
        /// 与射线相交（参考图形图像编程精粹）
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
        
        /// <summary>
        /// 静态轴对齐矩形边界框相交检测
        /// </summary>
        /// <param name="aabb1"></param>
        /// <param name="aabb2"></param>
        /// <param name="crossAABB"></param>
        /// <returns></returns>
        public static bool StaticCrossAABBs(AABB aabb1,AABB aabb2,out AABB crossAABB)
        {
            crossAABB = new AABB();
            //判断是否重叠
            if (aabb1.min.x > aabb1.max.x) return false;
            if (aabb1.max.x < aabb1.min.x) return false;
            if (aabb1.min.y > aabb1.max.y) return false;
            if (aabb1.max.y < aabb1.min.y) return false;
            if (aabb1.min.z > aabb1.max.z) return false;
            if (aabb1.max.z < aabb1.min.z) return false;
            
            //有重叠,计算重叠部分aabb
            crossAABB.min.x = Mathf.Max(aabb1.min.x, aabb2.min.x);
            crossAABB.max.x = Mathf.Min(aabb1.max.x, aabb2.max.x);
            crossAABB.min.y = Mathf.Max(aabb1.min.x, aabb2.min.x);
            crossAABB.max.y = Mathf.Min(aabb1.max.x, aabb2.max.x);
            crossAABB.min.z = Mathf.Max(aabb1.min.x, aabb2.min.x);
            crossAABB.max.z = Mathf.Min(aabb1.max.x, aabb2.max.x);
            return true;
        }
        
        /// <summary>
        /// 动态轴对齐矩形边界框相交检测（参考图形图像编程精粹）
        /// </summary>
        /// <param name="stationaryBox"></param>
        /// <param name="movingBox"></param>
        /// <param name="d"></param>
        /// <returns>大于1：未相交</returns>
        public static float DynamicCrossAABBs(AABB stationaryBox,AABB movingBox,Vector3 d)
        {
            const float KNoIntersection = 1e10f;

            float tEnter = 0;
            float tLeave = 1;
            
            //计算每一维上的重叠部分，再将这个重叠部分和前面的重叠部分作相交
            //如果有一维上重叠部分为0则返回（不会相交）
            
            //检查x轴
            if(d.x == 0)
            {
                //Empty or infinite inverval on x
                if((stationaryBox.min.x >= movingBox.max.x) || (stationaryBox.max.x <= movingBox.min.x))
                {
                    //Empty time interval,so no intersection
                    return KNoIntersection;
                }
                //Inifinite time interval - no update necessary
            }
            else
            {
                //Divide once
                float oneOverD = 1 / d.x;
                //Compute time value when they begin and end overlapping
                float xEnter = (stationaryBox.min.x - movingBox.max.x) * oneOverD;
                float xLeave = (stationaryBox.max.x - movingBox.min.x) * oneOverD;
                //Check for interval out of order
                if(xEnter > xLeave)
                {
                    Utility.Swap(xEnter,xLeave);
                }
                //Update interval
                if (xEnter > tEnter) tEnter = xEnter;
                if (xLeave < tLeave) tLeave = xLeave;
                //Check if this resulted in empty interval
                if(tEnter > tLeave)
                {
                    return KNoIntersection;
                }
            }
            
            //检查y轴
            if(d.y == 0)
            {
                //Empty or infinite inverval on y
                if((stationaryBox.min.y >= movingBox.max.y) || (stationaryBox.max.y <= movingBox.min.y))
                {
                    //Empty time interval,so no intersection
                    return KNoIntersection;
                }
                //Inifinite time interval - no update necessary
            }
            else
            {
                //Divide once
                float oneOverD = 1 / d.y;
                //Compute time value when they begin and end overlapping
                float yEnter = (stationaryBox.min.y - movingBox.max.y) * oneOverD;
                float yLeave = (stationaryBox.max.y - movingBox.min.y) * oneOverD;
                //Check for interval out of order
                if(yEnter > yLeave)
                {
                    Utility.Swap(yEnter,yLeave);
                }
                //Update interval
                if (yEnter > tEnter) tEnter = yEnter;
                if (yLeave < tLeave) tLeave = yLeave;
                //Check if this resulted in empty interval
                if(tEnter > tLeave)
                {
                    return KNoIntersection;
                }
            }
            
            //检查z轴
            if(d.z == 0)
            {
                //Empty or infinite inverval on z
                if((stationaryBox.min.z >= movingBox.max.z) || (stationaryBox.max.z <= movingBox.min.z))
                {
                    //Empty time interval,so no intersection
                    return KNoIntersection;
                }
                //Inifinite time interval - no update necessary
            }
            else
            {
                //Divide once
                float oneOverD = 1 / d.z;
                //Compute time value when they begin and end overlapping
                float zEnter = (stationaryBox.min.z - movingBox.max.z) * oneOverD;
                float zLeave = (stationaryBox.max.z - movingBox.min.z) * oneOverD;
                //Check for interval out of order
                if(zEnter > zLeave)
                {
                    Utility.Swap(zEnter,zLeave);
                }
                //Update interval
                if (zEnter > tEnter) tEnter = zEnter;
                if (zLeave < tLeave) tLeave = zLeave;
                //Check if this resulted in empty interval
                if(tEnter > tLeave)
                {
                    return KNoIntersection;
                }
            }
            //好了，相交，返回焦点参数值
            return tEnter;
        }
        #endregion
    }
}