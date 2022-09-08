using System;
using Common.OneWayChainList;
using DataStruct.Tree.BTree.Base;

namespace DataStruct.Tree.BTree.BPlusTree
{
    public class BPlusTreeNode<T>:BTreeNodeBase<T>
    {
        #region 构造
        public BPlusTreeNode(int order,bool isLeaf,Func<T,T,int> compareFunc,BTreeBase<T> tree,int count = 0):base(order,isLeaf,compareFunc,tree,count)
        {
            
        }
        #endregion

        #region 查找
        public override BTreeNodeBase<T> FindNode(T data)
        {
            if(compareFunc == null)
            {
                return null;
            }

            int i = 0;
            while (i < count && compareFunc(data,Values[i]) > 0)
            {
                i++;
            }
            
            if(isLeaf)
            {
                if(data.Equals(Values[i]))
                {
                    return this;
                }

                return null;
            }

            return childs[i].FindNode(data);
        }
        #endregion
        
        #region 遍历
        public override void Traversal(ref OneWayChainList<T> traversaList, Action<T> action = null)
        {
            
        }
        #endregion

        #region 插入
        public override void InserData(T data)
        {
            throw new NotImplementedException();
        }
        #endregion
        
        #region 删除
        public override void DeleteData(T data)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 查询
        public override bool IsMinCount()
        {
            throw new NotImplementedException();
        }
        
        public override bool IsMaxCount()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 获取子节点
        public override BTreeNodeBase<T> GetNewTreeNoe(int order, bool isLeaf, Func<T, T, int> compareFunc, BTreeBase<T> tree, int count = 0)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}