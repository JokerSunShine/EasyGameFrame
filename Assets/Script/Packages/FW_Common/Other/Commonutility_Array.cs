namespace Packages.FW_Common.Other
{
    public static partial class Commonutility_Array
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