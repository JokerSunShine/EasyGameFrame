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
            foreach(var data in array)
            {
                Insert(data);
            }
        }
        #endregion
        
        #region 功能
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns></returns>
        public T FindMin()
        {
            Node<T> minNode = FindMinNode(root,compareFunc);
            if(minNode == null)
                return default(T);
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

            Node<T> parent = root,child = null;
            while(parent != null)
            {
                if(compareFunc(parent.data,data) == 0)
                {
                    return parent;
                }
                else
                {
                    child = Search(parent.child, data);
                    if(child != null)
                    {
                        return child;
                    }

                    parent = parent.next;
                }
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
            Node<T> node = Search(root, data);
            return node != null;
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
        
        /// <summary>
        /// 减少关键字的值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        public void DecreaseValue(Node<T> node,T data)
        {
            if(compareFunc(node.data,data) <= 0 || Contains(data))
            {
                return;
            }

            node.data = data;
            Node<T> curNode = node;
            while(curNode.parent != null && compareFunc(curNode.data,curNode.parent.data) < 0)
            {
                T curData = curNode.parent.data;
                curNode.parent.data = curNode.data;
                curNode.data = curData;

                curNode = curNode.parent;
            }
        }
        
        /// <summary>
        /// 增加关键字的值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        public void IncreaseValue(Node<T> node,T data)
        {
            if(compareFunc(node.data,data) >= 0 || Contains(data))
            {
                return;
            }

            node.data = data;
            Node<T> curNode = node,minNode = FindMinNode(node,compareFunc);
            while(minNode != null && compareFunc(minNode.data,curNode.data) < 0)
            {
                T curData = minNode.data;
                minNode.data = curNode.data;
                curNode.data = curData;

                curNode = minNode;
                minNode = FindMinNode(curNode, compareFunc);
            }
        }
        
        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="newData"></param>
        public void UpdateData(T oldData,T newData)
        {
            Node<T> node = Search(root, oldData);
            if (node != null)
                UpdateDataByNode(node, newData);
        }
        
        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="node"></param>
        /// <param name="newData"></param>
        public void UpdateDataByNode(Node<T> node,T newData)
        {
            if (node == null)
                return;

            int compareResult = compareFunc(node.data, newData);
            if(compareResult < 0)
                IncreaseValue(node,newData);
            else if(compareResult > 0)
                DecreaseValue(node,newData);
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
            childTree.next = rootTree.child;
            rootTree.child = childTree;
            rootTree.degree++;
        }
        
        /// <summary>
        /// 合并两个二项堆（两个二项堆合并到同一个二项堆中）
        /// </summary>
        /// <param name="tree1"></param>
        /// <param name="tree2"></param>
        /// <returns></returns>
        public static Node<T> Merge(Node<T> tree1,Node<T> tree2)
        {
            if (tree1 == null) return tree2;
            if (tree2 == null) return tree1;

            Node<T> tree1Node = tree1, tree2Node = tree2, addNode = null, root = null, nowNode = null;
            
            while(tree1Node != null || tree2Node != null)
            {
                if(tree1Node != null && tree2Node != null)
                {
                    if(tree1Node.degree > tree2Node.degree)
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
                    nowNode.next = addNode;
                }
                nowNode = addNode;
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
            Node<T> root = Merge(tree1, tree2);
            
            if(root == null)
            {
                return null;
            }
            
            Node<T> curNode = root,preNode = null,nextNode = null;
            while(curNode.next != null)
            {
                if((curNode.degree != curNode.next.degree) || (curNode.next.next != null && curNode.next.degree == curNode.next.next.degree))
                {
                    preNode = curNode;
                    curNode = curNode.next;
                }
                else if(compareFunc(curNode.data,curNode.next.data) <= 0)
                {
                    nextNode = curNode.next;
                    curNode.next = curNode.next.next;
                    Link(nextNode,curNode);
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

                    nextNode = curNode.next;
                    Link(curNode,curNode.next);
                    curNode = nextNode;
                }
            }

            return root;
        }
        
        /// <summary>
        /// 反转节点与兄弟节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Node<T> Reverse(Node<T> node)
        {
            if(node == null)
            {
                return null;
            }
            
            Node<T> NewNode = null,NowNode = node,ParentNode = null;
            while(NowNode != null)
            {
                ParentNode = NowNode.next;
                NowNode.next = NewNode;
                NewNode = NowNode;
                NowNode = ParentNode;
            }
            
            return NewNode;
        }
        
        /// <summary>
        /// 获取最小节点
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="compareFunc"></param>
        /// <returns></returns>
        public static Node<T> FindMinNode(Node<T> parentNode,Func<T,T,int> compareFunc)
        {
            if(parentNode == null)
                return null;
            
            Node<T> minNode = parentNode.child;
            while(minNode != null && minNode.next != null && compareFunc(minNode.data,minNode.next.data) > 0)
            {
                minNode = minNode.next;
            }

            return minNode;
        }
        #endregion
    }
}