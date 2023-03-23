using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;


namespace ExtendEditor
{
    public static class SelectionEx
    {
        public static string[] GetSelectedFolders()
        {
            List<string> folderList = new List<string>();
            var guids = Selection.assetGUIDs;
            foreach (var item in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(item);
                if (File.Exists(path))
                {
                    path = FileUtility.GetDirectory(path);
                }
                if (Directory.Exists(path))
                {
                    if (!folderList.Contains(path))
                    {
                        folderList.Add(path);
                    }
                }
            }
            return folderList.ToArray();
        }
    
        public static string[] GetSelectedCurFolders()
        {
            List<string> folderList = new List<string>();
            var guids = Selection.assetGUIDs;
            foreach (var item in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(item);
                if (Directory.Exists(path))
                {
                    if (!folderList.Contains(path))
                    {
                        folderList.Add(path);
                    }
                }
            }
            return folderList.ToArray();
        }
        
        public static string[] GetSelectedFiles(string patten = "")
        {
            List<string> fileList = new List<string>();
            var guids = Selection.assetGUIDs;
            foreach (var item in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(item);
                if (File.Exists(path) && (string.IsNullOrEmpty(patten) || Regex.Match(path,patten).Success))
                {
                    if (!fileList.Contains(path))
                    {
                        fileList.Add(path);
                    }
                }
            }
            return fileList.ToArray();
        }
    }
}