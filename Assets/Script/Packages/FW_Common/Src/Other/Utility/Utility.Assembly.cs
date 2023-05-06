using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    public static partial class Utility
    {
        public static class Assembly
        {
            #region 数据
            private static System.Reflection.Assembly[] s_Assemblies = null;
            private static Dictionary<string, System.Type> s_CachedTypes = new Dictionary<string, System.Type>(StringComparer.Ordinal);
            #endregion
            
            #region 获取
            public static System.Reflection.Assembly[] GetAssemblies()
            {
                if(s_Assemblies == null)
                    s_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
                return s_Assemblies;
            }
            
            public static System.Type GetType(string typeName)
            {
                if(string.IsNullOrEmpty(typeName))
                    CSDebug.LogError("无效的TypeName");
                
                System.Type type;
                
                if (s_CachedTypes.TryGetValue(typeName,out type))
                    return type;
                
                type = System.Type.GetType(typeName);
                if(type != null)
                {
                    s_CachedTypes[typeName] = type;
                    return type;
                }

                System.Reflection.Assembly[] m_Assemblies = GetAssemblies();
                foreach(System.Reflection.Assembly assembly in m_Assemblies)
                {
                    type = System.Type.GetType(String.Format("{0},{1}",typeName,assembly.FullName));
                    if(type != null)
                    {
                        s_CachedTypes.Add(typeName,type);
                        return type;
                    }
                }

                return null;
            }
            #endregion
            
            #region 设置
            public static void AssetChange()
            {
                s_Assemblies = null;
            }
            #endregion
        }
    }
}