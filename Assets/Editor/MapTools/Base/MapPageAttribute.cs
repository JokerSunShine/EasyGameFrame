using System;

namespace MapTools.Base
{
    public class MapPageAttribute : Attribute
    {
        public int layer;
        public string menuName;
        public MapPageAttribute(int layer,string menuName = default(string))
        {
            this.layer = layer;
            this.menuName = menuName;
        }
    }
}