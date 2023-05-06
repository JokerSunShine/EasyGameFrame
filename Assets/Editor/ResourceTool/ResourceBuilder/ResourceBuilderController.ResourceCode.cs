namespace ResourceTool
{
    public partial class ResourceBuilderController
    {
        public sealed class ResourceCode
        {
            #region 数据
            private readonly Platform m_Platform;
            public Platform Platform
            {
                get
                {
                    return m_Platform;
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
            
            private readonly int m_ZipLength;
            public int ZipLength
            {
                get
                {
                    return m_ZipLength;
                }
            }
            
            private readonly int m_ZipHashCode;
            public int ZipHashCode
            {
                get
                {
                    return m_ZipHashCode;
                }
            }
            #endregion
            
            #region 构造
            public ResourceCode(Platform platform, int length, int hashCode, int zipLength, int zipHashCode)
            {
                m_Platform = platform;
                m_Length = length;
                m_HashCode = hashCode;
                m_ZipLength = zipLength;
                m_ZipHashCode = zipHashCode;
            }
            #endregion
        }
    }
}