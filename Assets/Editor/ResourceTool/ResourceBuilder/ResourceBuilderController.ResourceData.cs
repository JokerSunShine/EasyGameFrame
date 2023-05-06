using System.Collections.Generic;

namespace ResourceTool
{
    public partial class ResourceBuilderController
    {
        public sealed class ResourceData
        {
            #region 数据
            private readonly string m_Name;
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }
            
            private readonly string m_Variant;
            public string Variant
            {
                get
                {
                    return m_Variant;
                }
            }
            
            private readonly string m_FileSystem;
            public string FileSystem
            {
                get
                {
                    return m_FileSystem;
                }
            }
            
            private readonly LoadType m_LoadType;
            public LoadType LoadType
            {
                get
                {
                    return m_LoadType;
                }
            }
            
            private readonly bool m_Packed;
            public bool Packed
            {
                get
                {
                    return m_Packed;
                }
            }
            
            public bool IsLoadFromBinary
            {
                get
                {
                    return m_LoadType == LoadType.LoadFromBinary || m_LoadType == LoadType.LoadFromBinaryAndQuickDecrypt || m_LoadType == LoadType.LoadFromBinaryAndDecrypt;
                }
            }


            public int AssetCount
            {
                get
                {
                    return m_AssetDatas.Count;
                }
            }
            
            private readonly string[] m_ResourceGroups;
            public string[] ResourceGroups
            {
                get
                {
                    return m_ResourceGroups;
                }
            }
            
            private readonly List<AssetData> m_AssetDatas;
            private readonly List<ResourceCode> m_Codes;
            #endregion
            
            #region 构造
            public ResourceData(string name, string variant, string fileSystem, LoadType loadType, bool packed, string[] resourceGroups)
            {
                m_Name = name;
                m_Variant = variant;
                m_FileSystem = fileSystem;
                m_LoadType = loadType;
                m_Packed = packed;
                m_ResourceGroups = resourceGroups;
                m_AssetDatas = new List<AssetData>();
                m_Codes = new List<ResourceCode>();
            }
            #endregion
            
            #region 获取
            public string[] GetAssetGuids()
            {
                string[] assetGuids = new string[m_AssetDatas.Count];
                for (int i = 0; i < m_AssetDatas.Count; i++)
                {
                    assetGuids[i] = m_AssetDatas[i].Guid;
                }

                return assetGuids;
            }
            
            public string[] GetAssetNames()
            {
                string[] assetNames = new string[m_AssetDatas.Count];
                for (int i = 0; i < m_AssetDatas.Count; i++)
                {
                    assetNames[i] = m_AssetDatas[i].Name;
                }

                return assetNames;
            }
            
            public AssetData[] GetAssetDatas()
            {
                return m_AssetDatas.ToArray();
            }
            
            public AssetData GetAssetData(string assetName)
            {
                foreach (AssetData assetData in m_AssetDatas)
                {
                    if (assetData.Name == assetName)
                    {
                        return assetData;
                    }
                }

                return null;
            }
            
            public ResourceCode GetCode(Platform platform)
            {
                foreach (ResourceCode code in m_Codes)
                {
                    if (code.Platform == platform)
                    {
                        return code;
                    }
                }

                return null;
            }
            
            public ResourceCode[] GetCodes()
            {
                return m_Codes.ToArray();
            }
            #endregion
            
            #region 添加
            public void AddAssetData(string guid, string name, int length, int hashCode, string[] dependencyAssetNames)
            {
                m_AssetDatas.Add(new AssetData(guid, name, length, hashCode, dependencyAssetNames));
            }
            
            public void AddCode(Platform platform, int length, int hashCode, int zipLength, int zipHashCode)
            {
                m_Codes.Add(new ResourceCode(platform, length, hashCode, zipLength, zipHashCode));
            }
            #endregion
        }
    }
}