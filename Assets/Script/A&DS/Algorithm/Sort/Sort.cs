using System;
using System.Linq;
using Common.DataStruct.Queue.ChainQueue;
using Common.OneWayChainList;
using DataStruct.Graph.MatrixGraph;
using DataStruct.HashTable;
using DataStruct.HashTable.HashTable_ChainList;
using DataStruct.Tree.Heap;
using DataStruct.Tree.Heap.MaxHeap;
using Packages.FW_Common.Other;
using Script.DataStruct.Tree.Heap.MinHeap;
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
                        Swap(array, i, j);
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
                    Swap(array, min, i);
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
        /// <summary>
        /// 小顶堆数据结构排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="compareFunc"></param>
        /// <returns></returns>
        public static T[] HeapSortByMinHeap(T[] array,Func<T,T,int> compareFunc)
        {
            IHeap<T> heap = new MinHeap<T>(compareFunc,array);
            int length = heap.Count;
            for(int i = 0;i < length;i++)
            {
                array[i] = heap.RemoveByIndex(0);
            }

            return array;
        }
        
        /// <summary>
        /// 大顶堆数据结构排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="compareFunc"></param>
        /// <returns></returns>
        public static T[] HeapSortByMaxHeap(T[] array,Func<T,T,int> compareFunc)
        {
            IHeap<T> heap = new MaxHeap<T>(compareFunc,array);
            int length = heap.Count;
            for(int i = length - 1;i >= 0;i--)
            {
                array[i] = heap.RemoveByIndex(0);
            }

            return array;
        }
        
        /// <summary>
        /// 原数组大顶堆排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="compareFunc"></param>
        /// <returns></returns>
        public static T[] HeapSort(T[] array,Func<T,T,int> compareFunc)
        {
            int length = array.Length;
            BuildMaxHeap(array,length,compareFunc);
            for(int i = length - 1;i >= 0;i--)
            {
                Swap(array,0,i);
                length--;
                Heapify(array,0,length,compareFunc);
            }

            return array;
        }
        
        private static void BuildMaxHeap(T[] array,int length,Func<T,T,int> compareFunc)
        {
            //从无儿子节点的父节点从下往上递归排序大顶堆
            int NoChildNodeIndex = (int) Math.Floor(length * 0.5f);
            //大顶堆最大子元素置顶
            for(int i = NoChildNodeIndex;i >= 0;i--)
            {
                Heapify(array,i,length,compareFunc);
            }
        }
        
        private static void Heapify(T[] array,int i,int length,Func<T,T,int> compareFunc)
        {
            //左叶子
            int left = 2 * i + 1;
            //右叶子
            int right = 2 * i + 2;
            //父节点
            int largest = i;
            //构建大顶堆
            //左叶子大，父节点标记为左叶子
            if(left < length && compareFunc(array[left],array[largest]) > 0)
            {
                largest = left;
            }
            //右叶子大，父节点标记为右叶子
            if(right < length && compareFunc(array[right],array[largest]) > 0)
            {
                largest = right;
            }
            //说明有子节点比父节点大，需要替换
            if(largest != i)
            {
                //替换节点和父节点进行替换
                Swap(array,i,largest);
                //替换后的父节点继续向下递归
                Heapify(array,largest,length,compareFunc);
            }
        }
        #endregion
        
        #region 快速排序
        public static T[] QuickSort(T[] array,Func<T,T,int> compareFunc,int left = -1,int right = -1)
        {
            if(left == -1 && right == -1)
            {
                left = 0;
                right = array.Length - 1;
            }
            if(left < right)
            {
                int partitionIndex = Partition(array, left, right, compareFunc);
                QuickSort(array,compareFunc, left, partitionIndex - 1);
                QuickSort(array,compareFunc, partitionIndex + 1,right);
            }

            return array;
        }
        
        private static int Partition(T[] array,int left,int right,Func<T,T,int> compareFunc)
        {
            int pivot = left;
            //用于指向pivot数据应该插入的位置
            int index = pivot + 1;
            for(int i = index;i <= right;i++)
            {
                //中间对比节点，比对比几点数据小的放左边，大的放右边
                if(compareFunc(array[i],array[pivot]) < 0)
                {
                    Swap(array,i,index);
                    index++;
                }
            }
            
            Swap(array,pivot,index - 1);
            return index - 1;
        }
        #endregion
        
        #region 计数排序（只能针对数值进行排序）
        /// <summary>
        /// 计数排序（只能针对数值进行排序）
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int[] CountingSort(int[] array)
        {
            int maxValue = CommonUtility_Num.GetMaxNum(array);
            int bucketLength = maxValue + 1;
            //计数列表
            int[] bucket = new int[bucketLength];
            foreach(int value in array)
            {
                bucket[value]++;
            }

            int sortIndex = 0;
            for(int i = 0;i < bucketLength;i++)
            {
                while(bucket[i] > 0)
                {
                    //计数对象重新排入
                    array[sortIndex++] = i;
                    bucket[i]--;
                }
            }

            return array;
        }
        
        private static int GetMaxValue(int[] array)
        {
            int maxValue = array[0];
            foreach(int data in array)
            {
                if(data > maxValue)
                {
                    maxValue = data;
                }
            }
            return maxValue;
        }
        #endregion
        
        #region 基数排序（只能针对数值进行排序）
        public static int[] RadixSort(int[] array)
        {
            int maxDigit = GetMaxDigit(array);
            return radixSort(array, maxDigit);

        }
        
        private static int GetMaxDigit(int[] array)
        {
            int maxNum = CommonUtility_Num.GetMaxNum(array);
            return CommonUtility_Num.GetMaxDigit(maxNum);
        }
        
        private static int[] radixSort(int[] array,int maxDigit)
        {
            int mod = 10,dev = 1;
            const int bucketSize = 10;
            //根据最大基数进行循环
            for(int i = 0;i < maxDigit;i++,dev *=10,mod *= 10)
            {
                //初始化桶
                int[][] counter = new int[bucketSize][];
                //根据当前位数进行桶存放
                for(int j = 0;j < array.Length;j++)
                {
                    int bucketIndex = array[j] % mod / dev;
                    counter[bucketIndex] = Commonutility_Array.ArrayAppend(counter[bucketIndex], array[j]);
                }

                int pos = 0;
                //分好的数据重新排到序列中
                foreach(int[] bucket in counter)
                {
                    if(bucket == null)
                    {
                        continue;
                    }
                    foreach(int value in bucket)
                    {
                        array[pos++] = value;
                    }
                }
            }

            return array;
        }
        #endregion
        
        #region 桶排序（只能针对数值进行排序）
        public static int[] BucketSort(int[] array)
        {
            if(array.Length <= 0)
            {
                return array;
            }
            
            int bucketSize = (int)(array.Length * 0.5f),minValue = array[0],maxValue = array[0];
            foreach(int value in array)
            {
                if(value < minValue)
                {
                    minValue = value;
                }
                else if(value > maxValue)
                {
                    maxValue = value;
                }
            }
            //初始化桶
            int bucketCount = (int) Math.Floor((float) (maxValue - minValue) / bucketSize) + 1;
            int[][] buckets = new int[bucketCount][];
            
            //数据分桶处理(由于粪桶是根据数据从小到大分桶，泛型没有大小之分)
            for(int i = 0;i < array.Length;i++)
            {
                int index = (int) Math.Floor((float) (array[i] - minValue) / bucketSize);
                buckets[index] = Commonutility_Array.ArrayAppend(buckets[index], array[i]);
            }
            
            int arrayIndex = 0;
            //桶内堆排，并讲数据规划到元数据
            foreach(int[] bucket in buckets)
            {
                if(bucket == null || bucket.Length <= 0)
                {
                    continue;
                }
                Sort<int>.InsertSort(array,CompareFunc);
                foreach(int value in bucket)
                {
                    array[arrayIndex++] = value;
                }
            }
            return array;
        }
        
        private static int CompareFunc(int i,int j)
        {
            return i - j;
        }
        #endregion
        
        #region 通用
        private static void Swap(T[] array,int i,int j)
        {
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        #endregion
    }
}