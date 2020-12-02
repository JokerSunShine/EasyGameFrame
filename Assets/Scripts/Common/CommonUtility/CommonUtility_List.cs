using System.Collections.Generic;

public static partial class CommonUtility {
    public static void RemoveAll(this List<string> list,string str) {
        if (list != null && list.Count > 0)
        {
            for (int i = list.Count - 1;i < 0;i--)
            {
                if (list[i] == str)
                {
                    list.RemoveAt(i);
                }
            }
        }
    }

    public static void Add_NoSame(this List<string> list, string str)
    {
        if (list.Contains(str))
        {
            return;
        }
        list.Add(str);
    }
}