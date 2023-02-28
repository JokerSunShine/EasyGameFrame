using System.Collections.Generic;
using Script.Algorithm.BagAlgorithm.BagPolicy;
using XLua;

namespace Script.Algorithm.BagAlgorithm
{
    public struct BagItem
    {
        public int itemId;
        public int weight;
        public int value;
        
        public BagItem(int itemId,int weight,int value)
        {
            this.itemId = itemId;
            this.weight = weight;
            this.value = value;
        }
    }
    
    public class BagAlgorithm
    {
        #region 数据
        private IBagPolicy bagPolicy = new CompleteItemBagPolicy();
        public IBagPolicy BagPolicy
        {
            set
            {
                if(value != null)
                {
                    bagPolicy = value;
                }
            }
        }

        private BagItem[] bagItem;
        private int[,] bagValueTable = null;
        #endregion
        
        #region 构造
        public BagAlgorithm(){}
        
        public BagAlgorithm(BagItem[] bagItem, int bagCapacity)
        {
            if(bagPolicy == null)
            {
                return;
            }

            this.bagItem = bagItem; 
            bagValueTable = bagPolicy.CreateBagValueTable(bagItem, bagCapacity);
        }
        #endregion
        
        #region 获取
        public int GetMaxValue(int maxItemIndex,int bagCapacity,List<BagItem> insertBagItem)
        {
            if(insertBagItem == null)
            {
                insertBagItem = new List<BagItem>();
            }
            if(bagValueTable == null || bagValueTable.Length <= 0 || bagItem == null ||
               maxItemIndex < 1 || maxItemIndex >= bagValueTable.Length || 
               bagCapacity < 0 || bagCapacity >= bagValueTable.LongLength)
            {
                return 0;
            }

            int nextBagCapacity = bagCapacity;
            if(bagValueTable[maxItemIndex,bagCapacity] > bagValueTable[maxItemIndex - 1,bagCapacity])
            {
                BagItem item = bagItem[maxItemIndex - 1];
                insertBagItem.Add(item);
                nextBagCapacity = bagCapacity - item.weight;
            }
            
            GetMaxValue(maxItemIndex - 1,nextBagCapacity,insertBagItem);
            return bagValueTable[maxItemIndex, bagCapacity];
        }
        
        public int GetMaxValueByCompleteBag(int maxItemIndex,int bagCapacity,List<BagItem> insertBagItem)
        {
            if(insertBagItem == null)
            {
                insertBagItem = new List<BagItem>();
            }
            if(bagValueTable == null || bagValueTable.Length <= 0 || bagItem == null ||
               maxItemIndex < 1 || maxItemIndex >= bagValueTable.Length || 
               bagCapacity < 0 || bagCapacity >= bagValueTable.LongLength)
            {
                return 0;
            }

            int nextBagCapacity = bagCapacity;
            int nextItemIndex = maxItemIndex;
            if (bagValueTable[maxItemIndex, bagCapacity] > bagValueTable[maxItemIndex - 1, bagCapacity])
            {
                BagItem item = bagItem[maxItemIndex - 1];
                insertBagItem.Add(item);
                nextBagCapacity = bagCapacity - item.weight;
            }
            else
                nextItemIndex = nextItemIndex - 1;
            
            GetMaxValue(nextItemIndex,nextBagCapacity,insertBagItem);
            return bagValueTable[maxItemIndex, bagCapacity];
        }
        #endregion

    }
}