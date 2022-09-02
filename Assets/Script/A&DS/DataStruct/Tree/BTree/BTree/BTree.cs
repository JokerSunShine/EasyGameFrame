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
        private BtreeNode<T> Root { get; set; }
        /// <summary>
        /// 阶
        /// </summary>
        public int order { get; }
        /// <summary>
        /// 对比方法
        /// </summary>
        private Func<T, T, int> compareFunc;
        #endregion
        
        #region 构造
        public BTree(int order,Func<T,T,int> compareFunc)
        {
            if(BTreeCheck(order,compareFunc) == false)
            {
                throw new Exception("只支持三阶以上B树！");
                return;
            }
            this.order = order;
            this.compareFunc = compareFunc;
            Root = new BtreeNode<T>(order,true,compareFunc);
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
            if(Root.IsMaxCount())
            {
                BtreeNode<T> newRoot = new BtreeNode<T>(order,false,compareFunc);
                newRoot.childs[0] = Root;
                Root = newRoot;
                Root.SplitChild(0);
            }
            Root.BTreeInserNodeNotFull(data);
        }
        #endregion
    }
}