using System;

namespace DataStruct.Tree.Heap.BinomialNode
{
    public class BinomialHeap<T>
    {
        #region 数据
        private Node<T> root;
        private Func<T, T, int> compareFunc;
        #endregion
        
        #region 构造
        public BinomialHeap(Func<T,T,int> compareFunc,T[] array)
        {
            this.compareFunc = compareFunc;
        }
        #endregion
        
        #region 功能
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns></returns>
        public T FindMin()
        {
            if(root == null)
                return default(T);
            
            Node<T> minNode = root;
            while(minNode != null && minNode.next != null && compareFunc(minNode.data,minNode.next.data) > 0)
            {
                minNode = minNode.next;
            }

            return minNode.data;
        }
        
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="root"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private Node<T> Search(Node<T> root,T data)
        {
            if(root == null)
            {
                return null;
            }
            if(compareFunc(root.data,data) == 0)
            {
                return root;
            }
            while(root.next != null)
            {
                return Search(root.next, data);
            }
            while(root.child != null)
            {
                return Search(root.child, data);
            }

            return null;
        }
        
        /// <summary>
        /// 二项堆是否有数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Contains(T data)
        {
            return Search(root, data) != null;
        }
        
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data"></param>
        public void Insert(T data)
        {
            if(Contains(data))
            {
                return;
            }
            Node<T> node = new Node<T>(data);
            root = Union(node,root,compareFunc);
        }
        
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Node<T> Remove(T data)
        {
            if(root == null)
            {
                return null;
            }

            Node<T> deleteNode = Search(root,data);
            if(deleteNode == null)
            {
                return null;
            }
            
            //删除节点上移数据
            while(deleteNode.parent != null)
            {
                //交换数据
                T curData = deleteNode.data;
                deleteNode.data = deleteNode.parent.data;
                deleteNode.parent.data = curData;

                deleteNode = deleteNode.parent;
            }

            Node<T> prevNode = null;
            Node<T> nowNode = root;
            while(compareFunc(nowNode.data,data) != 0)
            {
                prevNode = nowNode;
                nowNode = nowNode.next;
            }
            
            if(prevNode == null)
            {
                root = root.next;
            }
            else
            {
                prevNode.next = deleteNode.next;
            }

            root = Union(root, Reverse(deleteNode.child), compareFunc);
            return root;
        }
        #endregion
        
        #region 静态功能
        /// <summary>
        /// 二项树连接
        /// </summary>
        /// <param name="childTree">子二项树</param>
        /// <param name="rootTree">根二项树</param>
        public static void Link(Node<T> childTree,Node<T> rootTree)
        {
            childTree.parent = rootTree;
            childTree.child = rootTree.child;
            rootTree.child = childTree;
            rootTree.degree++;
        }
        
        /// <summary>
        /// 合并两个二项堆（两个二项堆合并到同一个二项堆中）
        /// </summary>
        /// <param name="tree1"></param>
        /// <param name="tree2"></param>
        /// <returns></returns>
        public static Node<T> Merge(Node<T> tree1,Node<T> tree2,Func<T,T,int> compareFunc)
        {
            if (tree1 == null) return tree2;
            if (tree2 == null) return tree1;

            Node<T> tree1Node = tree1, tree2Node = tree2, addNode = null, root = null;
            
            while(tree1Node != null || tree2Node != null)
            {
                if(tree1Node != null && tree2Node != null)
                {
                    if(compareFunc(tree1Node.data,tree2Node.data) > 0)
                    {
                        addNode = tree2Node;
                        tree2Node = tree2Node.next;
                    }
                    else
                    {
                        addNode = tree1Node;
                        tree1Node = tree1Node.next;
                    }
                }
                else
                {
                    if(tree1Node != null)
                    {
                        addNode = tree1Node;
                        tree1Node = tree1Node.next;
                    }
                    else
                    {
                        addNode = tree2Node;
                        tree2Node = tree2Node.next;
                    }
                }

                
                if(root == null)
                {
                    root = addNode;
                }
                else
                {
                    root.next = addNode;
                }
            }

            return root;
        }
        
        /// <summary>
        /// 合并二项堆（两个二项堆相同的合并）
        /// </summary>
        /// <param name="tree1"></param>
        /// <param name="tree2"></param>
        /// <param name="compareFunc"></param>
        /// <returns></returns>
        public Node<T> Union(Node<T> tree1,Node<T> tree2,Func<T,T,int> compareFunc)
        {
            Node<T> root = Merge(tree1, tree2, compareFunc);
            
            if(root == null)
            {
                return null;
            }
            
            Node<T> curNode = root,preNode = null;
            while(curNode.next != null)
            {
                if((curNode.degree != curNode.next.degree) || (curNode.next.next != null && curNode.next.degree == curNode.next.next.degree))
                {
                    preNode = curNode;
                    curNode = curNode.next;
                }
                else if(compareFunc(curNode.data,curNode.next.data) <= 0)
                {
                    curNode.next = curNode.next.next;
                    Link(curNode.next,curNode);
                }
                else
                {
                    if(preNode == null)
                    {
                        root = curNode.next;
                    }
                    else
                    {
                        preNode.next = curNode.next;
                    }
                    Link(curNode,curNode.next);
                    curNode = curNode.next;
                }
            }

            return root;
        }
        
        public Node<T> Reverse(Node<T> node)
        {
            if(node == null)
            {
                return null;
            }
            
            Node<T> curNode = node,root = null;
            while(curNode.next != null)
            {
                root = node.next;
                curNode = curNode.next;
            }

            return root;
        }
        #endregion
    }
}