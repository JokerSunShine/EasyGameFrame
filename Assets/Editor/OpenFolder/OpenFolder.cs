using UnityEditor;
using UnityEngine;

namespace Framework
{
    public static class OpenFolder
    {
        [MenuItem("Tools/Open Folder/Data Path",false,1)]
        public static void OpenFolderDataPath()
        {
            Utility.Folder.Execute(Application.dataPath);
        }
        
        [MenuItem("Tools/Open Folder/Stream Asset Path",false,3)]
        public static void OpenStreamAssetsPath()
        {
            Utility.Folder.Execute(Application.streamingAssetsPath);
        }
        
        [MenuItem("Tools/Open Folder/Console Log Path",false,4)]
        public static void OpenFolderConsoleLogPath()
        {
            Utility.Folder.Execute(Application.consoleLogPath);
        }
        
        [MenuItem("Tools/Open Folder/Persisten Data Path",false,2)]
        public static void OpenFolderPersistenDataPath()
        {
            Utility.Folder.Execute(Application.persistentDataPath);
        }
        
        [MenuItem("Tools/Open Folder/Temporary Cache Path",false,5)]
        public static void OpenFolderTemporaryCachePath()
        {
            Utility.Folder.Execute(Application.temporaryCachePath);
        }
    }
}