using System;

namespace DataStruct.Tree.BinaryTree.FullBinaryTree
{
    public class FullBinaryTree_Array<T>:ArrayBinaryTreeAbstract<T>
    {
        #region 数据
        private T[] nodeArray;
        public override T[] NodeArray
        {
            get => nodeArray;
        }
        #endregion
        
        #region 构造
        public FullBinaryTree_Array(T[] array)
        {
            if(CheckIsFullBinary(array.Length))
            {
                this.nodeArray = array;
            }
        }
        
        /// <summary>
        /// 检测是否满足满二叉树
        /// </summary>
        /// <returns></returns>
        public bool CheckIsFullBinary(int length)
        {
            double hight = Math.Log(length + 1, 2);
            double smallValue = hight - (int)hight;
            return smallValue == 0;
        }
        #endregion
    }
}