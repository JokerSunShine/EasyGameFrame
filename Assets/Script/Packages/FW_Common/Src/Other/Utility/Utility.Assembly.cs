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
            public static Dictionary<string, System.Type> s_CachedTypes = new Dictionary<string, System.Type>(StringComparer.Ordinal);
            #endregion
            
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

                return null;
            }
        }
    }
}