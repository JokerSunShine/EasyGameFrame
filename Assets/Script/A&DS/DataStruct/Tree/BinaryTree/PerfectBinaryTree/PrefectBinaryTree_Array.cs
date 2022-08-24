namespace DataStruct.Tree.BinaryTree.PerfectBinaryTree
{
    public class PrefectBinaryTree_Array<T>:ArrayBinaryTreeAbstract<T>
    {
        #region 数据
        private T[] nodeArray;
        public override T[] NodeArray
        {
            get => nodeArray;
        }
        #endregion
        
        #region 构造
        public PrefectBinaryTree_Array()
        {
            
        }
        
        public PrefectBinaryTree_Array(T[] array)
        {
            this.nodeArray = array;
        }
        #endregion
    }
}