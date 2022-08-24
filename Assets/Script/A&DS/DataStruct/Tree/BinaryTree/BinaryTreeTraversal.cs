using System;
using Common.DataStruct.Queue.ChainQueue;

namespace DataStruct.Tree.BinaryTree
{
    //二叉树遍历
    public class BinaryTreeTraversal<T>
    {
        private static int index = 0;
        
        #region 链式遍历
        /// <summary>
        /// 链式前序
        /// </summary>
        /// <param name="treeRoot">根节点</param>
        /// <param name="traversalList">顺序后的数据</param>
        /// <param name="action">回调</param>
        /// <param name="isFirst">第一次遍历</param>
        public static void PreorderTraversal_ChainTree(Node<T> treeRoot,ref Node<T>[] traversalList,Action<Node<T>> action = null,bool isFirst = true)
        {
            if(treeRoot == null)
            {
                return;
            }
            if(isFirst)
            {
                index = 0;
            }
            if(traversalList != null)
            {
                traversalList[index++] = treeRoot;
            }
            if(action != null)
            {
                action(treeRoot);
            }

            PreorderTraversal_ChainTree(treeRoot.LeftNode,ref traversalList,action,false);
            PreorderTraversal_ChainTree(treeRoot.RightNode,ref traversalList,action,false);
        }

        /// <summary>
        /// 链式中序
        /// </summary>
        /// <param name="treeRoot"></param>
        /// <param name="traversalList"></param>
        /// <param name="action"></param>
        /// <param name="isFirst"></param>
        public static void MiddleTraversal_ChainTree(Node<T> treeRoot,ref Node<T>[] traversalList,Action<Node<T>> action = null,bool isFirst = true)
        {
            if(treeRoot == null)
            {
                return;
            }
            if(isFirst)
            {
                index = 0;
            }

            MiddleTraversal_ChainTree(treeRoot.LeftNode,ref traversalList,action,false);
            
            if(traversalList != null)
            {
                traversalList[index++] = treeRoot;
            }
            if(action != null)
            {
                action(treeRoot);
            }
            
            MiddleTraversal_ChainTree(treeRoot.RightNode,ref traversalList,action,false);
        }
        
        /// <summary>
        /// 链式后序
        /// </summary>
        /// <param name="treeRoot"></param>
        /// <param name="traversalList"></param>
        /// <param name="action"></param>
        /// <param name="isFirst"></param>
        public static void BehindTraversal_ChainTree(Node<T> treeRoot,ref Node<T>[] traversalList,Action<Node<T>> action = null,bool isFirst = true)
        {
            if(treeRoot == null)
            {
                return;
            }
            if(isFirst)
            {
                index = 0;
            }

            BehindTraversal_ChainTree(treeRoot.LeftNode,ref traversalList,action,false);
            BehindTraversal_ChainTree(treeRoot.RightNode,ref traversalList,action,false);
            
            if(traversalList != null)
            {
                traversalList[index++] = treeRoot;
            }
            if(action != null)
            {
                action(treeRoot);
            }
        }
        
        /// <summary>
        /// 层次遍历
        /// </summary>
        public static void LayerTraversal_ChainTree(Node<T> treeRoot,ref Node<T>[] traversalList,Action<Node<T>> action = null,bool isFirst = true)
        {
            if(treeRoot == null)
            {
                return;
            }
            if(isFirst)
            {
                index = 0;
            }

            ChainQueue<Node<T>> chainQueue = new ChainQueue<Node<T>>();
            chainQueue.Enqueue(treeRoot);
            while(chainQueue.Count > 0)
            {
                Node<T> node = chainQueue.Dequeue();
                if(traversalList != null)
                {
                    traversalList[index++] = node;
                }
                if(action != null)
                {
                    action(treeRoot);
                }
                if(node.LeftNode != null)
                {
                    chainQueue.Enqueue(node.LeftNode);
                }
                if(node.RightNode != null)
                {
                    chainQueue.Enqueue(node.RightNode);
                }
            }
        }
        #endregion
        
