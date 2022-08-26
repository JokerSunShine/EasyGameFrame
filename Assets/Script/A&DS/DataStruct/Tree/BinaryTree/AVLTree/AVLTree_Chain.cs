using System;
using DataStruct.Tree.BinaryTree.BinarySearchTree;

namespace DataStruct.Tree.BinaryTree.AVLTree
{
    public class AVLTree_Chain<T>:BinarySearchTree_Chain<T>
    {
        #region 构造
        public AVLTree_Chain(T[] array,Func<T,T,int> func):base(array,func)
        {
            
        }
        #endregion
        
        #region 调整失衡
        
        #endregion
    }
}