public static class CommonUtility_Filed
{
    public static string GetFileName(string path)
    {
        path = path.Replace("\\", "/");
        string dir = GetDirectory(path);
        path = path.Replace(dir + "/", "");
        path = path.Substring(0, path.LastIndexOf("."));
        return path;
    }

    public static string GetDirectory(string path)
    {
        path = path.Replace("\\", "/");
        int lastIndex = path.LastIndexOf("/");
        if (lastIndex == -1) return "";

        path = path.Substring(0, lastIndex);
        return path;
    }
}
