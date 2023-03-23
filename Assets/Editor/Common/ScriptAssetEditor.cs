using System.IO;
using UnityEditor;
using UnityEngine;

namespace ExtendEditor
{
    public class ScriptAssetEditor : EditorWindow
    {
        private static string[] paths;
        
        [MenuItem("Assets/Create/AssetScript",true)]
        private static bool ValidateSelection()
        {
            paths = SelectionEx.GetSelectedFiles(".cs");
            return paths.Length > 0;
        }
        
        [MenuItem("Assets/Create/AssetScript",false)]
        private static void CreateAsset()
        {
            if(paths == null || paths.Length <= 0)
            {
                return;
            }
            foreach(string path in paths)
            {
                string fileName = Path.GetFileNameWithoutExtension(path);
                Object ob = ScriptableObject.CreateInstance(fileName);
                if(ob == null)
                {
                    continue;
                }
                string extension = Path.GetExtension(path);
                string targetPath = path.Replace(".cs", ".asset");
                AssetDatabase.CreateAsset(ob,targetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}