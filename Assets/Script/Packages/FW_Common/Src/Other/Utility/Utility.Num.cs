namespace Framework
{
    public static partial class Utility
    {
        public static class Num
        {
            /// <summary>
            /// 获取最大数
            /// </summary>
            /// <param name="array"></param>
            /// <returns></returns>
            public static int GetMaxNum(int[] array)
            {
                int maxValue = array[0];
                foreach(int data in array)
                {
                    if(data > maxValue)
                    {
                        maxValue = data;
                    }
                }
                return maxValue;
            }
        
            /// <summary>
            /// 获取位数
            /// </summary>
            /// <param name="num"></param>
            /// <returns></returns>
            public static int GetMaxDigit(int num)
            {
                if(num == 0)
                {
                    return 1;
                }

                int length = 0;
                for(int curNum = num;curNum != 0;curNum /= 10)
                {
                    length++;
                }

                return length;
            } 
        }
    }
}