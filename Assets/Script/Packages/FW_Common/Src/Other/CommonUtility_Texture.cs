using UnityEngine;

namespace Packages.FW_Common.Other
{
    public enum TextureFormatType
    {
        EXR,
        JPG,
        PNG,
        TGA,
    }
    
    public class CommonUtility_Texture
    {
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