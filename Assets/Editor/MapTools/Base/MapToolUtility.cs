using System.Collections.Generic;
using System.IO;
using DynamicTerrain;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapTools.Base
{
    public enum SceneResType
    {
        ArtScene,
        ClientScene,
        ArtSceneRes,
    }
    
    public struct PixcelCache
    {
        public float red;
        public int index;
        public PixcelCache(float red,int index)
        {
            this.red = red;
            this.index = index;
        }
        public void ChangeRed(float red)
        {
            this.red = red;
        }
    }
    
    public static class MapToolUtility
    {
        #region 数据
        private static readonly int texSize = 256;
        #endregion
        
        #region 场景
        public static string GetSceneRootPath(SceneResType type)
        {
            if(type == SceneResType.ArtScene)
            {
                return "Assets/Art/Scene";
            }
            else if(type == SceneResType.ClientScene)
            {
                return "Assets/Res/Scene";
            }
            else if(type == SceneResType.ArtSceneRes)
            {
                return "Assets/Art/SceneRes";
            }

            return string.Empty;
        }
        
        public static string GetEditorScenePath(string sceneName,SceneResType type)
        {
            string basePath = GetSceneRootPath(type);
            if(string.IsNullOrEmpty(basePath))
            {
                return string.Empty;
            }

            return string.Format("{0}/{1}.unity", basePath, sceneName);
        }        
        public static string[] GetSceneNames(SceneResType type = SceneResType.ArtScene)
        {
            string basePath = GetSceneRootPath(type);
            string[] scenes = AssetDatabase.FindAssets("t:Scene", new string[] {"Assets/Art/Scene"});
            string sceneGUID;
            for(int i = 0;i < scenes.Length;i++)
            {
                sceneGUID = scenes[i];
                scenes[i] = System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(sceneGUID));
            }
            return scenes;
        }
        
        public static Scene OpenScene(string sceneName,SceneResType type,OpenSceneMode sceneMode = OpenSceneMode.Single)
        {
            string scenePath = GetEditorScenePath(sceneName, type);
            Scene scene = EditorSceneManager.GetSceneByPath(scenePath);
            if(!scene.isLoaded)
            {
                scene = EditorSceneManager.OpenScene(scenePath,sceneMode);
            }

            return scene;
        }
        #endregion
        
        #region 数据资源
        //地图高度数据
        public static HeightMapData GetHeightMapData(string sceneName)
        {
            if(string.IsNullOrEmpty(sceneName))
            {
                return null;
            }

            string path = string.Format("{0}/{1}/HeightMapData.asset", GetSceneRootPath(SceneResType.ArtSceneRes),
                sceneName);
            HeightMapData heightMapData = AssetDatabase.LoadAssetAtPath<HeightMapData>(path);
            if(heightMapData == null)
            {
                heightMapData = ScriptableObject.CreateInstance<HeightMapData>();
                heightMapData.heightMap = AssetDatabase.LoadAssetAtPath<Texture2D>(
                    string.Format("{0}/{1}/Textures/TerrainHeight.bmp", GetSceneRootPath(SceneResType.ArtSceneRes),
                        sceneName));
                AssetDatabase.CreateAsset(heightMapData,path);
            }

            return heightMapData;
        }
        
        /// <summary>
        /// 根据多张通道图合并为一张图，用于与textureArray配合显示地图内容
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="passMapList"></param>
        /// <param name="texture2D"></param>
        public static void CreatePassMapTexture(string sceneName,List<Texture2D> passMapList,out Texture2D texture2D)
        {
            texture2D = new Texture2D(texSize,texSize,TextureFormat.RGB24,false,false);
            texture2D.filterMode = FilterMode.Point;
            Color color;
            float overSixTeen = 1 / 16f;
            string texture2DSavePath = string.Format("{0}/{1}/Textures/TerrianControlTex.png",
                GetSceneRootPath(SceneResType.ArtSceneRes), sceneName);
            for(int width = 0;width < texSize;width++)
            {
                for(int height = 0;height < texSize;height++)
                {
                    color = Color.black;
                    List<PixcelCache> pixcelCahce = GetPassColorList(passMapList,width,height);
                    pixcelCahce.Sort(PassColorCompare);
                    //统计颜色列表
                    List<PixcelCache> pixcelColorList = new List<PixcelCache>();
                    pixcelColorList.Add(pixcelCahce[0]);
                    if(pixcelCahce[1].red > 0.05f)
                    {
                        pixcelColorList.Add(pixcelCahce[1]);
                    }

                    int minTexSize = texSize - 1;
                    //八方向
                    for(int cross = -1;cross <= 1;cross++)
                    {
                        if(width == 0 && cross == -1 || width == minTexSize && cross == 1)
                        {
                            continue;
                        }
                        for(int portrait = -1;portrait <= 1;portrait++)
                        {
                            if(height == 0 && portrait == -1 || height == minTexSize && portrait == 1)
                            {
                                continue;
                            }

                            List<PixcelCache> curPixcelList =
                                GetPassColorList(passMapList, cross + width, portrait + height);
                            bool isContain = false;
                            foreach(PixcelCache pixcel in pixcelColorList)
                            {
                                PixcelCache findResult = curPixcelList.Find(_ =>
                                {
                                    return pixcel.index == _.index;
                                });
                                if(!findResult.Equals(default(PixcelCache)))
                                {
                                    isContain = true;
                                    if(pixcel.red < findResult.red)
                                    {
                                        pixcel.ChangeRed(findResult.red);
                                    }
                                }
                                if(!isContain)
                                {
                                    pixcelColorList.Add(findResult);
                                }
                            }
                        }
                    }
                    //整理后处理后的像素列表
                    pixcelColorList.Sort(PassColorCompare);
                    color.r = pixcelColorList.Count > 0 ? pixcelColorList[0].index * overSixTeen : 0;
                    color.g = pixcelColorList.Count > 1 ? pixcelColorList[1].index * overSixTeen : 0;
                    color.b = pixcelColorList.Count > 2 ? pixcelColorList[2].index * overSixTeen : 0;
                    texture2D.SetPixel(width,height,color);
                }
            }
            texture2D.Apply();
            
        }
        
        /// <summary>
        /// 获取通道图占用列表
        /// </summary>
        /// <param name="passMapList"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static List<PixcelCache> GetPassColorList(List<Texture2D> passMapList,int x,int y)
        {
            if(x >= texSize || y >= texSize)
            {
                return null;
            }

            List<PixcelCache> passIndexList = new List<PixcelCache>();
            for(int i = 0;i < passMapList.Count;i++)
            {
                passIndexList.Add(new PixcelCache(passMapList[i].GetPixel(x,y).r,i));
            }

            return passIndexList;
        }
        
        /// <summary>
        /// 颜色权重排序
        /// </summary>
        /// <param name="cacheA"></param>
        /// <param name="cacheB"></param>
        /// <returns></returns>
        public static int PassColorCompare(PixcelCache cacheA,PixcelCache cacheB)
        {
            return cacheB.red.CompareTo(cacheA.red);
        }
        #endregion
    }
}