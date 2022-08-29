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
        
        #region 功能
        public override Node<T> InsertNode(T value)
        {
            Node<T> node = base.InsertNode(value);
            TryBalanceChange(node);
            return node;
        }

        public override Node<T> DeleteNode(T data, bool changeCount = true)
        {
            Node<T> node = base.DeleteNode(data, changeCount);
            if(changeCount)
            {
                TryBalanceChange(node);    
            }
            return node;
        }
        #endregion
        
        #region 调整失衡
        /// <summary>
        /// 尝试矫正失衡（一般情况下传入新加入的节点）
        /// </summary>
        /// <param name="node"></param>
        public void TryBalanceChange(Node<T> node)
        {
            if(node == null || node.Parent == null)
            {
                return;
            }
            if(Math.Abs(node.Parent.BalanceFactor) < 2)
            {
                TryBalanceChange(node.Parent);
                return;
            }

            int leftDepth = node.LeftNode != null ? node.LeftNode.Depth : 0;
            int rightDepth = node.RightNode != null ? node.RightNode.Depth : 0;
            //子节点失衡方向
            int childUnbalanceDir = leftDepth - rightDepth;
            if(node.PointType == Node<T>.ParentPointType.Right)
            {
                if(childUnbalanceDir < 0)
                {
                    LL_Rotate(node.Parent);
                }
                else if(childUnbalanceDir > 0)
                {
                    RL_Rotate(node.Parent);
                }
            }
            else if(node.PointType == Node<T>.ParentPointType.Left)
            {
                if(childUnbalanceDir > 0)
                {
                    RR_Rotate(node.Parent);
                }
                else if(childUnbalanceDir < 0)
                {
                    LR_Rotate(node.Parent);
                }
            }
            TryBalanceChange(node.Parent);
        }
        
        /// <summary>
        /// 左旋
        /// </summary>
        /// <param name="node">节点</param>
        public void LL_Rotate(Node<T> node)
        {
            if(node == null || node.RightNode == null)
            {
                return;
            }
            Node<T> rightNode = node.RightNode;
            Node<T> rightLeftNode = node.RightNode.LeftNode;
            
            if(node.Parent == null)
            {
                head = rightNode;
            }
            else
            {
                node.Parent.LeftNode = rightNode;
            }
            node.RightNode = rightLeftNode;
            if(rightLeftNode != null)
            {
                rightLeftNode.Parent = node;
            }
            rightNode.LeftNode = node;
            rightNode.Parent = node.Parent;
            node.Parent = rightNode;
        }
        
        /// <summary>
        /// 右旋
        /// </summary>
        /// <param name="node">节点</param>
        public void RR_Rotate(Node<T> node)
        {
            if(node == null || node.LeftNode == null)
            {
                return;
            }

            Node<T> leftNode = node.LeftNode;
            Node<T> leftRightNode = node.LeftNode.RightNode;
            
            if(node.Parent == null)
            {
                head = leftNode;
            }
            else
            {
                node.Parent.RightNode = leftNode;
            }
            node.LeftNode = leftRightNode;
            if(leftRightNode != null)
            {
                leftRightNode.Parent = node;
            }
            leftNode.RightNode = node;
            leftNode.Parent = node.Parent;
            node.Parent = leftNode;
        }
        
        /// <summary>
        /// 左子树右节点变更
        /// </summary>
        /// <param name="node"></param>
        public void LR_Rotate(Node<T> node)
        {
            LL_Rotate(node.LeftNode);
            RR_Rotate(node);
        }
        
        /// <summary>
        /// 右子树左节点变更
        /// </summary>
        /// <param name="node"></param>
        public void RL_Rotate(Node<T> node)
        {
            RR_Rotate(node.RightNode);
            LL_Rotate(node);
        }
        #endregion
    }
}