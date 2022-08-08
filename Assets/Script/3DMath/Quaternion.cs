using UnityEngine;

namespace _3DMath
{
    public class Quaternion
    {
        #region 数据
        public float w, x, y, z;
        public static readonly Quaternion identityQuaternion = new Quaternion(0, 0, 0, 1);
        #endregion
        
        #region 构造
        public Quaternion()
        {
            
        }
        
        public Quaternion(float x,float y,float z,float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        #endregion
        
        #region 内部
        public void Identity()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.w = 1;
        }
        
        /// <summary>
        /// 设置为绕X轴旋转指定角度的四元数
        /// </summary>
        /// <param name="theta"></param>
        public void SetRotateAboutX(float theta)
        {
            float thetaOver2 = theta * 0.5f;

            w = Mathf.Cos(thetaOver2);
            x = Mathf.Sin(thetaOver2);
            y = 0;
            z = 0;
        }
        
        /// <summary>
        /// 设置为绕Y轴旋转指定角度的四元数
        /// </summary>
        /// <param name="theta"></param>
        public void SetRotateAboutY(float theta)
        {
            float thetaOver2 = theta * 0.5f;

            w = Mathf.Cos(thetaOver2);
            x = 0;
            y = Mathf.Sin(thetaOver2);
            z = 0;
        }
        
        /// <summary>
        /// 设置为绕Z轴旋转指定角度的四元数
        /// </summary>
        /// <param name="theta"></param>
        public void SetRotateAboutZ(float theta)
        {
            float thetaOver2 = theta * 0.5f;

            w = Mathf.Cos(thetaOver2);
            x = 0;
            y = 0;
            z = Mathf.Sin(thetaOver2);
        }
        
        /// <summary>
        /// 设置成绕任意轴旋转指定角度的四元数
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="theta"></param>
        public void SetRotateAboutAxis(Vector3 axis,float theta)
        {
            Vector3 rotateAxis = axis;
            if(Vector3.IsNormalVector(rotateAxis) == false)
            {
                rotateAxis = rotateAxis.Normalize();
            }
            
            float thetaOver2 = theta * 0.5f;
            
            w = Mathf.Cos(thetaOver2);
            x = Mathf.Sin(thetaOver2) * rotateAxis.x;
            y = Mathf.Sin(thetaOver2) * rotateAxis.y;
            z = Mathf.Sin(thetaOver2) * rotateAxis.z;
        }
        
        /// <summary>
        /// 获取旋转角
        /// </summary>
        /// <returns></returns>
        public float GetRotationAngle()
        {
            return GetRotationAngle(this);
        }
        
        /// <summary>
        /// 获取旋转轴
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRotationAxis()
        {
            return GetRotationAxis(this);
        }
        
        /// <summary>
        /// 规范化
        /// </summary>
        public void Normalize()
        {
            Normallize(this);
        }
        #endregion
        
        #region 静态
        /// <summary>
        /// 获取旋转角
        /// </summary>
        /// <returns></returns>
        public static float GetRotationAngle(Quaternion q)
        {
            float thetaOver2 = MathUtility.SafeACos(q.w);
            return thetaOver2 * 2;
        }
        
        /// <summary>
        /// 获取旋转轴
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRotationAxis(Quaternion q)
        {
            float sinThetaOver2 = Mathf.Sqrt(1 - q.w * q.w);
            float oneOverSinThetaOver2 = 1 / sinThetaOver2;
            return new Vector3(q.x * oneOverSinThetaOver2, q.y * oneOverSinThetaOver2, q.z * oneOverSinThetaOver2);
        }
        
        /// <summary>
        /// 叉乘
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public static Quaternion operator *(Quaternion q1,Quaternion q2)
        {
            return new Quaternion(
                q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y,
                q1.w * q2.y + q1.y * q2.w + q1.z * q2.x - q1.x * q2.z,
                q1.w * q2.z + q1.z * q2.w + q1.x * q2.y - q1.y * q2.x,
                q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
            );
        }
        
        /// <summary>
        /// 获取模长
        /// </summary>
        /// <returns></returns>
        public static float GetLength(Quaternion q)
        {
            return Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z);
        }
        
        /// <summary>
        /// 规范化四元数
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static void Normallize(Quaternion q)
        {
            float length = GetLength(q);

            if(length <= 0)
            {
                return;
            }
            
            if (length > 0)
            {
                float oneOverMag = 1 / length;
                q.w *= oneOverMag;
                q.x *= oneOverMag;
                q.y *= oneOverMag;
                q.z *= oneOverMag;
            }
        }
        
