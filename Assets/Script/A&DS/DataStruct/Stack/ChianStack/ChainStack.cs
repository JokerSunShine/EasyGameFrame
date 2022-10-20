using System.Runtime.CompilerServices;
using Common.DataStruct.Queue.ChainQueue;
using Common.OneWayChainList;

namespace Common.DataStruct.Stack.ChianStack
{
    public class ChainStack<T>
    {
        #region 数据
        public Node<T> Top { get; set; }
        public int Count { get; set; }
        #endregion
        
        #region 构造
        public ChainStack()
        {
            Top = null;
            Count = 0;
        }
        
        public ChainStack(T[] array)
        {
            if(array == null || array.Length <= 0)
            {
                return;
            }
            foreach(T item in array)
            {
                Push(item);
            }
        }
        
        public ChainStack(OneWayChainList<T> list)
        {
            TransformOnWayChainList(list);
        }
        #endregion
        
        #region 查询
        public bool IsEmpty()
        {
            return Top == null || Count == 0;
        }
        #endregion
        
        #region 功能
        public void Push(T item)
        {
            Node<T> node = new Node<T>(item);
            if(Top == null)
            {
                Top = node;
            }
            else
            {
                node.Next = Top;
                Top = node;
            }

            Count++;
        }
        
        public T Pop()
        {
            if(IsEmpty())
            {
                return default(T);
            }

            Node<T> node = Top;
            Top = Top.Next;
            Count--;
            return node.Data;
        }
        
        public void Clear()
        {
            Top = null;
        }
        #endregion
        
        #region 转化
        public ChainStack<T> TransformStack(ChainQueue<T> queue)
        {
            while(queue.IsEmpty() == false)
            {
                Push(queue.Dequeue());
            }

            return this;
        }
        
        public ChainStack<T> TransformOnWayChainList(OneWayChainList<T> oneWayChainList)
        {
            foreach(T data in oneWayChainList)
            {
                Push(data);
            }

            return this;
        }
        #endregion
    }
}