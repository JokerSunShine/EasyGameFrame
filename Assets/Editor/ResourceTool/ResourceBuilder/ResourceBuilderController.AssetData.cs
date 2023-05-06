namespace ResourceTool
{
    public partial class ResourceBuilderController
    {
        public sealed class AssetData
        {
            #region 数据
            private readonly string m_Guid;
            public string Guid
            {
                get
                {
                    return m_Guid;
                }
            }
            
            private readonly string m_Name;
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }
            
            private readonly int m_Length;
            public int Length
            {
                get
                {
                    return m_Length;
                }
            }
            
            private readonly int m_HashCode;
            public int HashCode
            {
                get
                {
                    return m_HashCode;
                }
            }
            
            
            private readonly string[] m_DependencyAssetNames;
            public string[] DependencyAssetNames
            {
                get
                {
                    return m_DependencyAssetNames;
                }
            }
            #endregion
            
            #region 构造
            public AssetData(string guid, string name, int length, int hashCode, string[] dependencyAssetNames)
            {
                m_Guid = guid;
                m_Name = name;
                m_Length = length;
                m_HashCode = hashCode;
                m_DependencyAssetNames = dependencyAssetNames;
            }
            #endregion
        }
    }
}