        /// <summary>
        /// 点乘
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public static float Dot(Quaternion q1,Quaternion q2)
        {
            return q1.w * q2.w + q1.x * q2.x + q1.y * q2.y + q1.z * q2.z;
        }
        
        /// <summary>
        /// 四元数共轭
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Quaternion Conjugate(Quaternion q)
        {
            return new Quaternion(-q.x,-q.y,-q.z,q.w);
        }
        
        /// <summary>
        /// 四元数求幂
        /// </summary>
        /// <param name="q">四元数</param>
        /// <param name="exp">幂</param>
        /// <returns></returns>
        public static Quaternion Pow(Quaternion q,float exp)
        {
            //w = cosθ/ 2，如果大于1，则表示sinθ/2 = 0,则没必要计算xyz
            if(Mathf.Abs(q.w) > 0.99999f)
            {
                return q;
            }

            float alpha = Mathf.Acos(q.w);
            float newAlpha = exp * alpha;
            float mul = Mathf.Sin(newAlpha) / Mathf.Sin(alpha);

            return new Quaternion(q.x * mul, q.y * mul, q.z * mul, Mathf.Cos(newAlpha));
        }
        #endregion
        
        #region 转换
        /// <summary>
        /// 矩阵转换四元数
        /// </summary>
        /// <param name="martix">矩阵</param>
        /// <returns></returns>
        public static Quaternion RotationMatrixToQuaternion(RotationMatrix martix)
        {
            float m11 = martix.m11;
            float m12 = martix.m12;
            float m13 = martix.m13;
            float m21 = martix.m21;
            float m22 = martix.m22;
            float m23 = martix.m23;
            float m31 = martix.m31;
            float m32 = martix.m32;
            float m33 = martix.m33;

            float WSquared = m11 + m22 + m33;
            float XSquared = m11 - m22 - m33;
            float YSquared = -m11 + m22 - m33;
            float ZSquared = -m11 - m22 + m33;

            float maxIndex = 0;
            float maxSquared = WSquared;
            if(maxSquared < XSquared)
            {
                maxIndex = 1;
                maxSquared = XSquared;
            }
            if(maxSquared < YSquared)
            {
                maxIndex = 2;
                maxSquared = YSquared;
            }
            if(maxSquared < ZSquared)
            {
                maxIndex = 3;
                maxSquared = ZSquared;
            }

            Quaternion q = new Quaternion();
            switch(maxIndex)
            {
                case 0:
                    q.w = maxSquared;
                    q.x = (m23 - m32) / (4 * maxSquared);
                    q.y = (m31 - m13) / (4 * maxSquared);
                    q.z = (m12 - m21) / (4 * maxSquared);
                    break;
                case 1:
                    q.w = (m23 - m32) / (4 * maxSquared);
                    q.x = maxSquared;
                    q.y = (m12 + m21) / (4 * maxSquared);
                    q.z = (m31 + m13) / (4 * maxSquared);
                    break;
                case 2:
                    q.w = (m31 - m13) / (4 * maxSquared);
                    q.x = (m12 + m21) / (4 * maxSquared);
                    q.y = maxSquared;
                    q.z = (m23 + m32) / (4 * maxSquared);
                    break;
                case 3:
                    q.w = (m12 - m21) / (4 * maxSquared);
                    q.x = (m31 + m13) / (4 * maxSquared);
                    q.y = (m23 + m32) / (4 * maxSquared);
                    q.z = maxSquared;
                    break;
            }
            return q;
        }
        
        /// <summary>
        /// 欧拉角转换四元数
        /// </summary>
        /// <param name="euler"></param>
        /// <returns></returns>
        public static Quaternion EulerAngleToQuaternion(EulerAngle euler)
        {
            float pOver2 = euler.Pitch * 0.5f, hOver2 = euler.Heading * 0.5f, bOver2 = euler.Bank * 0.5f;
            float sinP = Mathf.Sin(pOver2),
                cosP = Mathf.Cos(pOver2),
                sinH = Mathf.Sin(hOver2),
                cosH = Mathf.Cos(hOver2),
                sinB = Mathf.Sin(bOver2),
                cosB = Mathf.Cos(bOver2);
            return new Quaternion(
                cosH * sinP * cosB + sinH * cosP * sinB,
                sinH * cosP * cosB - cosH * sinP * sinB,
                cosH * cosP * sinB - sinH * sinP * cosB,
                cosH * cosP * cosB + sinH * sinP * sinB
                );
        }
        #endregion
    }
}