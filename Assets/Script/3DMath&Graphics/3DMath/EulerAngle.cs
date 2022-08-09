using Newtonsoft.Json;
using UnityEngine;

namespace _3DMath
{
    public class EulerAngle:Vector3
    {
        #region 数据
        public float Heading
        {
            get
            {
                return y;
            }
        }
        
        public float Pitch
        {
            get
            {
                return x;
            }
        }
        
        public float Bank
        {
            get
            {
                return z;
            }
        }
        
        public float HeadingAngle
        {
            get
            {
                return Heading * 180 / Mathf.PI;
            }
        }
        
        public float PitchAngle
        {
            get
            {
                return Pitch * 180 / Mathf.PI;
            }
        }
        
        public float BankAngle
        {
            get
            {
                return Bank * 180 / Mathf.PI;
            }
        }
        #endregion
        
        #region 构造
        public EulerAngle()
        {
            
        }
        
        /// <summary>
        /// 欧拉角
        /// </summary>
        /// <param name="heading">弧度</param>
        /// <param name="pitch">弧度</param>
        /// <param name="bank">弧度</param>
        public EulerAngle(float pitch,float heading,float bank):base(pitch,heading,bank)
        {
            
        }
        #endregion
        
        #region 内部
        public void ObjectMatrixToEuler(Matrix4x3 mat)
        {
            EulerAngle euler = ObjectMatrixToEulerAngle(mat);
            this.x = euler.x;
            this.y = euler.y;
            this.z = euler.z;
        }
        
        public void InertialMatrixToEuler(Matrix4x3 mat)
        {
            EulerAngle euler = InertialMatrixToEulerAngle(mat);
            this.x = euler.x;
            this.y = euler.y;
            this.z = euler.z;
        }
        #endregion
        
        #region 静态
        /// <summary>
        /// 检测是否存在万向锁（x轴层级为中的情况）
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static bool CheckUniversalLock(Matrix4x3 mat)
        {
            return Mathf.Abs(mat.m32) > 0.999999f;
        }
        
        /// <summary>
        /// 检测是否存在万向锁（欧拉角的x）
        /// </summary>
        /// <param name="euler"></param>
        /// <returns></returns>
        public static bool CheckUniversalLock(EulerAngle euler)
        {
            return Mathf.Abs(euler.Pitch) > 0.999999f;
        }
        
        /// <summary>
        /// 物体矩阵转换欧拉角
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static EulerAngle ObjectMatrixToEulerAngle(Matrix4x3 mat)
        {
            EulerAngle euler = new EulerAngle();
            if(CheckUniversalLock(mat))
            {
                euler.x = Mathf.Asin(-mat.m32);
                euler.y = Mathf.Atan2(-mat.m23, mat.m11);
                euler.z = 0;
            }
            else
            {
                euler.x = Mathf.Asin(-mat.m32);
                euler.y = Mathf.Atan2(mat.m31, mat.m33);
                euler.z = Mathf.Atan2(mat.m12, mat.m22);
            }

            return euler;
        }
        
        /// <summary>
        /// 惯性矩阵转换欧拉角
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static EulerAngle InertialMatrixToEulerAngle(Matrix4x3 mat)
        {
            Matrix4x3 transposMat = Matrix4x3.Transpose(mat);
            return ObjectMatrixToEulerAngle(transposMat);
        }
        
        /// <summary>
        /// 弧度规范到Π(-180 ~ 180)
        /// </summary>
        /// <returns></returns>
        public static float NormallizePi(float theta)
        {
            theta += Mathf.PI;
            theta -= Mathf.FloorToInt(theta * MathUtility.k1Over2PI) * MathUtility.k2PI;
            theta -= Mathf.PI;
            return theta;
        }
        
        /// <summary>
        /// 欧拉角规范化(x:-90~90,y:-180~180,z:-180~180)
        /// </summary>
        /// <param name="euler"></param>
        /// <returns></returns>
        public static void Canonize(EulerAngle euler)
        {
            euler.x = NormallizePi(euler.x);
            
            if(euler.x < -MathUtility.kPIOver2)
            {
                euler.x = -Mathf.PI - euler.x;
                euler.y += Mathf.PI;
                euler.z += Mathf.PI;
            }
            else if(euler.x > MathUtility.kPIOver2)
            {
                euler.x = Mathf.PI - euler.x;
                euler.y += Mathf.PI;
                euler.z += Mathf.PI;
            }
            
            if(CheckUniversalLock(euler))
            {
                euler.y += euler.z;
                euler.z = 0;
            }
            else
            {
                euler.z = NormallizePi(euler.z);
            }

            euler.y = NormallizePi(euler.y);
        }
        
        /// <summary>
        /// 四元数转换欧拉角
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static EulerAngle QuaternionToEulerAngle(Quaternion q)
        {
            float x = q.x, y = q.y, z = q.z, w = q.w;
            float m32 = -2 * (q.y * q.z - q.w * q.x);
            EulerAngle euler = new EulerAngle();
            //检测万向锁
            if(Mathf.Abs(m32) > 0.999999f)
            {
                euler.x = Mathf.Asin(m32);
                euler.y = Mathf.Atan2( q.w * q.y - q.x * q.z, 0.5f - q.y * q.y - q.z * q.z);
                euler.z = 0;
            }
            else
            {
                euler.x = Mathf.Asin(m32);
                euler.y = Mathf.Atan2( q.w * q.y + q.x * q.z, 0.5f - q.x * q.x - q.y * q.y);
                euler.z = Mathf.Atan2(q.x * q.y + q.w * q.z, 0.5f - q.x * q.x - q.z * q.z);
            }
            return euler;
        }
        #endregion
    }
}