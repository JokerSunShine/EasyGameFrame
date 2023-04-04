using System;

namespace ResourceTool.Base
{
    public class ResourcePageAttribute : Attribute
    {
        public int order;
        public string menuName;
        
        public ResourcePageAttribute(int order,string menuName = default(string))
        {
            this.order = order;
            this.menuName = menuName;
        }
    }
}