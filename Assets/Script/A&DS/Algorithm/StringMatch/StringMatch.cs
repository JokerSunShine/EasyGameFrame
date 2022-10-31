using System;
using _3DMath;

namespace Algorithm.StringMatch
{
    public class StringMatch
    {
        #region 暴力匹配
        /// <summary>
        /// 暴力匹配
        /// </summary>
        /// <param name="s">被比较字符串</param>
        /// <param name="patten">搜索字符串</param>
        /// <returns>匹配的开始下标</returns>
        public static int BruteForce(string s,string patten)
        {
            int i = 0, j = 0, index;
            while(i < s.Length && j < patten.Length)
            {
                if(s[i] == patten[j])
                {
                    i++;
                    j++;
                }
                else
                {
                    i = i - (j - 1);
                    j = 0;
                }
            }

            if (j >= patten.Length)
                index = i - patten.Length;
            else
                index = -1;
            return index;
        }
        #endregion
        
        #region BM算法
        public static int BMMatch(string s,string patten)
        {
            //源字符串长度，匹配字符串长度，匹配起点，匹配剩余数量
            int originStrLength = s.Length, pattenLength = patten.Length,index = 0,matchNum = 0;
            int[] badCharShifts = BuildBadCharactorHeuristic(patten);
            int[] goodSuffixShifts = BuildGoodSuffixHeuristic(patten);
            
            while(index <= (originStrLength - pattenLength))
            {
                matchNum = pattenLength - 1;
                
                while(matchNum >= 0 && patten[matchNum] == s[index + matchNum])
                {
                    matchNum--;
                }
                
                if(matchNum < 0)
                {
                    return index;
                }
                else
                {
                    index += Math.Max(goodSuffixShifts[matchNum],
                        badCharShifts[s[index + matchNum]] - (pattenLength - 1) + matchNum);
                }
            }

            return -1;
        }
        
        /// <summary>
        /// 启发式创建坏字符
        /// </summary>
        /// <returns></returns>
        private static int[] BuildBadCharactorHeuristic(string patten)
        {
            int m = patten.Length;
            int[] badCharactorShifts = new int[MathUtility.AlphabetSize];
            
            for(int i = 0;i < MathUtility.AlphabetSize;i++)
            {
                badCharactorShifts[i] = m;
            }
            
            for(int i = 0;i < m;i++)
            {
                badCharactorShifts[patten[i]] = m - i - 1;
            }

            return badCharactorShifts;
        }
        
        /// <summary>
        /// 启发式创建好后缀
        /// </summary>
        /// <param name="patten"></param>
        /// <returns></returns>
        private static int[] BuildGoodSuffixHeuristic(string patten)
        {
            int pattenLegnth = patten.Length;
            int[] goodSuffixShifts = new int[pattenLegnth];
            int[] suffixLengthArray = GetSuffixLengthArray(patten);
            
            for(int i = 0;i < pattenLegnth;i++)
            {
                goodSuffixShifts[i] = pattenLegnth;
            }

            int j = 0;
            for(int i = pattenLegnth - 1;i >= -1;--i)
            {
                if(i == -1 || suffixLengthArray[i] == i + 1)
                {
                    for(;j < pattenLegnth - 1 - i;++j)
                    {
                        if(goodSuffixShifts[j] == pattenLegnth)
                        {
                            goodSuffixShifts[j] = pattenLegnth - 1 - i;
                        }
                    }
                }
            }
            
            for(int i = 0;i < pattenLegnth - 1;++i)
            {
                goodSuffixShifts[pattenLegnth - 1 - suffixLengthArray[i]] = pattenLegnth - 1 - i;
            }

            return goodSuffixShifts;
        }
        
        private static int[] GetSuffixLengthArray(string patten)
        {
            int pattenLength = patten.Length;
            int[] suffixLengthArray = new int[pattenLength];

            int f = 0, q = 0, i = 0;

            suffixLengthArray[pattenLength - 1] = pattenLength;

            q = pattenLength - 1;
            
            for(i = pattenLength - 2;i > 0;--i)
            {
                if(i > q && suffixLengthArray[i + pattenLength - 1 - f] < i - q)
                {
                    suffixLengthArray[i] = suffixLengthArray[i + pattenLength - 1 - f];
                }
                else
                {
                    if(i < q)
                    {
                        q = i;
                    }

                    f = i;
                    
                    while(q >= 0 && patten[q] == patten[q + pattenLength - 1 - f])
                    {
                        q--;
                    }

                    suffixLengthArray[i] = f - q;
                }
            }

            return suffixLengthArray;
        }
        #endregion
        
        #region KMP算法
        public static int KMPMatch(string s,string patten)
        {
            int i = 0, j = 0, index;
            int[] nextArray = GetNextArray(patten);
            
            while(i < s.Length && j < patten.Length)
            {
                if(j == -1 || s[i] == patten[j])
                {
                    i++;
                    j++;
                }
                else
                {
                    j = nextArray[j];
                }
            }

            if (j >= patten.Length)
            {
                index = i - patten.Length;
            }
            else
            {
                index = -1;
            }

            return index;
        }
        
        /// <summary>
        /// 获取匹配失败位移表
        /// </summary>
        /// <param name="patten"></param>
        /// <returns></returns>
        private static int[] GetNextArray(string patten)
        {
            int j = 0, k = -1;
            int[] nextTbl = new int[patten.Length];

            nextTbl[0] = -1;
            
            while(j < patten.Length - 1)
            {
                if (k == -1 || patten[k] == patten[j])
                {
                    j++;
                    k++;
                    if (patten[k] != patten[j])
                    {
                        nextTbl[j] = k;
                    }
                    else
                    {
                        nextTbl[j] = nextTbl[k];
                    }
                }
                else
                {
                    k = nextTbl[k];
                }
            }

            return nextTbl;
        }
        #endregion
    }
}