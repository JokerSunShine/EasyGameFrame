using System;

namespace DataStruct.Tree.BinaryTree.BinarySearchTree
{
    public class BinarySearchTree_Chain<T>:ChainBinaryTreeAbstract<T>
    {
        #region 数据
        private Node<T> head;
        public override Node<T> Head { get => head;}

        private int count;
        public override int Count
        {
            get => count;
        }
        
        Func<T,T,int> compareFunc;
        #endregion
        
        #region 构造
        /// <summary>
        /// 构造二叉排序树
        /// </summary>
        /// <param name="array">元数据</param>
        /// <param name="func">对比方法（返回 -1：小于，0：等于，1：大于）</param>
        public BinarySearchTree_Chain(T[] array,Func<T,T,int> func)
        {
            if(array == null || func == null)
            {
                return;
            }

            this.compareFunc = func;
            foreach(T data in array)
            {
                InsertNode(data);
            }
        }
        #endregion
        
        #region 功能
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="node"></param>
        public void InsertNode(T value,Node<T> node = null)
        {
            if(head == null)
            {
                head = new Node<T>(value);
                return;
            }
            if(node == null && head != null)
            {
                node = head;
            }
            
            int compareValue = compareFunc(value, node.Data);
            Node<T> nextValue = null;
            switch(compareValue)
            {
                case -1:
                    nextValue = node.LeftNode;
                    if(nextValue == null)
                    {
                        node.LeftNode = new Node<T>(value);
                        node.LeftNode.Parent = node;
                        return;
                    }
                    break;
                case 0:
                    return;
                case 1:
                    nextValue = node.RightNode;
                    if(nextValue == null)
                    {
                        node.RightNode = new Node<T>(value);
                        node.RightNode.Parent = node;
                        return;
                    }
                    break;
            }

            InsertNode(value, nextValue);
        }
        
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data"></param>
        public void DeleteNode(T data)
        {
            
        }
        
        /// <summary>
        /// 查找节点
        /// </summary>
        /// <returns></returns>
        public Node<T> FindNode(T data,Node<T> node = null,int lastCompareState = 0)
        {
            if(head == null)
            {
                return null;
            }
            if(node == null)
            {
                node = head;
            }
            int compareValue = compareFunc(data, node.Data);
            Node<T> curNode = null;
            switch(compareValue)
            {
                case -1:
                    curNode = node.LeftNode != null ? FindNode(data, node.LeftNode,compareValue) : lastCompareState == -1 ? node : node.Parent;
                    break;
                case 0:
                    curNode = node;
                    break;
                case 1:
                    curNode = node.RightNode != null ? FindNode(data, node.RightNode,compareValue) : lastCompareState == 1 ? node : node.Parent;
                    break;
            }

            return curNode;
        }
        #endregion
    }
}