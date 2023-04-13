using System.Collections.Generic;
using Framework;

namespace ResourceTool
{
    public class Resource
    {
        #region 数据
        public string Name
        {
            get;
            private set;
        }
        
        public string Variant
        {
            get;
            private set;
        }
        
        public string FullName
        {
            get
            {
                return string.IsNullOrEmpty(Variant) ? Name : Utility.String.Format("{0}.{1}", Name, Variant);
            }
        }
        
        public string FileSystem
        {
            get;
            set;
        }
        
        public LoadType LoadType
        {
            get;
            set;
        }
        
        public bool Packed
        {
            get;
            set;
        }
        
        public AssetType AssetType
        {
            get;
            private set;
        }

        private readonly List<Asset> Assets;
        private readonly List<string> ResourceGroups;
        #endregion
        
        #region 构造
        private Resource(string name,string variant,string fileSystem,LoadType loadType,bool packed,string[] resourceGroups)
        {
            Name = name;
            Variant = variant;
            FileSystem = fileSystem;
            LoadType = loadType;
            Packed = packed;
            Assets = new List<Asset>();
            ResourceGroups = new List<string>();
            AssetType = AssetType.UNKNOW;
            
            foreach(string resourceGroup in resourceGroups)
            {
                AddResourceGroup(resourceGroup);
            }
        }
        
        public static Resource Create(string name,string variant,string fileSystem,LoadType loadType,bool packed,string[] resourceGroup)
        {
            return new Resource(name,variant,fileSystem,loadType,packed,resourceGroup ?? new string[0]);
        }
        #endregion
        
        #region 获取
        public Asset[] GetAssets()
        {
            return Assets.ToArray();
        }
        
        public string[] GetResourceGroups()
        {
            return ResourceGroups.ToArray();
        }
        #endregion
        
        #region 查询
        public bool HasResourceGroup(string resourceGroup)
        {
            if (string.IsNullOrEmpty(resourceGroup))
                return false;
            return ResourceGroups.Contains(resourceGroup);
        }
        #endregion
        
        #region 设置
        public void Rename(string name,string variant)
        {
            Name = name;
            Variant = variant;
        }
        
        public void AddAsset(Asset asset)
        {
            if (asset.Resource != null)
                RemoveAsset(asset);
            AssetType = AssetType.ASSET;
            asset.Resource = this;
            Assets.Add(asset);
            Assets.Sort(AssetCompare);
        }
        
        public void RemoveAsset(Asset asset)
        {
            asset.Resource = null;
            Assets.Remove(asset);
            if (Assets.Count <= 0)
                AssetType = AssetType.UNKNOW;
        }
        
        public void AddResourceGroup(string resourceGroup)
        {
            if (string.IsNullOrEmpty(resourceGroup))
                return;
            if (ResourceGroups.Contains(resourceGroup))
                return;
            ResourceGroups.Add(resourceGroup);
            ResourceGroups.Sort();
        }
        
        public bool RemoveResourceGroup(string resourceGroup)
        {
            if (string.IsNullOrEmpty(resourceGroup))
                return false;
            return ResourceGroups.Remove(resourceGroup);
        }
        #endregion
        
        #region 对比
        private int AssetCompare(Asset a,Asset b)
        {
            return a.CompareTo(b);
        }
        #endregion
        
        #region 销毁
        public void Clear()
        {
            foreach(Asset asset in Assets)
            {
                asset.Clear();
            }
            Assets.Clear();
            ResourceGroups.Clear();
        }
        #endregion
    }
}