using System;
using DataStruct.Tree.BinaryTree.AVLTree;

namespace DataStruct.Tree.BinaryTree.RedBlackTree
{
    public class RedBalckTree_Chain<T>:AVLTree_Chain<T>
    {
        #region 构造
        public RedBalckTree_Chain(T[] array,Func<T,T,int> func):base(array,func)
        {
            
        }
        #endregion
        
        #region 添加
        public override Node<T> InsertNode(T value)
        {
            Node<T> insertNode = TryInsertNode(value);
            RedBlackInsertAdjust(insertNode);
            return insertNode;
        }
        
        /// <summary>
        /// 红黑树插入矫正
        /// </summary>
        public void RedBlackInsertAdjust(Node<T> node)
        {
            //新插入的节点为红色
            node.color = false;
            
            //有父节点，父节点的颜色也是红色
            while(node.Parent != null && node.Parent.color == false)
            {
                Node<T> parentNode = node.Parent;
                Node<T> grandParentNode = parentNode.Parent;
                //没有祖父节点，则不需要处理
                if(grandParentNode == null)
                {
                    break;
                }
                //父节点是祖父节点的左节点
                if(node.Parent.PointType == Node<T>.ParentPointType.Left)
                {
                    Node<T> uncleNode = grandParentNode.RightNode;
                    //叔节点只是作为旋转条件，如果没有叔节点，就拟定一个黑色树节点用于右旋达到平衡
                    if(uncleNode == null)
                    {
                        uncleNode = new Node<T>(true);
                    }
                    //叔节点是红色
                    if(uncleNode.color == false)
                    {
                        //父节点和叔节点全部改成黑色
                        parentNode.color = uncleNode.color = true;
                        //祖父节点改成红色,继续祖父进行调整
                        grandParentNode.color = false;
                        node = grandParentNode;
                    }
                    else
                    {
                        //叔节点是黑色
                        //节点是父节点的右节点，则对父节点左旋
                        if(node.PointType == Node<T>.ParentPointType.Right)
                        {
                            LL_Rotate(parentNode);
                            node = parentNode;
                        }
                        //节点是父节点的左节点，则将父节点变成黑色，祖父节点变成红色，祖父节点右旋
                        node.Parent.color = true;
                        grandParentNode.color = false;
                        RR_Rotate(grandParentNode);
                    }
                }
                else
                {
                    Node<T> uncleNode = grandParentNode.LeftNode;
                    if(uncleNode == null)
                    {
                        uncleNode = new Node<T>(true);
                    }
                    //叔节点是红色
                    if(uncleNode.color == false)
                    {
                        //父节点和叔节点全部改成黑色
                        parentNode.color = uncleNode.color = true;
                        //祖父节点改成红色，继续祖父进行调整
                        grandParentNode.color = false;
                        node = grandParentNode;
                    }
                    else
                    {
                        //叔节点是黑色
                        //节点是父节点的左节点，则对父节点右旋
                        if(node.PointType == Node<T>.ParentPointType.Left)
                        {
                            RR_Rotate(parentNode);
                            node = parentNode;
                        }
                        //节点是父节点的右节点，则将父节点变成黑色，祖父节点变成红色，祖父节点左旋
                        node.Parent.color = true;
                        grandParentNode.color = false;
                        LL_Rotate(grandParentNode);
                    }
                }
            }

            Head.color = true;
        }
        #endregion
 
        #region 删除
        public override Node<T> DeleteNode(T data, bool changeCount = true)
        {
            Node<T> descendantNode;
            Node<T> deleteNode = TryDeleteNode(data,out descendantNode);
            if(descendantNode == null)
            {
                descendantNode = new Node<T>(true,deleteNode.Parent,deleteNode.PointType);
            }
            RedBlackDeleteAdjust(descendantNode);
            return descendantNode;
        }
        
