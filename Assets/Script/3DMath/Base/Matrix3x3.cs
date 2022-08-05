using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace _3DMath
{
    public class Matrix3x3
    {
        #region 数据
        //旋转类型
        public enum AxisType
        {
            x,
            y,
            z
        }
        public float m11, m12, m13, m21, m22, m23, m31, m32, m33;
        #endregion
        
        #region 构造函数
        public Matrix3x3()
        {
            
        }
        public Matrix3x3(float m11,float m12,float m13,float m21,float m22,float m23,float m31,float m32,float m33)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }
        #endregion
        
        #region 内部
        //向量矩阵乘法
        public Vector3 MultiplyVector(Vector3 vec)
        {
            return vec * this;
        }
        
        //矩阵矩阵乘法
        public Matrix3x3 MultiplyMatrix(Matrix3x3 matrix)
        {
            return this * matrix;
        }
        #endregion
        
        #region 静态
        /// <summary>
        /// 向量矩阵乘法
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 vec, Matrix3x3 matrix) =>
            new Vector3(
                vec.x * matrix.m11 + vec.y * matrix.m21 + vec.z * matrix.m31,
                vec.x * matrix.m12 + vec.y * matrix.m22 + vec.z * matrix.m32,
                vec.x * matrix.m13 + vec.y * matrix.m23 + vec.z * matrix.m33
                );
        
        /// <summary>
        /// 矩阵矩阵乘法
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Matrix3x3 operator *(Matrix3x3 matrix1,Matrix3x3 matrix2) => 
            new Matrix3x3(
                matrix1.m11 * matrix2.m11 + matrix1.m12 * matrix2.m21 + matrix1.m13 * matrix2.m31,
                matrix1.m11 * matrix2.m12 + matrix1.m12 * matrix2.m22 + matrix1.m13 * matrix2.m32,
                matrix1.m11 * matrix2.m13 + matrix1.m12 * matrix2.m23 + matrix1.m13 * matrix2.m33,
                matrix1.m21 * matrix2.m11 + matrix1.m22 * matrix2.m21 + matrix1.m23 * matrix2.m31,
                matrix1.m21 * matrix2.m12 + matrix1.m22 * matrix2.m22 + matrix1.m23 * matrix2.m32,
                matrix1.m21 * matrix2.m13 + matrix1.m22 * matrix2.m23 + matrix1.m23 * matrix2.m33,
                matrix1.m31 * matrix2.m11 + matrix1.m32 * matrix2.m21 + matrix1.m33 * matrix2.m31,
                matrix1.m31 * matrix2.m12 + matrix1.m32 * matrix2.m22 + matrix1.m33 * matrix2.m32,
                matrix1.m31 * matrix2.m13 + matrix1.m32 * matrix2.m23 + matrix1.m33 * matrix2.m33
                );
        /// <summary>
        /// 转置
        /// </summary>
        /// <returns></returns>
        public static Matrix3x3 Transpose(Matrix3x3 mat)
        {
            return new Matrix3x3(mat.m11,mat.m21,mat.m31,mat.m12,mat.m22,mat.m32,mat.m13,mat.m23,mat.m33);
        }
        
        /// <summary>
        /// 设置旋转矩阵
        /// </summary>
        /// <param name="type">轴类型</param>
        /// <param name="theta">弧度</param>
        /// <returns></returns>
        public static Matrix3x3 SetRotate(AxisType type,float theta)
        {
            Matrix3x3 matrix = new Matrix3x3();
            float sin = Mathf.Sin(theta), cos = Mathf.Cos(theta);
            switch(type)
            {
                case AxisType.x:
                    matrix.m11 = 1;
                    matrix.m12 = 0;
                    matrix.m13 = 0;
                    matrix.m21 = 0;
                    matrix.m22 = cos;
                    matrix.m23 = sin;
                    matrix.m31 = 0;
                    matrix.m32 = -sin;
                    matrix.m33 = cos;
                    break;
                case AxisType.y:
                    matrix.m11 = cos;
                    matrix.m12 = 0;
                    matrix.m13 = -sin;
                    matrix.m21 = 0;
                    matrix.m22 = 1;
                    matrix.m23 = 0;
                    matrix.m31 = sin;
                    matrix.m32 = 0;
                    matrix.m33 = cos;
                    break;
                case AxisType.z:
                    matrix.m11 = cos;
                    matrix.m12 = sin;
                    matrix.m13 = 0;
                    matrix.m21 = -sin;
                    matrix.m22 = cos;
                    matrix.m23 = 0;
                    matrix.m31 = 0;
                    matrix.m32 = 0;
                    matrix.m33 = 1;
                    break;
            }

            return matrix;
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix3x3 SetScale(float x, float y, float z) => new Matrix3x3(x,0,0,0,y,0,0,0,z);
        public static Matrix3x3 SetScale(Vector3 v) => new Matrix3x3(v.x, 0, 0, 0, v.y, 0, 0, 0, v.z);
        
        /// <summary>
        /// 投影
        /// </summary>
        /// <param name="normal">垂直投影平面的法线</param>
        /// <returns></returns>
        public static Matrix3x3 SetProjection(Vector3 normal)
        {
            if(Vector3.IsNormalVector(normal) == false)
            {
                return null;
            }

            Matrix3x3 matrix = new Matrix3x3();
            matrix.m11 = 1 - normal.x * normal.x;
            matrix.m22 = 1 - normal.y * normal.y;
            matrix.m33 = 1 - normal.z * normal.z;

            matrix.m12 = matrix.m21 = -normal.x * normal.y;
            matrix.m13 = matrix.m31 = -normal.x * normal.z;
            matrix.m23 = matrix.m32 = -normal.y * normal.z;

            return matrix;
        }
        
        /// <summary>
        /// 轴向镜像
        /// </summary>
        /// <param name="axis">轴类型</param>
        /// <returns></returns>
        public static Matrix3x3 SetReflect(AxisType axis)
        {
            Matrix3x3 matrix = new Matrix3x3();
            switch(axis)
            {
                case AxisType.x:
                    matrix.m11 = -1;
                    matrix.m12 = 0;
                    matrix.m13 = 0;
                    matrix.m21 = 0;
                    matrix.m22 = 1;
                    matrix.m23 = 0;
                    matrix.m31 = 0;
                    matrix.m32 = 0;
                    matrix.m33 = 1;
                    break;
                case AxisType.y:
                    matrix.m11 = 1;
                    matrix.m12 = 0;
                    matrix.m13 = 0;
                    matrix.m21 = 0;
                    matrix.m22 = -1;
                    matrix.m23 = 0;
                    matrix.m31 = 0;
                    matrix.m32 = 0;
                    matrix.m33 = 1;
                    break;
                case AxisType.z:
                    matrix.m11 = 1;
                    matrix.m12 = 0;
                    matrix.m13 = 0;
                    matrix.m21 = 0;
                    matrix.m22 = 1;
                    matrix.m23 = 0;
                    matrix.m31 = 0;
                    matrix.m32 = 0;
                    matrix.m33 = -1;
                    break;
            }

            return matrix;
        }
        
        /// <summary>
        /// 任意平面镜像
        /// </summary>
        /// <param name="normal">垂直平面的法线</param>
        /// <returns></returns>
        public static Matrix3x3 SetReflect(Vector3 normal)
        {
            if(Vector3.IsNormalVector(normal) == false)
            {
                return null;
            }

            Matrix3x3 matrix = new Matrix3x3();
            float ax = -2 * normal.x, ay = -2 * normal.y,az = -2 * normal.z;
            matrix.m11 = 1 + ax * normal.x;
            matrix.m22 = 1 + ay * normal.y;
            matrix.m33 = 1 + az * normal.z;
            matrix.m12 = matrix.m21 = ax * normal.y;
            matrix.m13 = matrix.m31 = ax * normal.z;
            matrix.m23 = matrix.m32 = ay * normal.z;
            return matrix;
        }
        
        /// <summary>
        /// 轴向切变
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="s">切变程度</param>
        /// <param name="t">切变程度</param>
        /// <returns></returns>
        public static Matrix3x3 SetShear(AxisType axis,float s,float t)
        {
            Matrix3x3 mat = new Matrix3x3();
            switch (axis)
            {
                case AxisType.x:
                    mat.m11 = 1;
                    mat.m12 = s;
                    mat.m13 = t;
                    mat.m21 = 0;
                    mat.m22 = 1;
                    mat.m23 = 0;
                    mat.m31 = 0;
                    mat.m32 = 0;
                    mat.m33 = 1;
                    break;
                case AxisType.y:
                    mat.m11 = 1;
                    mat.m12 = 0;
                    mat.m13 = 0;
                    mat.m21 = s;
                    mat.m22 = 1;
                    mat.m23 = t;
                    mat.m31 = 0;
                    mat.m32 = 0;
                    mat.m33 = 1;
                    break;
                case AxisType.z:
                    mat.m11 = 1;
                    mat.m12 = 0;
                    mat.m13 = 0;
                    mat.m21 = 0;
                    mat.m22 = 1;
                    mat.m23 = 0;
                    mat.m31 = s;
                    mat.m32 = t;
                    mat.m33 = 1;
                    break;
            }

            return mat;
        }
        
        /// <summary>
        /// 行列式
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static float Determinant(Matrix3x3 mat)
        {
            return mat.m11 * (mat.m22 * mat.m33 - mat.m23 * mat.m32) +
                   mat.m12 * (mat.m23 * mat.m31 - mat.m21 * mat.m33) +
                   mat.m13 * (mat.m21 * mat.m32 - mat.m22 * mat.m31);
        }
        
        /// <summary>
        /// 矩阵的逆
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix3x3 Inverse(Matrix3x3 mat)
        {
            float det = Determinant(mat);
            if(Mathf.Abs(det) < 0.00001f)
            {
                return null;
            }

            float oneOverDet = 1 / det;
            Matrix3x3 standarAdjMat = StandardAdjMatrix(mat);
            return new Matrix3x3(
                standarAdjMat.m11 * oneOverDet,
                standarAdjMat.m12 * oneOverDet,
                standarAdjMat.m13 * oneOverDet,
                standarAdjMat.m21 * oneOverDet,
                standarAdjMat.m22 * oneOverDet,
                standarAdjMat.m23 * oneOverDet,
                standarAdjMat.m31 * oneOverDet,
                standarAdjMat.m32 * oneOverDet,
                standarAdjMat.m33 * oneOverDet
            );
        }
        
        /// <summary>
        /// 标准伴随矩阵
        /// </summary>
        /// <returns></returns>
        public static Matrix3x3 StandardAdjMatrix(Matrix3x3 mat)
        {
            return Transpose(AdjunctMatrix(mat));
        }
        
        /// <summary>
        /// 代数余子矩阵
        /// </summary>
        /// <returns></returns>
        public static Matrix3x3 AdjunctMatrix(Matrix3x3 mat)
        {
            return new Matrix3x3(
                mat.m22 * mat.m33 - mat.m23 * mat.m32,
                mat.m23 * mat.m31 - mat.m21 * mat.m33,
                mat.m21 * mat.m32 - mat.m22 * mat.m31,
                mat.m13 * mat.m32 - mat.m12 * mat.m33,
                mat.m11 * mat.m33 - mat.m13 * mat.m31,
                mat.m12 * mat.m31 - mat.m11 * mat.m32,
                mat.m12 * mat.m23 - mat.m13 * mat.m22,
                mat.m13 * mat.m21 - mat.m11 * mat.m23,
                mat.m11 * mat.m22 - mat.m12 * mat.m21
            );
        }
        #endregion
    }
}