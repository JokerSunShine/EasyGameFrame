using Script.Packages.FW_Core.Src;

namespace Script.Packages.FW_Resource.ResourceLoad
{
    #region 枚举
    public enum ResourceMode
    {
        Editor = 0,
        Package = 1,
        HotUpdate = 2,
    }
    #endregion
    
    #region 构造
    public class ResourceAsset : AbstractResource
    {
        #region 数据
        #endregion

        public override void Awake()
        {
            
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public override void Init()
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }

        public override void ClearMemory()
        {
        }

        public override bool FileExists(string path)
        {
            return base.FileExists(path);
        }

        public override void FixedUpdate()
        {
        }

        public override void LateUpdate()
        {
        }

        public override byte[] LoadFile(string path)
        {
            return base.LoadFile(path);
        }

        public override void SaveFile(string path, byte[] bytes)
        {
        }

        public override void SaveFile(string path, string text)
        {
        }
    }
    #endregion
}