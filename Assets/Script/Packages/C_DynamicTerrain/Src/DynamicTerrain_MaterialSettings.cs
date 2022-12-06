using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DynamicTerrain
{
    [ExecuteInEditMode]
    public class DynamicTerrain_MaterialSettings : MonoBehaviour
    {
        public Vector4[] maintex_STs;
        private Material mat;
        public Texture2DArray mainTex;

        public TerrainData terrainData;
        private void OnEnable()
        {
            mat = GetComponent<Renderer>().sharedMaterial;
            mainTex = mat.GetTexture("_MainTex") as Texture2DArray;
            int length = mainTex.depth;
            string sceneId = gameObject.name.Replace("Terrain_", "");
            string terrainDataPath = string.Format("Assets/Res/SceneRes/{0}/TerrainData.asset", sceneId);
            terrainData = AssetDatabase.LoadAssetAtPath<TerrainData>(terrainDataPath);
            if (terrainData.mainTex_STs == null)
            {
                terrainData.mainTex_STs = new Vector4[length];
                for (int i = 0; i < length; i++)
                {
                    terrainData.mainTex_STs[i] = new Vector4(128, 128, 0, 0);
                }
            }
            else if (terrainData.mainTex_STs.Length < length)
            {
                Vector4[] newSTs = new Vector4[mainTex.depth];
                for (int i = 0; i < terrainData.mainTex_STs.Length; i++)
                {
                    newSTs[i] = terrainData.mainTex_STs[i];
                }

                for (int i = terrainData.mainTex_STs.Length; i < mainTex.depth; i++)
                {
                    newSTs[i] = new Vector4(128, 128, 0, 0);
                }
                terrainData.mainTex_STs = newSTs;
            }
            maintex_STs = terrainData.mainTex_STs;

            Shader.SetGlobalTexture("Terrain_HeightMap", terrainData.heightMapData.heightMap);
            Shader.SetGlobalTexture("_Terrain_AmbientTex", terrainData.lightMap);
            Shader.SetGlobalFloat("_GlobalAmbientIntensity",1f);//全局的环境光贴图亮度系数，实现地图等变暗效果


            transform.position = new Vector3(0, terrainData.heightMapData.heightOffset, 0);
            transform.localScale = new Vector3(1, terrainData.heightMapData.heightScale, 1);
        }
        private void Update()
        {
            if (mat)
            {
                for (int i = 0; i < terrainData.mainTex_STs.Length; i++)
                {
                    terrainData.mainTex_STs[i] = maintex_STs[i];
                    EditorUtility.SetDirty(terrainData);
                }
                mat.SetVectorArray("_MainTex_STs", maintex_STs);
            }
        }
    }
}
#endif