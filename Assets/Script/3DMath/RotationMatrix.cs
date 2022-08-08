using UnityEngine;

namespace _3DMath
{
    public class RotationMatrix:Matrix4x3
    {
        #region 构造
        public RotationMatrix():base()
        {
            
        }
        public RotationMatrix(float m11,float m12,float m13,float m21,float m22,float m23,float m31,float m32,float m33):base(m11,m12,m13,m21,m22,m23,m31,m32,m33)
        {
            
        }
        #endregion
        
        #region 内部
        /// <summary>
        /// 惯性坐标转物体坐标
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 InertialToObject(Vector3 v)
        {
            return MultiplyVector(v);
        }
        
        /// <summary>
        /// 物体坐标转惯性坐标
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 ObjectToInertial(Vector3 v)
        {
            Matrix4x3 mat = Matrix4x3.Transpose(this);
            return mat.MultiplyVector(v);
        }
        
        /// <summary>
        /// 欧拉角转换旋转矩阵
        /// </summary>
        /// <param name="euler"></param>
        public void Transition(EulerAngle euler)
        {
            RotationMatrix matrix = EulerAngleToRotationMatrix(euler);
            this.m11 = matrix.m11;
            this.m12 = matrix.m12;
            this.m13 = matrix.m13;
            this.m21 = matrix.m21;
            this.m22 = matrix.m22;
            this.m23 = matrix.m23;
            this.m31 = matrix.m31;
            this.m32 = matrix.m32;
            this.m33 = matrix.m33;
        }
        #endregion

        #region 静态
        /// <summary>
        /// 欧拉角转换矩阵
        /// </summary>
        /// <param name="euler"></param>
        /// <returns></returns>
        public static RotationMatrix EulerAngleToRotationMatrix(EulerAngle euler)
        {
            float sinh = Mathf.Sin(euler.Heading),
                cosh = Mathf.Cos(euler.Heading),
                sinb = Mathf.Sin(euler.Bank),
                cosb = Mathf.Cos(euler.Bank),
                sinp = Mathf.Sin(euler.Pitch),
                cosp = Mathf.Cos(euler.Pitch);
            return new RotationMatrix(
                cosh * cosb + sinh * sinp * sinb, 
                sinb * cosp,
                -sinh * cosb + cosh * sinp * sinb,
                -cosh * sinb + sinh * sinp * cosb, 
                cosb * cosp,
                sinb * sinh + cosh * sinp * cosb,
                sinh * cosp,
                -sinp,
                cosh * cosp
            );
        }
        
        /// <summary>
        /// 四元数转换矩阵
        /// </summary>
        /// <returns></returns>
        public static RotationMatrix QuaternionToRotationMatrix(Quaternion q)
        {
            RotationMatrix matrix = new RotationMatrix();
            matrix.m11 = 1 - 2 * (q.y * q.y + q.z * q.z);
            matrix.m12 = 2 * (q.x * q.y + q.w * q.z);
            matrix.m13 = 2 * (q.x * q.z - q.w * q.y);
            matrix.m21 = 2 * (q.x * q.y - q.w * q.z);
            matrix.m22 = 1 - 2 * (q.x * q.x + q.z * q.z);
            matrix.m23 = 2 * (q.y * q.z + q.w * q.x);
            matrix.m31 = 2 * (q.x * q.z + q.w * q.y);
            matrix.m32 = 2 * (q.y * q.z - q.w * q.x);
            matrix.m33 = 1 - 2 * (q.x * q.x + q.y * q.y);
            return matrix;
        }
        #endregion
    }
}