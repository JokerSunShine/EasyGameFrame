using System;
using DataStruct.Tree.BTree.BTree;

namespace DataStruct.Tree.BTree.BPlusTree
{
    public class BPlusTreeNode<T>:BtreeNode<T>
    {
        #region 构造
        public BPlusTreeNode(int order,bool isLeaf,Func<T,T,int> compareFunc,BTree<T> tree,int count = 0):base(order,isLeaf,compareFunc,tree,count)
        {
            
        }
        #endregion
        
        
    }
}