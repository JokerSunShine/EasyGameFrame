using System.Collections.Generic;

namespace Script.Algorithm.BagAlgorithm.BagPolicy
{
    public interface IBagPolicy
    {
        int[,] CreateBagValueTable(BagItem[] bagItem, int bagCapacity);
    }
}