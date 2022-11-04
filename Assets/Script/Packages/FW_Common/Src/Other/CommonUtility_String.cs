public static partial class CommonUtility
{
    /// <summary>
    /// 移除指定数量目录
    /// </summary>
    /// <param name="dir">原始目录</param>
    /// <param name="time">清除次数</param>
    /// <returns>清理后的目录</returns>
    public static string RemoveBackDir(string dir,int time)
    {
        dir = dir.Replace("\\","/");
        for(int i = 0;i < time;i++)
        {
            int lastIndex = dir.LastIndexOf("/");
            if (lastIndex == dir.Length - 1)
            {
                dir = dir.Substring(0,dir.Length -1);
            }
            lastIndex = dir.LastIndexOf("/");
            dir = dir.Substring(0,lastIndex);
        }
        return dir;
    }
}
