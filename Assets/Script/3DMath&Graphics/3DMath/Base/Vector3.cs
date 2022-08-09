using System;
using UnityEngine;

namespace _3DMath
{
    public class Vector3
    {
        #region 数据
        public float x, y, z;
        #endregion
        
        #region 构造
        public Vector3(){}
        public Vector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        public Vector3(float x,float y,float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        #endregion
        
        #region 内部
        //零向量
        public void Zero()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        
        //乘法
        public void Multiply(float a)
        {
            x *= a;
            y *= a;
            z *= a;
        }
        
        //除法
        public void Division(float a)
        {
            float oneOver = 1.0f / a;
            x *= oneOver;
            y *= oneOver;
            z *= oneOver;
        }
        
        //单位化（标准化）
        public Vector3 Normalize()
        {
            return this / VectorLength(this);
        }
        
        //加法
        public void Add(Vector3 v)
        {
            x += v.x;
            y += v.y;
            z += v.z;
        }
        
        //减法
        public void SubStraction(Vector3 v)
        {
            x -= v.x;
            y -= v.y;
            z -= v.z;
        }
        
        //距离
        public float Distance(Vector3 v)
        {
            return Distance(this,v);
        }
        
        //点乘
        public float Dot(Vector3 v)
        {
            return this * v;
        }
        
        //弧度
        public float Radian(Vector3 v)
        {
            float dotValue = Dot(v);
            return Mathf.Acos(dotValue / (VectorLength(this) * VectorLength(v)));
        }
        
        //角度
        public float Angle(Vector3 v)
        {
            float radian = this.Radian(v);
            return radian * 180 / Mathf.PI;
        }
        
        //叉乘
        public Vector3 Cross(Vector3 v)
        {
            return Cross(this,v);
        }
        #endregion

        #region 静态
        //模长
        public static float  VectorLength(Vector3 v)
        {
            return Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }
        
        //单位化（标准化）
        public static Vector3 Normalize(Vector3 v) => v / VectorLength(v);
        //负向量
        public static Vector3 operator -(Vector3 v) => new Vector3(-v.x, -v.y, -v.z);
        
        //乘法
        public static Vector3 operator *(Vector3 v, float a) => new Vector3(a * v.x,a * v.y,a * v.z);
        public static Vector3 operator *(float a, Vector3 v) => new Vector3(a * v.x,a * v.y,a * v.z);
        //点乘
        public static float operator *(Vector3 v, Vector3 v1) => v.x * v1.x + v.y * v1.y + v.z * v1.z;
        
        //除法
        public static Vector3 operator /(Vector3 v,float a)
        {
            float oneOver = 1.0f / a;
            return new Vector3(oneOver * v.x, oneOver * v.y, oneOver * v.z);
        }

        //加法
        public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        
        //减法
        public static Vector3 operator -(Vector3 v1, Vector3 v2) => new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        
        //距离
        public static float Distance(Vector3 v1,Vector3 v2)
        {
            Vector3 value = v1 - v2;
            return VectorLength(value);
        }
        
        //叉乘
        public static Vector3 Cross(Vector3 v1,Vector3 v2)
        {
            return new Vector3(
                v1.y * v2.z - v1.z * v2.y,
                v1.z * v2.x - v1.x * v2.z,
                v1.x * v2.y - v1.y * v2.x
            );
        }
        
        /// <summary>
        /// 是否是单位向量
        /// </summary>
        /// <returns></returns>
        public static bool IsNormalVector(Vector3 v)
        {
            if(v == null)
            {
                return false;
            }
            return Mathf.Abs(v * v - 1.0f) < 0.0001f;
        }
        #endregion
    }
}