        /// <summary>
        /// 红黑树删除矫正
        /// </summary>
        /// <param name="node"></param>
        public void RedBlackDeleteAdjust(Node<T> node)
        {
            //x不是根节点且颜色为黑色（红色删除不影响红黑树规律）
            while(node.Parent != null && node.color)
            {
                Node<T> parentNode = node.Parent;
                Node<T> brotherNode, leftNephew, rightNephew;
                //是父节点的左节点
                if(node.PointType == Node<T>.ParentPointType.Left)
                {
                    brotherNode = parentNode.RightNode;
                    //如果没有兄弟，则左边减少反而更加平衡，所以不用处理
                    if(brotherNode == null)
                    {
                        break;
                    }
                    //兄弟节点是红色
                    if(brotherNode.color == false)
                    {
                        //兄弟节点变成黑色，父节点变成红色，父节点左旋，然后更新兄弟节点
                        brotherNode.color = true;
                        parentNode.color = false;
                        LL_Rotate(parentNode);
                        brotherNode = node.Parent.RightNode;
                    }

                    leftNephew = brotherNode.LeftNode;
                    rightNephew = brotherNode.RightNode;
                    //兄弟节点为黑色，左右侄子为黑色（没有也是黑色）,不管都要降低右边的高度
                    if((leftNephew == null || leftNephew.color) && (rightNephew == null || rightNephew.color))
                    {
                        brotherNode.color = false;
                        node = parentNode;
                    }
                    else
                    {
                        //左侄子为红色且右侄子为黑色,则将左侄子变成黑色，兄弟变成红色，然后右旋兄弟，更改引用
                        if((leftNephew != null && leftNephew.color == false) && (rightNephew == null || rightNephew.color))
                        {
                            leftNephew.color = true;
                            brotherNode.color = false;
                            RR_Rotate(brotherNode);
                            brotherNode = parentNode.RightNode;
                        }
                        //右侄子为红色，则将右侄子变成黑色，兄弟变成父节点颜色，父节点变成黑色，然后父节点左旋
                        if(rightNephew != null && rightNephew.color == false)
                        {
                            brotherNode.color = parentNode.color;
                            rightNephew.color = parentNode.color = true;
                            LL_Rotate(parentNode);
                            node = Head;
                        }
                    }
                }
                else
                {
                    brotherNode = parentNode.LeftNode;
                    //如果没有兄弟，则右边减少反而更加平衡，所以不用处理
                    if(brotherNode == null)
                    {
                        break;
                    }
                    //兄弟节点是红色
                    if(brotherNode.color == false)
                    {
                        //兄弟节点变成黑色，父节点变成红色，父节点右旋，然后更新兄弟节点
                        brotherNode.color = true;
                        parentNode.color = false;
                        RR_Rotate(parentNode);
                        brotherNode = node.Parent.LeftNode;
                    }

                    leftNephew = brotherNode.LeftNode;
                    rightNephew = brotherNode.RightNode;
                    //兄弟节点为黑色，左右侄子为黑色（没有也是黑色）,不管都要降低左边的高度
                    if((leftNephew == null || leftNephew.color) && (rightNephew == null || rightNephew.color))
                    {
                        brotherNode.color = false;
                        node = parentNode;
                    }
                    else
                    {
                        //右侄子为红色且左侄子为黑色,则将右侄子变成黑色，兄弟变成红色，然后左旋兄弟，更改引用
                        if(rightNephew != null && rightNephew.color == false && (leftNephew == null || leftNephew.color)) 
                        {
                            rightNephew.color = true;
                            brotherNode.color = false;
                            LL_Rotate(brotherNode);
                            brotherNode = parentNode.LeftNode;
                        }
                        //左侄子为红色，则将左侄子变成黑色，兄弟变成父节点颜色，父节点变成黑色，然后父节点右旋
                        if(leftNephew != null && leftNephew.color == false)
                        {
                            brotherNode.color = parentNode.color;
                            leftNephew.color = parentNode.color = true;
                            RR_Rotate(parentNode);
                            node = Head;
                        }
                    }
                }
            }

            node.color = true;
        }
        #endregion
    }
}