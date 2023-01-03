using DynamicTerrain;
using MapTools.Base;
using Packages.FW_Common.Other;
using UnityEditor;
using UnityEngine;

namespace MapTools.TerrainTool
{
    [PageAttribute(1,"地形工具")]
    public class TerrainTool : MapToolBase
    {
        #region 数据
        private string[] sceneNames;
        private int chooseIndex;
        public int ChooseIndex
        {
            get
            {
                return chooseIndex;
            }
            set
            {
                bool valueIsChange = value != chooseIndex;
                chooseIndex = value;
                if(valueIsChange)
                {
                    MapToolUtility.OpenScene(sceneNames[value], SceneResType.ArtScene);
                    RefreshSceneResouerce();
                }
            }
        }

        private SerializedObject serializedObject;
        private SerializedProperty passMaps_serializedProperty;
        public Texture2D[] texture2Ds;
        private Texture2D hollowMap;
        private Texture2D heightMap;
        private Texture2D LightMap;
        private Material terrainMat;
        private HeightMapData heightMapData;
        #endregion
        
        #region 初始化
        public override void Init()
        {
            base.Init();
            sceneNames = MapToolUtility.GetSceneNames();
            chooseIndex = 0;
            RefreshSceneResouerce();
        }
        #endregion
        
        #region 刷新
        public override void OnRefresh()
        {
            
        }
        
        private void RefreshSceneResouerce()
        {
            RefreshPassMaps();
            RefreshMap();
        }
        
        private void RefreshPassMaps() 
        {
            string sceneName = sceneNames[ChooseIndex];
            string assetsPath = string.Format("{0}/Art/SceneRes/{1}/Textures/PassMaps",Application.dataPath, sceneName);
            string[] assetsDataPath = CommonUtility_Directory.GetAllFilePath(assetsPath, "*.png");
            texture2Ds = null;
            if(assetsDataPath == null || assetsDataPath.Length <= 0)
            {
                serializedObject = null;
                passMaps_serializedProperty = null;
                return;
            }
            texture2Ds = new Texture2D[assetsDataPath.Length];
            for(int i = 0;i < assetsDataPath.Length;i++)
            {
                string path = assetsDataPath[i].Replace(CommonLoadPath.LoadOriginRootPath + "/", "");
                texture2Ds[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            }
            serializedObject = new SerializedObject(this);
            passMaps_serializedProperty = serializedObject.FindProperty("texture2Ds");
        }
        
        private void RefreshMap()
        {
            string sceneName = sceneNames[ChooseIndex];
            hollowMap = AssetDatabase.LoadAssetAtPath<Texture2D>(string.Format("{0}/{1}/Textures/HollowMap.bmp",MapToolUtility.GetSceneRootPath(SceneResType.ArtSceneRes),sceneName));
            heightMap = AssetDatabase.LoadAssetAtPath<Texture2D>(string.Format("{0}/{1}/Textures/TerrainHeight.bmp",MapToolUtility.GetSceneRootPath(SceneResType.ArtSceneRes),sceneName));
            LightMap = AssetDatabase.LoadAssetAtPath<Texture2D>(string.Format("{0}/{1}/Textures/TerrainLight.jpg",MapToolUtility.GetSceneRootPath(SceneResType.ArtSceneRes),sceneName));
            terrainMat = AssetDatabase.LoadAssetAtPath<Material>(string.Format("{0}/{1}/Materials/Terrain_Dynamic.mat",MapToolUtility.GetSceneRootPath(SceneResType.ArtSceneRes),sceneName));
            heightMapData = MapToolUtility.GetHeightMapData(sceneName);
        }
        #endregion
        
        #region GUI
        public override void OnToolGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                ChooseIndex = EditorGUILayout.Popup("Scene", ChooseIndex, sceneNames, GUILayout.Width(250));
                GUILayout.EndHorizontal();
                if(passMaps_serializedProperty != null)
                    EditorGUILayout.PropertyField(passMaps_serializedProperty);
                hollowMap = EditorGUILayout.ObjectField("HollowMap", hollowMap, typeof(Texture2D), false,GUILayout.Width(250)) as Texture2D;
                heightMap = EditorGUILayout.ObjectField("HeightMap", heightMap, typeof(Texture2D), false,GUILayout.Width(250)) as Texture2D;
                LightMap = EditorGUILayout.ObjectField("LightMap", LightMap, typeof(Texture2D), false,GUILayout.Width(250)) as Texture2D;
                heightMapData.heightOffset =
                    EditorGUILayout.FloatField("Height Offset", heightMapData.heightOffset, GUILayout.Width(250));
                heightMapData.heightScale =
                    EditorGUILayout.FloatField("Height Offset", heightMapData.heightScale, GUILayout.Width(250));
                GUILayout.BeginHorizontal();
                terrainMat = EditorGUILayout.ObjectField("TerrainMat", terrainMat, typeof(Material), false,GUILayout.Width(250)) as Material;
                if(GUILayout.Button("创建地形材质",GUILayout.Width(100)))
                {
                      
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        #endregion
        
        #region Destroy
        public override void OnClose()
        {
        }
        #endregion
    }
}