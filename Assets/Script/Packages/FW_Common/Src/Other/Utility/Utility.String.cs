using System;
using System.Text;
using _3DMath;

namespace Framework
{
    public static partial class Utility
    {
        public static class String
        {
            [ThreadStatic]
            private static StringBuilder cacheStringBuilder = null;
            
            public static string Format(string format,string arg0)
            {
                if(string.IsNullOrEmpty(format))
                {
                    throw new Exception("数据格式无效");
                }

                CheckCacheStringBuilder();
                cacheStringBuilder.Length = 0;
                cacheStringBuilder.AppendFormat(format, arg0);
                return cacheStringBuilder.ToString();
            }
            
            public static string Format(string format,string arg0,string arg1)
            {
                if(string.IsNullOrEmpty(format))
                {
                    throw new Exception("数据格式无效");
                }

                CheckCacheStringBuilder();
                cacheStringBuilder.Length = 0;
                cacheStringBuilder.AppendFormat(format, arg0,arg1);
                return cacheStringBuilder.ToString();
            }
            
            public static string Format(string format,string arg0,string arg1,string arg2)
            {
                if(string.IsNullOrEmpty(format))
                {
                    throw new Exception("数据格式无效");
                }

                CheckCacheStringBuilder();
                cacheStringBuilder.Length = 0;
                cacheStringBuilder.AppendFormat(format, arg0,arg1,arg2);
                return cacheStringBuilder.ToString();
            }
            
            public static string Format(string format,string[] args)
            {
                if(string.IsNullOrEmpty(format))
                {
                    throw new Exception("数据格式无效");
                }
                
                if(args == null)
                {
                    throw new Exception("参数无效");
                }

                CheckCacheStringBuilder();
                cacheStringBuilder.Length = 0;
                cacheStringBuilder.AppendFormat(format,args);
                return cacheStringBuilder.ToString();
            }
            
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
            
            private static void CheckCacheStringBuilder()
            {
                if(cacheStringBuilder == null)
                    cacheStringBuilder = new StringBuilder(MathUtility.StringBuilderSize);
            }
        }
    }
}

