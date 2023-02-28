using System;
using System.Collections.Generic;

namespace Script.Algorithm.BagAlgorithm.BagPolicy
{
    //单物品背包策略
    public class SingleItemBagPolicy : IBagPolicy
    {
        public int[,] CreateBagValueTable(BagItem[] bagItem, int bagCapacity)
        {
            int[,] bagValueTable = new int[bagItem.Length + 1,bagCapacity + 1];
            int bagValueTableIndex;
            BagItem curBagItem;
            for(int i = 0;i < bagItem.Length;i++)
            {
                bagValueTableIndex = i + 1;
                curBagItem = bagItem[i];
                for(int j = 1;j <= bagCapacity;j++)
                {
                    //如果当前物品放不下，那么当前背包最优价值 = 前一个道具的最优价值
                    if(j < curBagItem.weight)
                    {
                        bagValueTable[bagValueTableIndex, j] = bagValueTable[bagValueTableIndex - 1, j];
                    }
                    else
                    {
                        //如果当前物品放得下，则判断放进背包或者不放进背包后的价值哪个大，哪个大则就是最优价值
                        bagValueTable[bagValueTableIndex, j] = Math.Max(bagValueTable[bagValueTableIndex - 1, j],
                            bagValueTable[bagValueTableIndex - 1, j - curBagItem.weight] + curBagItem.value);
                    }
                }
            }
            return bagValueTable;
        }
    }
}