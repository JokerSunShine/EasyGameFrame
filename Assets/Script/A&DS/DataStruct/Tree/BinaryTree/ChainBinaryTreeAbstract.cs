using System;

namespace DataStruct.Tree.BinaryTree
{
    public abstract class ChainBinaryTreeAbstract<T>
    {
        #region 数据
        public abstract Node<T> Head { get; }
        public virtual Node<T> LeftLeaf { get => null; }
        public abstract int Count { get; }
        public virtual Func<T,T,int> CompareFunc { get; }
        #endregion
    }
}