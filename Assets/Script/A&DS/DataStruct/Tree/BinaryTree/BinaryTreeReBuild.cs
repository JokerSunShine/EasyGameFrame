namespace DataStruct.Tree.BinaryTree
{
    public class BinaryTreeReBuild<T>
    {
        //二叉树重建
        //本质是通过遍历结果的规律得到实际的二叉树
        //只有前序 + 中序 / 中序 + 后续进行递归得到
        
        /// <summary>
        /// 前中序转二叉树
        /// </summary>
        /// <param name="preList"></param>
        /// <param name="MiddleList"></param>
        /// <param name="preStart"></param>
        /// <param name="preEnd"></param>
        /// <param name="middleStart"></param>
        /// <param name="middleEnd"></param>
        /// <returns></returns>
        public static Node<T> PreAndMiddleToBinaryTree(Node<T>[] preList,Node<T>[] MiddleList,int preStart,int preEnd,int middleStart,int middleEnd)
        {
            if(preStart >= preList.Length ||  preList[preStart] == null)
            {
                return null;
            }
            Node<T> node = new Node<T>(preList[preStart].Data);
            if(preStart == preEnd && middleStart == middleEnd)
            {
                return node;
            }

            int root = 0;
            foreach(Node<T> curNode in MiddleList)
            {
                if(node.Data.Equals(curNode.Data))
                {
                    break;
                }
                root++;
            }

            int leftLength = root - middleStart;
            int rightLength = middleEnd - root;
            
            if(leftLength > 0)
            {
                node.LeftNode = PreAndMiddleToBinaryTree(preList,MiddleList,preStart + 1,preStart + leftLength,middleStart,middleStart + leftLength);
            }
            if(rightLength > 0)
            {
                node.RightNode = PreAndMiddleToBinaryTree(preList, MiddleList, preStart + leftLength + 1, preEnd,
                    root + 1, middleEnd);
            }

            return node;
        }
        
        /// <summary>
        /// 中序后续遍历结果转二叉树
        /// </summary>
        /// <param name="middleList"></param>
        /// <param name="behindList"></param>
        /// <param name="middleStart"></param>
        /// <param name="middleEnd"></param>
        /// <param name="behindStart"></param>
        /// <param name="behindEnd"></param>
        /// <returns></returns>
        public static Node<T> MiddleAndBehindToBinaryTree(Node<T>[] middleList,Node<T>[] behindList,int middleStart,int middleEnd,int behindStart,int behindEnd)
        {
            if(behindEnd < 0 || behindList[behindEnd] == null)
            {
                return null;
            }
            Node<T> node = new Node<T>(behindList[behindEnd].Data);
            if(middleStart == middleEnd && behindStart == behindEnd)
            {
                return node;
            }

            int root = 0;
            foreach(Node<T> curNode in middleList)
            {
                if(node.Data.Equals(curNode.Data))
                {
                    break;
                }
                root++;
            }

            int leftLength = root - middleStart;
            int rightLength = middleEnd - root;
            
            if(leftLength > 0)
            {
                node.LeftNode = MiddleAndBehindToBinaryTree(middleList, behindList, middleStart,
                    middleStart + leftLength - 1, behindStart, behindStart + leftLength - 1);
            }
            if(rightLength > 0)
            {
                node.RightNode = MiddleAndBehindToBinaryTree(middleList,behindList,root + 1,middleEnd,behindStart + leftLength,behindEnd - 1);
            }

            return node;
        }
    }
}