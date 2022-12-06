using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicTerrain
{
    public class TerrainData : ScriptableObject
    {

        public Vector2Int mapSize = new Vector2Int(256, 256);
        public Vector2Int bloackSize = new Vector2Int(48, 48);
        public Vector2Int viewSize = new Vector2Int(32, 32);
        public Material mat;
        public Vector4[] mainTex_STs;
        public HeightMapData heightMapData;
        public Texture2D lightMap;
    }
}