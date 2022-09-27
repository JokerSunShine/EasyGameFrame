using System;
using DataStruct.Tree.BinaryTree;

namespace DataStruct.Tree.Heap.MaxHeap
{
    public class MaxHeap<T>:ArrayBinaryTreeAbstract<T>
    {
         #region 数据
        private T[] heap;
        public override T[] NodeArray
        {
            get { return heap; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        private int count = 0;
        public int Count
        {
            get
            {
                return count;
            }
        }
        
        Func<T,T,int> compareFunc;
        #endregion
        
        #region 构造
        public MaxHeap(Func<T,T,int> compareFunc,T[] array)
        {
            heap = new T[4];
            this.compareFunc = compareFunc;
            foreach(var data in array)
            {
                Push(data);
            }
        }
        #endregion
        
        #region 功能
        public void Push(T data)
        {
            if(count + 1 >= NodeArray.Length)
            {
                ExtenSize();
            }
            NodeArray[count++] = data;
            int nowIndex = count, parentIndex;
            
            while(nowIndex > 1)
            {
                parentIndex = nowIndex / 2 - 1;
                if(compareFunc(NodeArray[nowIndex - 1],NodeArray[parentIndex]) <= 0)
                {
                    break;
                }

                T swapData = NodeArray[nowIndex - 1];
                NodeArray[nowIndex - 1] = NodeArray[parentIndex];
                NodeArray[parentIndex] = swapData;

                nowIndex = parentIndex + 1;
            }
        }
        
        public T RemoveByIndex(int index)
        {
            if(index < 0 || index >= count)
            {
                return default(T);
            }
            
            if(index == count - 1)
            {
                return NodeArray[--count];
            }
            
            T removeData = NodeArray[index];
            int nowIndex = index + 1, nextChildIndex = 0,arrayNowIndex = nowIndex - 1,arrayNextChildIndex = 0;
            NodeArray[index] = NodeArray[--count];
            
            while(nowIndex * 2 <= count)
            {
                nextChildIndex = nowIndex * 2;
                arrayNextChildIndex = nextChildIndex - 1;

                if (nextChildIndex <= count && compareFunc(NodeArray[arrayNextChildIndex + 1], NodeArray[arrayNextChildIndex]) > 0)
                {
                    nextChildIndex++;
                    arrayNextChildIndex++;
                }
                
                if(compareFunc(NodeArray[arrayNowIndex],NodeArray[arrayNextChildIndex]) > 0)
                {
                    return removeData;
                }
                
                T swapData = NodeArray[arrayNowIndex];
                NodeArray[arrayNowIndex] = NodeArray[arrayNextChildIndex];
                NodeArray[arrayNextChildIndex] = swapData;

                nowIndex = nextChildIndex;
                arrayNowIndex = nowIndex - 1;
            }
            return removeData;
        }
        
        public bool Remove(T data)
        {
            int index = FindIndex(data);
            if(index >= 0)
            {
                RemoveByIndex(index);
            }
            return index >= 0;
        }
        
        public bool Find(T data)
        {
            int index = FindIndex(data);
            return index > 0;
        }
        
        public int FindIndex(T data)
        {
            if(NodeArray != null && count > 0)
            {
                for(int i = 0;i < count;i++)
                {
                    if(data.Equals(NodeArray[i]))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
        
        public void ExtenSize()
        {
            T[] newArray = new T[NodeArray.Length * 2];
            for(int i = 0;i < count;i++)
            {
                newArray[i] = NodeArray[i];
            }

            heap = newArray;
        }
        #endregion
    }
}