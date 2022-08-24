namespace DataStruct.Tree.BinaryTree
{
    public abstract class ArrayBinaryTreeAbstract<T>
    {
        public abstract T[] NodeArray { get; }
        
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        public T this[int index]
        {
            get
            {
                if(NodeArray != null && index < NodeArray.Length)
                {
                    return NodeArray[index];
                }
                return default(T);
            }
        }
    }
}