namespace Script.DataStruct.Tree.Heap.MinHeap
{
    public class MinHeap<T>
    {
        #region 数据
        /// <summary>
        /// 堆数据
        /// </summary>
        private T[] heap;
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
        #endregion
        
        #region 构造
        public MinHeap(int length)
        {
            heap = new T[length];
        }
        #endregion
        
        #region 功能
        public void Push(T data)
        {
            heap[count++] = data;
            
        }
        #endregion
    }
}