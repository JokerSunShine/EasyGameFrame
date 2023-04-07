namespace Framework
{
    public static partial class Utility
    {
        public static class Array
        {
            public static int[] ArrayAppend(int[] array,int value)
            {
                int[] newArray = array == null ? new int[1] : new int[array.Length + 1];
                if(array != null)
                {
                    array.CopyTo(newArray,0);
                }

                newArray[newArray.Length - 1] = value;
                return newArray;
            }
        }
    }
}