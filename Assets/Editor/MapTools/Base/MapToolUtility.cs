using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MapTools.Base
{
    public enum SceneResType
    {
        ArtScene,
        ClientScene,
    }
    
    public static class MapToolUtility
    {
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
                scenes[i] = Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(sceneGUID));
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
    }
}