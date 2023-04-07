using System.IO;
using UnityEditor;

namespace ExtendEditor
{
    public class FileUtility
    {
        public static string GetDirectory(UnityEngine.Object obj)
        {
            string path = "Assets";
            path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(path))
            {
                path = System.IO.Path.GetDirectoryName(path);
            }
            return path;
        }
        
        public static string GetDirectory(string path)
        {
            int lastIndex = path.LastIndexOf("/");
            if (lastIndex == -1) return "";

            path = path.Substring(0, lastIndex);
            return path;
        }
    }
}
