using System;

namespace DataStruct.HashTable
{
    //散列表键值通用算法
    public class HashTableKeyAlgorithm
    {
        /// <summary>
        /// 线性探测
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="length">模长</param>
        /// <param name="step">步数</param>
        /// <returns></returns>
        public static int LinearProbing(object key,int length,int step = 0)
        {
            if(key == null)
            {
                return 0;
            }

            return (key.GetHashCode() + step) % length;
        }
        
        /// <summary>
        /// 二次探测
        /// </summary>
        /// <param name="key"></param>
        /// <param name="length"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static int QuadraticProbing(object key,int length,int step = 0)
        {
            if(key == null)
            {
                return 0;
            }

            return (int)(key.GetHashCode() + Math.Pow((double) step, 2)) % length;
        }
        
        /// <summary>
        /// 双重哈希
        /// </summary>
        /// <param name="key"></param>
        /// <param name="length"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static int DoubleHash(object key,int length,int step = 0)
        {
            if(key == null)
            {
                return 0;
            }

            int hashCode = key.GetHashCode();
            if(step > 0)
            {
                hashCode += step * (hashCode + 1) % (length - 2);
            }
            return hashCode % length;
        }
    }
}