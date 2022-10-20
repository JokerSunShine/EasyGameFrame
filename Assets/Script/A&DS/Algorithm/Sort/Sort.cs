using System;
using System.Linq;
using Common.DataStruct.Queue.ChainQueue;
using Common.OneWayChainList;
using DataStruct.Graph.MatrixGraph;
using UnityEngine;

namespace Algorithm.Sort
{
    public class Sort<T>
    {
        #region 冒泡排序
        public static void BubbleSort(T[] array,Func<T,T,int> compareFunc)
        {
            for(int i = 1;i < array.Length;i++)
            {
                for(int j = 0;j < array.Length - 1;j++)
                {
                    if(compareFunc(array[j],array[i]) > 0)
                    {
                        T temp = array[j];
                        array[j] = array[i];
                        array[i] = temp;
                    }
                }
            }
        }
        #endregion
        
        #region 选择排序
        public static void SelectSort(T[] array,Func<T,T,int> compareFunc)
        {
            for(int i = 0;i < array.Length - 1;i++)
            {
                int min = i;
                for(int j = i + 1;j < array.Length;j++)
                {
                    if(compareFunc(array[j],array[min]) < 0)
                    {
                        min = j;
                    }
                }
                if(min != i)
                {
                    T minValue = array[min];
                    array[min] = array[i];
                    array[i] = minValue;
                }
            }
        }
        #endregion
        
        #region 插入排序
        public static void InsertSort(T[] array,Func<T,T,int> compareFunc)
        {
            for(int i = 1;i < array.Length;i++)
            {
                T insertValue = array[i];
                int j = i;
                //如果插入数据比前面的数据小，则将前面的数据放到后面
                while(j > 0 && compareFunc(insertValue,array[j - 1]) < 0)
                {
                    array[j] = array[j - 1];
                    j--;
                }
                //当插入数据不能往前放后，则将插入数据放到停止的地方
                if(j != i)
                {
                    array[j] = insertValue;
                }
            }
        }
        #endregion
        
        #region 希尔排序
        public static void ShellSort(T[] array,Func<T,T,int> compareFunc)
        {
            float length = array.Length;
            //对比间隔
            int gap = (int) Math.Floor(length * 0.5f);
            
            while(gap > 0)
            {
                for(int i = gap;i < array.Length;i++)
                {
                    T insertValue = array[i];
                    //间隔前的数据下标
                    int j = i - gap;
                    while (j >= 0 && compareFunc(insertValue,array[j]) < 0)
                    {
                        //如果对比替换成功，继续尝试往前对比和替换
                        array[j + gap] = array[j];
                        j -= gap;
                    }
                    //最后将对比替换后的数据放到目标点，类似插排
                    array[j + gap] = insertValue;
                }
                //缩小对比间隔
                gap = (int)Math.Floor(gap * 0.5f);
            }
        }
        #endregion
        
        #region 归并排序
        public static T[] MergeSort(T[] array,Func<T,T,int> compareFunc)
        {
            if(array.Length < 2)
            {
                return array;
            }

            int middle = (int) Math.Floor(array.Length * 0.5f);
            T[] left = array.Take(middle).ToArray();
            T[] right = array.Skip(middle).ToArray();
            //通过递归，先将最小的组两组进行排序，依次向上到完整的排序
            return Merge(MergeSort(left,compareFunc), MergeSort(right,compareFunc),compareFunc);
        }
        
        private static T[] Merge(T[] left,T[] right,Func<T,T,int> compareFunc)
        {
            T[] result = new T[left.Length + right.Length];
            //一个指向左数组，一个指向右数组
            int resultIndex = 0,leftIndex = 0,rightIndex = 0;
            while(leftIndex < left.Length && rightIndex < right.Length)
            {
                result[resultIndex++] = compareFunc(left[leftIndex], right[rightIndex]) <= 0
                    ? left[leftIndex++]
                    : right[rightIndex++];
            }
            while(leftIndex < left.Length)
            {
                result[resultIndex++] = left[leftIndex++];
            }
            while(rightIndex < right.Length)
            {
                result[resultIndex++] = right[rightIndex++];
            }

            return result;
        }
        #endregion
        
        #region 堆排序
        // public static T[] HeapSort(T[] array,Func<T,T,int> compareFunc)
        // {
        //     
        // }
        #endregion
    }
}