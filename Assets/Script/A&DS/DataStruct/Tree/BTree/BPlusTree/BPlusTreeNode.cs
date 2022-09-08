using System;
using Common.OneWayChainList;
using DataStruct.Tree.BTree.BTree;

namespace DataStruct.Tree.BTree.BPlusTree
{
    public class BPlusTreeNode<T>
    {
        // #region 数据
        // //下一个节点
        // public BPlusTreeNode<T> NextNode;
        // public BPlusTreeNode<T>[] childs;
        // #endregion
        //
        // #region 构造
        // public BPlusTreeNode(int order,bool isLeaf,Func<T,T,int> compareFunc,BTree<T> tree,int count = 0):base(order,isLeaf,compareFunc,tree,count)
        // {
        //     
        // }
        // #endregion
        //
        // #region 查找
        // public BPlusTreeNode<T> FindNode(T data)
        // {
        //     if(compareFunc == null)
        //     {
        //         return null;
        //     }
        //     
        //     int i = 0;
        //     while(i < count && compareFunc(data,Values[i]) >= 0)
        //     {
        //         i++;
        //     }
        //     if(isLeaf)
        //     {
        //         if(Values[i].Equals(data))
        //             return this;
        //         return null;
        //     }
        //
        //     return childs[i].FindNode(data);
        // }
        // #endregion
        //
        // #region 遍历
        // public override void Traversal(ref OneWayChainList<T> traversaList, Action<T> action = null)
        // {
        //     
        // }
        //
        // private BPlusTreeNode<T> FindLeftNode()
        // {
        //     if(isLeaf)
        //     {
        //         return this;
        //     }
        //     if(childs.Length <= 0)
        //     {
        //         return null;
        //     }
        //
        //     return childs[0].FindNode();
        // }
        // #endregion
        //
        // #region 插入
        // public override void InserData(T data)
        // {
        // }
        // #endregion
        //
        // #region 删除
        //
        // public override void DeleteData(T data)
        // {
        //     
        // }
        // #endregion
    }
}