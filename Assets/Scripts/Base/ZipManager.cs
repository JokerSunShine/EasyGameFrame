using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;

public class ZipManager
{
    /// <summary>
    /// 创建压缩文件
    /// </summary>
    /// <param name="zipPath">文件路径</param>
    /// <param name="fileDict">压缩内容</param>
    /// <param name="compressionLevel"></param>
    public static void CreateZip(string zipPath,Dictionary<string,byte[]> fileDict,int compressionLevel = 0)
    {
        FileStream fileStream = File.Create(zipPath);
        ZipOutputStream zipStream = new ZipOutputStream(fileStream);
        zipStream.SetLevel(compressionLevel);
        foreach (string fileName in fileDict.Keys)
        {
            zipStream.PutNextEntry(new ZipEntry(fileName));
            byte[] buffer = fileDict[fileName];
            zipStream.Write(buffer,0,buffer.Length);
        }
        zipStream.Flush();
        zipStream.Close();
        zipStream.Close();
    }
}
