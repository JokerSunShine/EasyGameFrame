using System.Collections.Generic;
using Framework;
using UnityEditor;

namespace ResourceTool
{
    public sealed partial class ResourceBuilderController
    {
        #region 数据
        public Platform Platforms
        {
            get;
            set;
        }
        
        public bool ZipSelected
        {
            get;
            set;
        }
        
        public bool UncompressedAssetBundleSelected
        {
            get;
            set;
        }
        
        public bool DisableWriteTypeTreeSelected
        {
            get;
            set;
        }
        
        public bool DeterministicAssetBundleSelected
        {
            get;
            set;
        }
        
        public bool ForceRebuildAssetBundleSelected
        {
            get;
            set;
        }
        
        public bool IgnoreTypeTreeChangesSelected
        {
            get;
            set;
        }
        
        public bool AppendHashToAssetBundleNameSelected
        {
            get;
            set;
        }
        
        public bool ChunkBasedCompressionSelected
        {
            get;
            set;
        }
        
        public bool StrictModeSelected
        {
            get;
            set;
        }
        
        public bool DryRunBuildSelected
        {
            get;
            set;
        }
        
        public bool DisableLoadAssetByFileNameSelected
        {
            get;
            set;
        }
        
        public bool DisableLoadAssetByFileNameWithExtensionSelected
        {
            get;
            set;
        }
        
        public bool AssetBundleStripUnityVersionSelected
        {
            get;
            set;
        }

        private const string NoneOptionName = "<None>";
        private IBuildEventHandler m_BuildEventHandler;
        private readonly List<string> m_BuildEventHandlerTypeNames;
        #endregion
        
        #region 构造
        public ResourceBuilderController()
        {
            m_BuildEventHandlerTypeNames = new List<string>()
            {
                NoneOptionName
            };
            m_BuildEventHandlerTypeNames.AddRange(Utility.Type.GetEditorTypeNames(typeof(IBuildEventHandler)));
        }
        #endregion
        
        #region 查询
        public bool PlatformIsSelected(Platform platform)
        {
            return (Platforms & platform) != 0;
        }
        #endregion
        
        #region 设置
        public void SelectPlatform(Platform platform,bool selected)
        {
            if (selected)
                Platforms |= platform;
            else
                Platforms &= ~platform;
        }
        
        public void SetBuildEventHandler(IBuildEventHandler buildEventHandler)
        {
            m_BuildEventHandler = buildEventHandler;
        }
        
        public bool SetBuildEventHandlerByName(string buildEventHandlerName)
        {
            if(string.IsNullOrEmpty(buildEventHandlerName))
            {
                buildEventHandlerName = string.Empty;
                SetBuildEventHandler(null);
                return true;
            }
            if(m_BuildEventHandlerTypeNames.Contains(buildEventHandlerName))
            {
                
            }

            return true;
        }
        #endregion
    }
}