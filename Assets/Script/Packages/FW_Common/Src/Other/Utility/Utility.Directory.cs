namespace Framework
{
    public static partial class Utility
    {
        public static class Directory
        {
            public static string[] GetAllFilePath(string rootPath,string searchPatten = "*")
            {
                if(System.IO.Directory.Exists(rootPath) == false)
                {
                    return null;
                }

                return System.IO.Directory.GetFiles(rootPath, searchPatten);
            }
        }
    }
}