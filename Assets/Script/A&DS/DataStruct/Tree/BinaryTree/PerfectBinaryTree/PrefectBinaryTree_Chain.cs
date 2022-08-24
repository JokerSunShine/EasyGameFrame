namespace DataStruct.Tree.BinaryTree.PerfectBinaryTree
{
    public class PrefectChainBinaryTreeChain<T>:ChainBinaryTreeAbstract<T>
    {
        #region 数据
        private Node<T> head;
        public override Node<T> Head
        {
            get => head;
        }

        private Node<T> leftLeaf;
        public override Node<T> LeftLeaf
        {
            get => leftLeaf;
        }

        private int count;
        public override int Count
        {
            get => count;
        }
        #endregion
        
        #region 构造
        public PrefectChainBinaryTreeChain(){}
        
        public PrefectChainBinaryTreeChain(T data)
        {
            head = new Node<T>(data);
        }
        
        public PrefectChainBinaryTreeChain(T[] dataList)
        {
            CreatePrefectTree(dataList);
        }
        
        public Node<T> CreatePrefectTree(T[] dataList,int index = 1)
        {
            if(index > dataList.Length)
            {
                return null;
            }
            count++;
            Node<T> node = new Node<T>(dataList[index - 1]);
            if(head == null)
            {
                head = node;
            }
            node.LeftNode = CreatePrefectTree(dataList, index * 2);
            if(node.LeftNode != null && leftLeaf == null)
            {
                leftLeaf = node.LeftNode;
            }
            node.RightNode = CreatePrefectTree(dataList, index * 2 + 1);
            return node;
        }
        #endregion
    }
}