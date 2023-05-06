using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Framework;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ResourceTool
{
    /// <summary>
    /// 资源创建控制器
    /// </summary>
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
        
        public int InternalResourceVersion
        {
            get;
            set;
        }

        public bool OutputPackageSelected
        {
            get;
            set;
        }
        
        public bool OutputFullSelected
        {
            get;
            set;
        }
        
        public bool OutputPackedSelected
        {
            get;
            set;
        }
        
        public string BuildEventHandlerTypeName
        {
            get;
            set;
        }
        
        public string OutputDirectory
        {
            get;
            set;
        }
        
        public string ApplicationGameVersion
        {
            get
            {
                return Application.version;
            }
        }
        
        public bool IsValidOutputDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(OutputDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(OutputDirectory))
                {
                    return false;
                }

                return true;            
            }
        }
        
        public string WorkingPath
        {
            get
            {
                if(!IsValidOutputDirectory)
                    return string.Empty;
                return Utility.Path.GetRegularPath(Utility.String.Format("{0}/Working", OutputDirectory));
            }
        }
        
        public string PackagePath
        {
            get
            {
                if(!IsValidOutputDirectory)
                    return string.Empty;
                return Utility.Path.GetRegularPath(Utility.String.Format("{0}/Package/{1}_{2}", OutputDirectory,ApplicationGameVersion.Replace('.','_'),InternalResourceVersion.ToString()));
            }
        }
        
        public string FullPath
        {
            get
            {
                if(!IsValidOutputDirectory)
                    return string.Empty;
                return Utility.Path.GetRegularPath(Utility.String.Format("{0}/Full/{1}_{2}", OutputDirectory,ApplicationGameVersion.Replace('.','_'),InternalResourceVersion.ToString()));
            }
        }
        
        public string PackedPath
        {
            get
            {
                if(!IsValidOutputDirectory)
                    return string.Empty;
                return Utility.Path.GetRegularPath(Utility.String.Format("{0}/Packed/{1}_{2}", OutputDirectory,ApplicationGameVersion.Replace('.','_'),InternalResourceVersion.ToString()));
            }
        }
        
        public string BuildReportPath
        {
            get
            {
                if(!IsValidOutputDirectory)
                    return string.Empty;
                return Utility.Path.GetRegularPath(Utility.String.Format("{0}/BuildReport/{1}_{2}", OutputDirectory,ApplicationGameVersion.Replace('.','_'),InternalResourceVersion.ToString()));
            }
        }
        
        private const string NoneOptionName = "<None>";
        private const string DefaultExtension = "dat";
        
        private IBuildEventHandler m_BuildEventHandler;
        
        private readonly List<string> m_BuildEventHandlerTypeNames;
        
        private readonly BuildReport m_BuildReport;
        private readonly ResourceCollection m_ResourceCollection;
        #endregion
        
        #region 事件
        public event Action<int, int> OnLoadingResource = null;
        public event Action<int,int> OnLoadingAsset = null;
        public event Action OnLoadCompleted = null;
        #endregion
        
        #region 构造
        public ResourceBuilderController()
        {
            m_BuildEventHandlerTypeNames = new List<string>()
            {
                NoneOptionName
            };
            m_BuildEventHandlerTypeNames.AddRange(Utility.Type.GetEditorTypeNames(typeof(IBuildEventHandler)));
            
            m_BuildReport = new BuildReport();
            
            m_ResourceCollection = new ResourceCollection();
            m_ResourceCollection.OnLoadingResource += (index, count) =>
            {
                if(OnLoadingResource != null)
                {
                    OnLoadingResource(index,count);
                }
            };
            m_ResourceCollection.OnLoadingAsset += (index, count) =>
            {
                if(OnLoadingAsset != null)
                {
                    OnLoadingAsset(index,count);
                }
            };
            m_ResourceCollection.OnLoadCompleted += () =>
            {
                if(OnLoadCompleted != null)
                {
                    OnLoadCompleted();
                }
            };
            
            
        }
        #endregion
        
        #region 查询
        public bool PlatformIsSelected(Platform platform)
        {
            return (Platforms & platform) != 0;
        }
        #endregion
        
        #region 获取
        public string[] GetBuildEventHandlerTypeNames()
        {
            return m_BuildEventHandlerTypeNames.ToArray();
        }
        
        public int GetBuildEventHandlerIndex()
        {
            string[] names = GetBuildEventHandlerTypeNames();
            for(int i = 0;i < names.Length;i++)
            {
                if(BuildEventHandlerTypeName == names[i])
                {
                    return i;
                }
            }

            return 0;
        }
        
        public static string GetExtension(ResourceData resourceData)
        {
            if(resourceData.IsLoadFromBinary)
            {
                string assetName = resourceData.GetAssetNames()[0];
                int position = assetName.LastIndexOf('.');
                if (position >= 0)
                {
                    return assetName.Substring(position + 1);
                }
            }

            return DefaultExtension;
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
        
        public bool SetBuildEventHandlerByIndex(int buildEventHandlerIndex)
        {
            if(buildEventHandlerIndex <= 0)
            {
                SetBuildEventHandlerByName(string.Empty);
                return true;
            }
            string[] names = GetBuildEventHandlerTypeNames();
            if(names == null || names.Length <= 0 || buildEventHandlerIndex >= names.Length)
            {
                SetBuildEventHandlerByName(string.Empty);
                return false;
            }
            SetBuildEventHandlerByName(names[buildEventHandlerIndex]);
            return true;
        }
        
        public bool SetBuildEventHandlerByName(string buildEventHandlerName)
        {
            if(string.IsNullOrEmpty(buildEventHandlerName))
            {
                BuildEventHandlerTypeName = string.Empty;
                SetBuildEventHandler(null);
                return true;
            }
            if(m_BuildEventHandlerTypeNames.Contains(buildEventHandlerName))
            {
                Type buildEventHandlerType = Utility.Assembly.GetType(buildEventHandlerName);
                if(buildEventHandlerType != null)
                {
                    IBuildEventHandler buildEventHandler = (IBuildEventHandler)Activator.CreateInstance(buildEventHandlerType);
                    if(buildEventHandler != null)
                    {
                        BuildEventHandlerTypeName = buildEventHandlerName;
                        SetBuildEventHandler(buildEventHandler);
                        return true;
                    }
                }
            }
            BuildEventHandlerTypeName = string.Empty;
            SetBuildEventHandler(null);
            return false;
        }
        #endregion
        
        #region 配置文件
        public bool LoadConfig()
        {
            if(!File.Exists(Utility.Path.ResourceBuilderConfigFilePath))
            {
                return false;
            }
            
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Utility.Path.ResourceBuilderConfigFilePath);
                XmlNode xmlResource = xmlDocument.SelectSingleNode("ResourceBuilder");
                XmlNode xmlSettings = xmlResource.SelectSingleNode("Settings");

                XmlNodeList xmlNodeList;
                XmlNode xmlNode;

                xmlNodeList = xmlSettings.ChildNodes;
                for(int i = 0;i < xmlNodeList.Count;i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    switch (xmlNode.Name)
                    {
                        case "InternalResourceVersion":
                            InternalResourceVersion = int.Parse(xmlNode.InnerText); 
                            break;
                        case "Platforms":
                            Platforms = (Platform)int.Parse(xmlNode.InnerText); 
                            break;
                        case "ZipSelected":
                            ZipSelected = bool.Parse(xmlNode.InnerText); 
                            break;
                        case "UncompressedAssetBundleSelected":
                            UncompressedAssetBundleSelected = bool.Parse(xmlNode.InnerText);
                            if (UncompressedAssetBundleSelected)
                                ChunkBasedCompressionSelected = false;
                            break;
                        case "DisableWriteTypeTreeSelected":
                            DisableWriteTypeTreeSelected = bool.Parse(xmlNode.InnerText); 
                            if (DisableWriteTypeTreeSelected)
                                IgnoreTypeTreeChangesSelected = false;
                            break;
                        case "DeterministicAssetBundleSelected":
                            DeterministicAssetBundleSelected = bool.Parse(xmlNode.InnerText); 
                            break;
                        case "ForceRebuildAssetBundleSelected":
                            ForceRebuildAssetBundleSelected = bool.Parse(xmlNode.InnerText); 
                            break;
                        case "IgnoreTypeTreeChangesSelected":
                            IgnoreTypeTreeChangesSelected = bool.Parse(xmlNode.InnerText); 
                            if (IgnoreTypeTreeChangesSelected)
                                DisableWriteTypeTreeSelected = false;
                            break;
                        case "AppendHashToAssetBundleNameSelected":
                            AppendHashToAssetBundleNameSelected = bool.Parse(xmlNode.InnerText); 
                            break;
                        case "ChunkBasedCompressionSelected":
                            ChunkBasedCompressionSelected = bool.Parse(xmlNode.InnerText); 
                            if (ChunkBasedCompressionSelected)
                                UncompressedAssetBundleSelected = false;
                            break;
                        case "OutputPackageSelected":
                            OutputPackageSelected = bool.Parse(xmlNode.InnerText); 
                            break;
                        case "OutputFullSelected":
                            OutputFullSelected = bool.Parse(xmlNode.InnerText); 
                            break;
                        case "OutputPackedSelected":
                            OutputPackedSelected = bool.Parse(xmlNode.InnerText); 
                            break;
                        case "BuildEventHandlerTypeName":
                            BuildEventHandlerTypeName = xmlNode.InnerText; 
                            break;
                        case "OutputDirectory":
                            OutputDirectory = xmlNode.InnerText; 
                            break;
                    }
                }
            }
            catch
            {
                File.Delete(Utility.Path.ResourceBuilderConfigFilePath);
                return false;
            }

            return true;
        }
        
        public bool SaveConfig()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));
                
                XmlElement xmlResourceBuilder = xmlDocument.CreateElement("ResourceBuilder");
                xmlDocument.AppendChild(xmlResourceBuilder);

                XmlElement xmlSetting = xmlDocument.CreateElement("Settings");
                xmlResourceBuilder.AppendChild(xmlSetting);

                XmlElement xmlElement;

                xmlElement = xmlDocument.CreateElement("InternalResourceVersion");
                xmlElement.InnerText = InternalResourceVersion.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("Platforms");
                xmlElement.InnerText = ((int)Platforms).ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("ZipSelected");
                xmlElement.InnerText = ZipSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("UncompressedAssetBundleSelected");
                xmlElement.InnerText = UncompressedAssetBundleSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("DisableWriteTypeTreeSelected");
                xmlElement.InnerText = DisableWriteTypeTreeSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("DeterministicAssetBundleSelected");
                xmlElement.InnerText = DeterministicAssetBundleSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("ForceRebuildAssetBundleSelected");
                xmlElement.InnerText = ForceRebuildAssetBundleSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("IgnoreTypeTreeChangesSelected");
                xmlElement.InnerText = IgnoreTypeTreeChangesSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("AppendHashToAssetBundleNameSelected");
                xmlElement.InnerText = AppendHashToAssetBundleNameSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("ChunkBasedCompressionSelected");
                xmlElement.InnerText = ChunkBasedCompressionSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("OutputPackageSelected");
                xmlElement.InnerText = OutputPackageSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("OutputFullSelected");
                xmlElement.InnerText = OutputFullSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("OutputPackedSelected");
                xmlElement.InnerText = OutputPackedSelected.ToString();
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("BuildEventHandlerTypeName");
                xmlElement.InnerText = BuildEventHandlerTypeName;
                xmlSetting.AppendChild(xmlElement);
                
                xmlElement = xmlDocument.CreateElement("OutputDirectory");
                xmlElement.InnerText = OutputDirectory;
                xmlSetting.AppendChild(xmlElement);

                string directoryName = Path.GetDirectoryName(Utility.Path.ResourceBuilderConfigFilePath);
                if (Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);
                xmlDocument.Save(Utility.Path.ResourceBuilderConfigFilePath);
                AssetDatabase.Refresh();
                return true;
            }
            catch
            {
                if(File.Exists(Utility.Path.ResourceBuilderConfigFilePath))
                {
                    File.Delete(Utility.Path.ResourceBuilderConfigFilePath);
                }
                return false;
            }
        }
        #endregion
        
        #region 资源预处理
        
        #endregion
    }
}