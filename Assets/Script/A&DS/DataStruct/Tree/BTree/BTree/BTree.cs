using System;
using Common.OneWayChainList;
using DataStruct.Tree.BTree.Base;

namespace DataStruct.Tree.BTree.BTree
{
    public class BTree<T>:BTreeBase<T>
    {
        #region 构造
        public BTree(int order,Func<T,T,int> compareFunc):base(order,compareFunc)
        {
            Root = new BtreeNode<T>(order,true,compareFunc,this);
        }
        
        public BTree(int order,Func<T,T,int> compareFunc,T[] dataList):this(order,compareFunc)
        {
            foreach(T data in dataList)
            {
                Insert(data);
            }
        }
        #endregion
        
        #region 查找
        public override BTreeNodeBase<T> FindValue(T data)
        {
            return Root.FindNode(data);
        }
        
        public override bool HaveValue(T data)
        {
            BTreeNodeBase<T> node = FindValue(data);
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
        public override void Traversal(ref OneWayChainList<T> traversaList,Action<T> action = null)
        {
            Root.Traversal(ref traversaList,action);
        }
        #endregion
        
        #region 插入
        public override void Insert(T data)
        {
            RootMaxSplit();
            Root.InserData(data);
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
        public override void Delete(T data)
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