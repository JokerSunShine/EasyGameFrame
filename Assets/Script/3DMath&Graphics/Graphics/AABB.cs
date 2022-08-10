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
        #endregion
    }
}