using System.Collections.Generic;

namespace Packages.FW_Common.Other
{
    public static class Temp<T>
    {
        private static List<T> m_List = new List<T>();
        public static List<T> List
        {
            get
            {
                m_List.Clear();
                return m_List;
            }
        }
    }
}