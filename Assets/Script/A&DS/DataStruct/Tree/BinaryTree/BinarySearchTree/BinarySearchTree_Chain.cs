using System;

namespace DataStruct.Tree.BinaryTree.BinarySearchTree
{
    public class BinarySearchTree_Chain<T>:ChainBinaryTreeAbstract<T>
    {
        #region 数据
        protected Node<T> head;
        public override Node<T> Head { get => head;}

        protected int count;
        public override int Count
        {
            get => count;
        }

        private Func<T, T, int> compareFunc;
        public override Func<T,T,int> CompareFunc { get => compareFunc; }
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
        public virtual Node<T> InsertNode(T value)
        {
            return TryInsertNode(value);
        }
        
        protected Node<T> TryInsertNode(T value)
        {
            if(head == null)
            {
                head = new Node<T>(value,this);
                count++;
                return head;
            }

            Node<T> parentNode = GetInsertNode(value);
            if(parentNode == null || parentNode.Data.Equals(value))
            {
                return null;
            }
            int compareValue = compareFunc(value, parentNode.Data);
            Node<T> inserNode = new Node<T>(value,this);
            Node<T> nextValue = null;
            switch(compareValue)
            {
                case -1:
                    nextValue = parentNode.LeftNode;
                    parentNode.LeftNode = inserNode;
                    inserNode.LeftNode = nextValue;
                    break;
                case 0:
                    return null;
                case 1:
                    nextValue = parentNode.RightNode;
                    parentNode.RightNode = inserNode;
                    inserNode.RightNode = nextValue;
                    break;
            }

            inserNode.Parent = parentNode;
            if(nextValue != null)
            {
                nextValue.Parent = inserNode;
            }

            count++;
            return inserNode;
        }
        
        /// <summary>
        /// 获取可以插入的节点
        /// </summary>
        /// <returns></returns>
        public Node<T> GetInsertNode(T data,Node<T> node = null)
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
                    curNode = node.LeftNode != null ? GetInsertNode(data, node.LeftNode) : node;
                    break;
                case 0:
                    curNode = node;
                    break;
                case 1:
                    curNode = node.RightNode != null ? GetInsertNode(data, node.RightNode) : node;
                    break;
            }

            return curNode;
        }
        
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data"></param>
        public virtual Node<T> DeleteNode(T data,bool changeCount = true)
        {
            Node<T> descendantNode;
            return TryDeleteNode(data,out descendantNode);
        }
        
        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="data"></param>
        /// <param name="changeCount">递归用数据</param>
        /// <param name="descendantNode">移除节点的继承节点</param>
        /// <returns>移除的节点</returns>
        protected Node<T> TryDeleteNode(T data,out Node<T> descendantNode,bool changeCount = true)
        {
            Node<T> node = FindNode(data);
            descendantNode = null;
            if(node == null)
            {
                return null;
            }

            int degree = node.Degree;
            int compareParentValue = -2;
            if(changeCount)
            {
                count--;
            }
            if(node.Parent != null)
            {
                compareParentValue = compareFunc(data, node.Parent.Data);
            }
            if(degree == 0)
            {
                //叶子节点直接删除
                if(compareParentValue == 1)
                {
                    node.Parent.RightNode = null;
                }
                else if(compareParentValue == -1)
                {
                    node.Parent.LeftNode = null;
                }
                else if(compareParentValue == -2)
                {
                    head = null;
                }
            }
            else if(degree == 1)
            {
                //有一个节点，节点的子节点给父节点
                Node<T> nextNode = node.LeftNode;
                if(nextNode == null)
                {
                    nextNode = node.RightNode;
                }
                if(compareParentValue == 1)
                {
                    node.Parent.RightNode = nextNode;
                }
                else if(compareParentValue == -1)
                {
                    node.Parent.LeftNode = nextNode;
                }
                else if(compareParentValue == -2)
                {
                    head = nextNode;
                }

                descendantNode = nextNode;
                nextNode.Parent = node.Parent;
            }
            else if(degree == 2)
            {
                //有两个节点，则寻找最佳后继节点代替被删除节点
                Node<T> curDescendantNode = GetDescendantNode(node);
                if(curDescendantNode == null)
                {
                    return null;
                }

                TryDeleteNode(curDescendantNode.Data,out descendantNode,false);
                descendantNode = curDescendantNode;
                //父节点替换
                Node<T> originParentNode = node.Parent;
                if(originParentNode != null)
                {
                    curDescendantNode.Parent = originParentNode;
                    if(originParentNode.LeftNode.Data.Equals(node.Data))
                    {
                        originParentNode.LeftNode = curDescendantNode;
                    }
                    else
                    {
                        originParentNode.RightNode = curDescendantNode;
                    }
                }
                else
                {
                    curDescendantNode.Parent = null;
                    head = curDescendantNode;
                }
                //左节点替换
                if(node.LeftNode != null)
                {
                    curDescendantNode.LeftNode = node.LeftNode;
                    node.LeftNode.Parent = curDescendantNode;
                }
                //右节点替换
                if(node.RightNode != null)
                {
                    curDescendantNode.RightNode = node.RightNode;
                    node.RightNode.Parent = curDescendantNode;
                }
            }

            return node;
        }
        
        /// <summary>
        /// 寻找节点
        /// </summary>
        /// <param name="data"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public Node<T> FindNode(T data,Node<T>node = null,bool isFirstFind = true)
        {
            if(head == null)
            {
                return null;
            }
            if(isFirstFind)
            {
                node = head;
            }
            if(node == null)
            {
                return null;
            }

            int compareValue = compareFunc(data, node.Data);
            Node<T> curNode = null;
            switch(compareValue)
            {
                case -1:
                    curNode = FindNode(data, node.LeftNode,false);
                    break;
                case 0:
                    curNode = node;
                    break;
                case 1:
                    curNode = FindNode(data, node.RightNode,false);
                    break;
            }

            return curNode;
        }
        
        /// <summary>
        /// 获取后继节点（该节点必然有右子树）
        /// </summary>
        /// <returns></returns>
        public Node<T> GetDescendantNode(Node<T> node)
        {
            if(node == null || node.RightNode == null)
            {
                return null;
            }

            Node<T> descendantNode = node.RightNode;
            while(descendantNode.LeftNode != null)
            {
                descendantNode = descendantNode.LeftNode;
            }

            return descendantNode;
        }
        #endregion
    }
}