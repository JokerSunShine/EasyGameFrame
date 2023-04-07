using System.Diagnostics;
using UnityEngine;

namespace Framework
{
    public static partial class Utility
    {
        public static class Folder
        {
            public static void Execute(string path)
            {
                path = String.Format("\"{0}\"", path);
                switch(Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        Process.Start("Explorer.exe", path.Replace('/', '\\'));
                        break;
                    case RuntimePlatform.OSXEditor:
                        Process.Start("open", path);
                        break;
                }
            }
        }
    }
}