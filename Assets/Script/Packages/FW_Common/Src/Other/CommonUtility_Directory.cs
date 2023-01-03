using System.IO;

namespace Packages.FW_Common.Other
{
    public static partial class CommonUtility_Directory
    {
        public static string[] GetAllFilePath(string rootPath,string searchPatten = "*")
        {
            if(Directory.Exists(rootPath) == false)
            {
                return null;
            }

            return Directory.GetFiles(rootPath, searchPatten);
        }
    }
}