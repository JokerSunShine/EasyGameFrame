using System.Collections.Generic;

namespace Common.TrieSearch
{
    public class TrieSearch
    {
        /// <summary>
        /// 单词树节点
        /// </summary>
        private TrieNode TrieRoot = new TrieNode(char.MinValue);
        
        /// <summary>
        /// 增加单词
        /// </summary>
        /// <param name="s"></param>
        /// <param name="trieNode"></param>
        /// <param name="pos"></param>
        public void Insert(string s,TrieNode trieNode = null,int pos = 0)
        {
            if(string.IsNullOrEmpty(s) || pos >= s.Length)
            {
                return;
            }
            
            //没有传入节点，则将起始节点作为根节点
            if(trieNode == null)
            {
                trieNode = TrieRoot;
            }

            //尝试获取子节点
            char c = s[pos];
            TrieNode node = trieNode.GetChildNode(c);
            
            //如果没有子节点则添加一个子节点
            if(node == null)
            {
                node = trieNode.AddChild(c);
            }
            
            //插入的节点是最后的节点
            if(pos == s.Length - 1)
            {
                node.AddCount();
            }
            else
            {
                Insert(s,node,pos + 1);
            }
        }

        /// <summary>
        /// 删除单词
        /// </summary>
        /// <param name="s"></param>
        /// <param name="trieNode"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool Delete(string s,TrieNode trieNode = null,int pos = 0)
        {
            if(string.IsNullOrEmpty(s))
            {
                return false;
            }
            
            //没有传入节点，则将起始节点作为根节点
            if(trieNode == null)
            {
                trieNode = TrieRoot;
            }
            //尝试获取子节点
            char c = s[pos];
            TrieNode node = trieNode.GetChildNode(c);
            if(node == null)
            {
                return false;
            }

            //如果字符串长度查询到最后，则删除当前节点
            bool ret;
            if(pos == s.Length - 1)
            {
                int before = node.Occurances;
                ret = before > 0;
                node.ResetCount();
            }
            else
            {
                ret = Delete(s, node, pos + 1);
            }
            
            //如果当前节点没有子节点，则删除节点
            if(node.HaveChildNode() && node.Occurances == 0)
            {
                trieNode.RemoveNode(c);
            }

            return ret;
        }
        
        /// <summary>
        /// 获取搜索单词最后的节点
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private TrieNode GetSearchNode(string s,TrieNode node = null,int pos = 0)
        {
            if(string.IsNullOrEmpty(s))
            {
                return null;
            }

            if(node == null)
            {
                node = TrieRoot;
            }

            char c = s[pos];
            
            TrieNode childNode = node.GetChildNode(c);
            if(childNode == null)
            {
                return null;
            }
            
            if(pos >= s.Length - 1)
            {
                return childNode;
            }
            else
            {
                return GetSearchNode(s, childNode, pos + 1);
            }
            return null;
        }
        
        /// <summary>
        /// 搜索单词
        /// </summary>
        /// <returns></returns>
        public bool SearchWord(string s)
        {
            TrieNode curNode = GetSearchNode(s);
            
            return curNode != null && curNode.Occurances > 0;
        }

        private List<string> wordList = new List<string>();
        
        /// <summary>
        /// 获取前缀单词列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetPrefixWord(string s,TrieNode node = null)
        {
            //从前缀后开始搜索
            if(node == null)
            {
                wordList.Clear();
                TrieNode prefixTrieNode = GetSearchNode(s);
                if(prefixTrieNode == null)
                {
                    return null;
                }
                
                GetPrefixWord(s, prefixTrieNode);
                return wordList;
            }
            
            if(node.Occurances > 0)
            {
                wordList.Add(node.GetString());
            }

            TrieNode[] nodes = node.GetTrieNodeArray();
            if(nodes == null)
            {
                return null;
            }
            for(int i = 0;i < nodes.Length;i++)
            {
                GetPrefixWord(s,nodes[i]);
            }
            return wordList;

        }
    }
}