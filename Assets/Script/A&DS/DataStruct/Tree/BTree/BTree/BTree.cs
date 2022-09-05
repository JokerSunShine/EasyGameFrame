using System;
using Common.OneWayChainList;

namespace DataStruct.Tree.BTree.BTree
{
    public class BTree<T>
    {
        #region 数据
        /// <summary>
        /// 根节点
        /// </summary>
        public BtreeNode<T> Root { get; set; }
        /// <summary>
        /// 阶
        /// </summary>
        public int order { get; }
        /// <summary>
        /// 对比方法
        /// </summary>
        private Func<T, T, int> compareFunc;

        private int middleIndex;
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
        public BTree(int order,Func<T,T,int> compareFunc)
        {
            if(BTreeCheck(order,compareFunc) == false)
            {
                throw new Exception("只支持三阶以上B树！");
            }
            this.order = order;
            this.compareFunc = compareFunc;
            Root = new BtreeNode<T>(order,true,compareFunc,this);
        }
        
        public BTree(int order,Func<T,T,int> compareFunc,T[] dataList):this(order,compareFunc)
        {
            foreach(T data in dataList)
            {
                Insert(data);
            }
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
        
        #region 查找
        public BtreeNode<T> FindValue(T data)
        {
            return Root.FindNode(data);
        }
        
        public bool HaveValue(T data)
        {
            BtreeNode<T> node = FindValue(data);
            foreach(var curData in node.Values)
            {
                if(curData.Equals(data))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
        
        #region 遍历
        public void Traversal(ref OneWayChainList<T> traversaList,Action<T> action = null)
        {
            Root.Traversal(ref traversaList,action);
        }
        #endregion
        
        #region 插入
        public void Insert(T data)
        {
            RootMaxSplit();
            Root.BTreeInserNodeNotFull(data);
            RootMaxSplit();
        }
        
        private void RootMaxSplit()
        {
            if(Root.IsMaxCount())
            {
                BtreeNode<T> newRoot = new BtreeNode<T>(order,false,compareFunc,this);
                newRoot.childs[0] = Root;
                Root = newRoot;
                Root.SplitChild(0);
            }  
        }
        #endregion
        
        #region 删除
        public void Delete(T data)
        {
            if(Root == null)
            {
                return;
            }
            Root.DeleteData(data);
            RootMaxSplit();
        }
        #endregion
    }
}