        #region 数组遍历
        /// <summary>
        /// 数组前序
        /// </summary>
        /// <param name="arrayBinaryTree">二叉树</param>
        /// <param name="traversalList">顺序后的数据</param>
        /// <param name="action">回调</param>
        /// <param name="curIndex">下标</param>
        public static void PreorderTraversal_ArrayTree(ArrayBinaryTreeAbstract<T> arrayBinaryTree,ref T[] traversalList,Action<T> action = null,bool isFirst = true,int curIndex = 0)
        {
            if(isFirst)
            {
                index = 0;
            }
            if(arrayBinaryTree == null || arrayBinaryTree[curIndex].Equals(default(T)))
            {
                return;
            }
            
            T data = arrayBinaryTree[curIndex++];

            if(traversalList != null)
            {
                traversalList[index++] = data;
            }
            if(action != null)
            {
                action(data);
            }

            PreorderTraversal_ArrayTree(arrayBinaryTree,ref traversalList,action,false,2 * curIndex - 1);
            PreorderTraversal_ArrayTree(arrayBinaryTree,ref traversalList,action,false,2 * curIndex);
        }
        
        /// <summary>
        /// 数组中序
        /// </summary>
        /// <param name="arrayBinaryTree">二叉树</param>
        /// <param name="traversalList">顺序后的数据</param>
        /// <param name="action">回调</param>
        /// <param name="curIndex">下标</param>
        public static void MiddleTraversal_ArrayTree(ArrayBinaryTreeAbstract<T> arrayBinaryTree,ref T[] traversalList,Action<T> action = null,bool isFirst = true,int curIndex = 0)
        {
            if(isFirst)
            {
                index = 0;
            }
            if(arrayBinaryTree == null || arrayBinaryTree[curIndex].Equals(default(T)))
            {
                return;
            }
            
            T data = arrayBinaryTree[curIndex++];
            
            MiddleTraversal_ArrayTree(arrayBinaryTree,ref traversalList,action,false,2 * curIndex - 1);
           
            if(traversalList != null)
            {
                traversalList[index++] = data;
            }
            if(action != null)
            {
                action(data);
            }
            
            MiddleTraversal_ArrayTree(arrayBinaryTree,ref traversalList,action,false,2 * curIndex);
        }
        
        /// <summary>
        /// 数组后序
        /// </summary>
        /// <param name="arrayBinaryTree">二叉树</param>
        /// <param name="traversalList">顺序后的数据</param>
        /// <param name="action">回调</param>
        /// <param name="curIndex">下标</param>
        public static void BehindTraversal_ArrayTree(ArrayBinaryTreeAbstract<T> arrayBinaryTree,ref T[] traversalList,Action<T> action = null,bool isFirst = true,int curIndex = 0)
        {
            if(isFirst)
            {
                index = 0;
            }
            if(arrayBinaryTree == null || arrayBinaryTree[curIndex].Equals(default(T)))
            {
                return;
            }
            
            T data = arrayBinaryTree[curIndex++];
            
            BehindTraversal_ArrayTree(arrayBinaryTree,ref traversalList,action,false,2 * curIndex - 1);
            BehindTraversal_ArrayTree(arrayBinaryTree,ref traversalList,action,false,2 * curIndex);
            
            if(traversalList != null)
            {
                traversalList[index++] = data;
            }
            if(action != null)
            {
                action(data);
            }
        }
        
        /// <summary>
        /// 层次遍历
        /// </summary>
        public static void LayerTraversal_ArrayTree(ArrayBinaryTreeAbstract<T> arrayBinaryTree,ref T[] traversalList,Action<T> action = null,bool isFirst = true)
        {
            if(isFirst)
            {
                index = 0;
            }
            
            if(arrayBinaryTree == null || arrayBinaryTree.NodeArray == null)
            {
                return;
            }

            foreach(var node in arrayBinaryTree.NodeArray)
            {
                if(traversalList != null)
                {
                    traversalList[index++] = node;
                }
                if(action != null)
                {
                    action(node);
                }
            }
        }
        #endregion
    }
}