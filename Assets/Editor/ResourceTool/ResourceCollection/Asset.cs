using System;
using UnityEditor;

namespace ResourceTool
{
    public sealed class Asset : IComparable<Asset>
    {
        #region 数据
        public string Guid
        {
            get;
            private set;
        }
        
        public string Name
        {
            get
            {
                return AssetDatabase.GUIDToAssetPath(Guid);
            }
        }
        
        public Resource Resource
        {
            get;
            set;
        }
        #endregion
        
        #region 构造
        public Asset(string guid,Resource resource)
        {
            Guid = guid;
            Resource = resource;
        }
        #endregion
        
        #region 静态
        public static Asset Create(string guid)
        {
            return new Asset(guid, null);
        }
        
        public static Asset Create(string guid,Resource resource)
        {
            return new Asset(guid, resource);
        }
        #endregion
        
        #region 功能
        public int CompareTo(Asset other)
        {
            return string.Compare(Guid, other.Guid, StringComparison.Ordinal);
        }
        #endregion
        
        #region 销毁
        public void Clear()
        {
            Resource = null;
        }
        #endregion

    }
}