using UnityEngine;

namespace DynamicTerrain
{
    public class HeightMapData : ScriptableObject
    {
        public Texture2D heightMap;
        public float heightScale = 4.1f;
        public float heightOffset;

        public float GetHeight(int coordX, int coordY)
        {
            return heightMap.GetPixel(coordX + 2, coordY).r * heightScale + heightOffset;
        }
    }
}