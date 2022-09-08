using System;
using Common.OneWayChainList;

namespace DataStruct.Tree.BTree.Base
{
    public abstract class BTreeBase<T>
    {
        #region 数据
        /// <summary>
        /// 根节点
        /// </summary>
        public BTreeNodeBase<T> Root { get; set; }
        /// <summary>
        /// 阶
        /// </summary>
        public int order { get; }
        /// <summary>
        /// 对比方法
        /// </summary>
        protected Func<T, T, int> compareFunc;
        private int middleIndex;
        /// <summary>
        /// 中间节点
        /// </summary>
        private int MiddleIndex
        {
            get
            {
                if(order <= 0)
                {
                    middleIndex = (order + 1) / 2;
                }

                return middleIndex;
            }
        }
        #endregion
        
        #region 构造
        public BTreeBase(int order,Func<T,T,int> compareFunc)
        {
            if(BTreeCheck(order,compareFunc) == false)
            {
                throw new Exception("只支持三阶以上B树！");
            }
            this.order = order;
            this.compareFunc = compareFunc;
        }
        
        public bool BTreeCheck(int order,Func<T,T,int> compareFunc)
        {
            if(order < 3 || compareFunc == null)
            {
                return false;
            }

            return true;
        }
        #endregion

        public abstract BTreeNodeBase<T> FindValue(T data);
        public abstract bool HaveValue(T data);
        public abstract void Traversal(ref OneWayChainList<T> traversaList,Action<T> action = null);
        public abstract void Insert(T data);
        public abstract void Delete(T data);
    }
}