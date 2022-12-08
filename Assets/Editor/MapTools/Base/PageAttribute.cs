using System;

namespace MapTools.Base
{
    public class PageAttribute : Attribute
    {
        public int layer;
        public string menuName;
        public PageAttribute(int layer,string menuName = default(string))
        {
            this.layer = layer;
            this.menuName = menuName;
        }
    }
}