using UnityEngine;

namespace Framework
{
    public static partial class Utility
    {
        public static class Texture
        {
            public enum TextureFormatType
            {
                EXR,
                JPG,
                PNG,
                TGA,
            }
            
            public static Texture2D SaveToPic(Texture2D texture2D,string path,TextureFormatType type = TextureFormatType.PNG)
            {
                byte[] bytes;
                if(type == TextureFormatType.EXR)
                {
                
                }
                else if(type == TextureFormatType.JPG)
                {
                
                }
                else if(type == TextureFormatType.PNG)
                {
                
                }
                else if(type == TextureFormatType.TGA)
                {
                
                }

                return texture2D;
            }
        }
    }
}