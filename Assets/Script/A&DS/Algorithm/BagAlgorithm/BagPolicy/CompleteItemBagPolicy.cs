using System;

namespace Script.Algorithm.BagAlgorithm.BagPolicy
{
    public class CompleteItemBagPolicy : IBagPolicy
    {
        public int[,] CreateBagValueTable(BagItem[] bagItem, int bagCapacity)
        {
            int[,] bagValueTable = new int[bagItem.Length + 1,bagCapacity + 1];
            for(int itemIndex = 1;itemIndex <= bagItem.Length;itemIndex++)
            {
                BagItem item = bagItem[itemIndex - 1];
                for(int weight = 1;weight <= bagCapacity;weight++)
                {
                    for(int insertNum = 0;insertNum <= weight / item.weight;insertNum++)
                    {
                        int insertWeight = insertNum * item.weight;
                        if(weight >= insertWeight)
                            bagValueTable[itemIndex, weight] = Math.Max(bagValueTable[itemIndex - 1,weight - insertWeight] + insertNum * item.value,bagValueTable[itemIndex,weight]);
                    }
                }
            }

            return bagValueTable;
        }   
    }
}