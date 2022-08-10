using UnityEngine;
using Vector3 = _3DMath.Vector3;

namespace Graphics
{
    public enum PolygonType
    {
        //正多边形
        RegularPolygon = 0,
        //凸多边形
        ConvexPolygon = 1,
        //凹多边形
        ConcavePolygon = 2,
    }
    
    public class Polygon
    {
        #region 静态
        public static PolygonType GetPolygonAngleType(Vector3[] point)
        {
            int pointCount = point.Length;
            float angleSum = 0;
            Vector3 e1 = Vector3.zero,e2 = Vector3.zero;
            for(int i = 0;i < pointCount;i++)
            {
                e1 = i == 0 ? point[pointCount - 1] - point[i] : point[i - 1] - point[i];
                e2 = i == pointCount - 1 ? point[0] - point[i] : point[i + 1] - point[i];
                
                e1.Normalize();
                e2.Normalize();

                float dot = e1.Dot(e2);
                float theta = Mathf.Acos(dot);
                angleSum += theta;
            }

            float InternalAngleSum = (pointCount - 2) * Mathf.PI;

            return angleSum < InternalAngleSum ? PolygonType.ConcavePolygon :
                angleSum > InternalAngleSum ? PolygonType.ConvexPolygon : PolygonType.RegularPolygon;
        }
        #endregion

    }
}