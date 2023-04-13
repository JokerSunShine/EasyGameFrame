using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Framework;
using ICSharpCode.NRefactory.Ast;
using UnityEditor;
using UnityEngine;

namespace ResourceTool
{
    public class ResourceCollection
    {
        #region 数据
        private const string SceneExtension = ".unity";
        
        private static readonly Regex ResourceNameRegex = new Regex(@"^([A-Za-z0-9\._-]+/)*[A-Za-z0-9\._-]+$");
        private static readonly Regex ResourceVariantRegex = new Regex(@"^[a-z0-9_-]+$");
        
        private readonly string configPath;
        private readonly SortedDictionary<string, Resource> Resources;
        private readonly SortedDictionary<string, Asset> Assets;
        #endregion
        
        #region 事件
        public event Action<int,int> OnLoadingResource = null;
        public event Action<int,int> OnLoadingAsset = null;
        public event Action OnLoadCompleted = null;
        #endregion
        
        #region 构造
        public ResourceCollection()
        {
            configPath = Utility.Path.ResourceCollectionConfigFilePath;
            Resources = new SortedDictionary<string,Resource>();
            Assets = new SortedDictionary<string, Asset>();
        }
        #endregion
        
        #region 获取
        public Resource GetResource(string name,string variant)
        {
            if (!IsValidResourceName(name, variant))
                return null;

            if (Resources.TryGetValue(GetResourceFullName(name, variant).ToLower(), out Resource resource))
                return resource;

            return null;
        }
        
        public Asset GetAsset(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return null;

            if (Assets.TryGetValue(guid, out Asset asset))
                return asset;

            return null;
        }
        
        public Resource[] GetResources()
        {
            return Resources.Values.ToArray();
        }
        
        public Asset[] GetAssets()
        {
            return Assets.Values.ToArray();
        }
        
        public string GetResourceFullName(string name,string variant)
        {
            return string.IsNullOrEmpty(variant) ? name : Utility.String.Format("{0}.{1}", name, variant);
        }
        #endregion
        
        #region 查询
        public bool IsValidResourceName(string name,string variant)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            
            if (!ResourceNameRegex.IsMatch(name))
                return false;
            
            if (!string.IsNullOrEmpty(variant) && !ResourceVariantRegex.IsMatch(variant))
                return false;

            return true;
        }
        
        public bool IsAvailableResourceName(string name,string variant,Resource resource)
        {
            Resource foundResource = GetResource(name, variant);
            
            //有相同的资源
            //缓存已有同名资源
            if (foundResource != null && foundResource != resource)
                return false;

            string[] foundPathNames = name.Split('/');
            foreach(Resource current in Resources.Values)
            {
                //缓存已有同地址资源
                if (resource != null && resource == current)
                    continue;
                
                //资源名相同，则必须有变体名
                //资源名字相同，变体不相同
                if(current.Name == name)
                {
                    if (current.Variant == null && variant != null)
                        return false;

                    if (current.Variant != null && variant == null)
                        return false;
                }

                //资源不允许有包含关系
                //资源名包含输入名，且资源名是上级名
                if (current.Name.Length > name.Length
                    && current.Name.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) == 0
                    && current.Name[name.Length] == '/')
                    return false;

                //输入名包含资源名，且输入名是上级名
                if (name.Length > current.Name.Length
                    && name.IndexOf(current.Name, StringComparison.CurrentCulture) == 0
                    && name[current.Name.Length] == '/')
                    return false;

                //资源名不允许有大小写差别
                string[] pathNames = current.Name.Split('/');
                string foundName,  pathName;
                for(int i = 0;i < foundPathNames.Length && i < pathNames.Length;i++)
                {
                    foundName = foundPathNames[i];
                    pathName = pathNames[i];
                    if (foundName.ToLower() != pathName.ToLower())
                        break;

                    if (foundName != pathName)
                        return false;
                }
            }

            return true;
        }
        #endregion
        
        #region 设置
        public bool AddResource(string name,string variant,string fileSystem,LoadType loadType,bool packed,string[] resourceGroups = null)
        {
            if (!IsValidResourceName(name, variant))
                return false;

            if (!IsAvailableResourceName(name, variant, null))
                return false;

            if (!string.IsNullOrEmpty(fileSystem) && !ResourceNameRegex.IsMatch(fileSystem))
                return false;

            Resource resource = Resource.Create(name, variant, fileSystem, loadType, packed, resourceGroups);
            Resources.Add(resource.FullName.ToLower(),resource);
            return true;
        }
        
        public bool RemoveResource(string name,string variant)
        {
            if (!IsValidResourceName(name, variant))
                return false;

            Resource resource = GetResource(name, variant);
            if (resource == null)
                return false;

            Asset[] assets = resource.GetAssets();
            resource.Clear();
            Resources.Remove(resource.FullName.ToLower());
            foreach(Asset asset in assets)
            {
                Assets.Remove(asset.Guid);
            }
            return true;
        }
        
        public bool AddAsset(string guid,string name,string variant)
        {
            if (string.IsNullOrEmpty(guid))
                return false;
                
            
            if (!IsValidResourceName(name, variant))
                return false;

            Resource resource = GetResource(name, variant);
            if (resource == null)
                return false;

            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(assetPath))
                return false;

            
            Asset[] resourceAssets = resource.GetAssets();
            foreach(Asset resourceAsset in resourceAssets)
            {
                if(resourceAsset.Name == assetPath)
                    continue;

                if (resourceAsset.Name.ToLower() == assetPath.ToLower())
                    return false;
            }

            //资源类型不一致
            bool isScene = assetPath.EndsWith(SceneExtension, StringComparison.Ordinal);
            if (isScene && resource.AssetType == AssetType.ASSET || !isScene && resource.AssetType == AssetType.SCENE)
                return false;

            Asset asset = GetAsset(guid);
            if (asset == null)
                asset = Asset.Create(guid);
            
            Assets.Add(guid,asset);
            resource.AddAsset(asset);
            
            return true;
        }
        
        public bool RemoveAsset(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return false;

            Asset asset = GetAsset(guid);
            if(asset != null)
            {
                asset.Resource.RemoveAsset(asset);
                Assets.Remove(guid);
            }
            return true;
        }
        #endregion
        
        #region 加载
        public bool Load()
        {
            Clear();
            if(!File.Exists(Utility.Path.ResourceCollectionConfigFilePath))
            {
                return false;
            }
            
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Utility.Path.ResourceCollectionConfigFilePath);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("ResourceCollection");
                XmlNode xmlResource = xmlRoot.SelectSingleNode("Resources");
                XmlNode xmlAsset = xmlRoot.SelectSingleNode("Assets");

                XmlNodeList xmlNodeList = xmlResource.ChildNodes;
                XmlNode xmlNode = null;
                int count = xmlNodeList.Count;
                
                for(int i = 0;i < count;i++)
                {
                    if (OnLoadingResource != null)
                        OnLoadingResource(i, count);

                    xmlNode = xmlNodeList.Item(i);
                    if(xmlNode.Name != "Resource")
                        continue;

                    string name = xmlNode.Attributes.GetNamedItem("Name").Value;
                    string variant = xmlNode.Attributes.GetNamedItem("Variant") != null
                        ? xmlNode.Attributes.GetNamedItem("Variant").Value
                        : null;
                    
                    string fileSystem = xmlNode.Attributes.GetNamedItem("FileSystem") != null
                        ? xmlNode.Attributes.GetNamedItem("FileSystem").Value
                        : null;
                    
                    byte loadType = 0;
                    if (xmlNode.Attributes.GetNamedItem("LoadType") != null)
                        byte.TryParse(xmlNode.Attributes.GetNamedItem("LoadType").Value, out loadType);
                    
                    bool packed = false;
                    if (xmlNode.Attributes.GetNamedItem("Packed") != null)
                        bool.TryParse(xmlNode.Attributes.GetNamedItem("Packed").Value, out packed);

                    string[] resourceGroups = null;
                    if (xmlNode.Attributes.GetNamedItem("ResourceGroups") != null)
                        resourceGroups = xmlNode.Attributes.GetNamedItem("ResourceGroups").Value.Split(',');
                    
                    if(!AddResource(name,variant,fileSystem,(LoadType)loadType,packed,resourceGroups))
                    {
                        CSDebug.LogWarningFormat("无法添加的Resource：{0}",GetResourceFullName(name,variant));
                        continue;
                    }
                }
                
                xmlNodeList = xmlAsset.ChildNodes;
                xmlNode = null;
                count = xmlNodeList.Count;
                
                for(int i = 0;i < count;i++)
                {
                    if (OnLoadingAsset != null)
                        OnLoadingAsset(i, count);

                    xmlNode = xmlNodeList.Item(i);
                    if (xmlNode.Name != "Asset")
                        continue;
                        
                    string guid = xmlNode.Attributes.GetNamedItem("GUID").Value;
                    string resourceName = xmlNode.Attributes.GetNamedItem("ResourceName").Value;
                    string varaint = xmlNode.Attributes.GetNamedItem("ResourceVariant") != null ? xmlNode.Attributes.GetNamedItem("ResourceVariant").Value : null;
                    
                    if(!AddAsset(guid,resourceName,varaint))
                    {
                        CSDebug.LogWarningFormat("无法添加的Asset：{0}",resourceName);
                    }
                }

                if (OnLoadCompleted != null)
                    OnLoadCompleted();

                return true;
            }
            catch
            {
                File.Delete(Utility.Path.ResourceCollectionConfigFilePath);
                if (OnLoadCompleted != null)
                    OnLoadCompleted();
                return false;
            }
            
            return true;
        }
        #endregion
        
        #region 保存
        public bool Save()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDocument.AppendChild(xmlDeclaration);

                XmlElement xmlRoot = xmlDocument.CreateElement("ResourceCollection");
                xmlDocument.AppendChild(xmlRoot);

                XmlElement xmlResource = xmlDocument.CreateElement("Resources");
                xmlRoot.AppendChild(xmlResource);

                XmlElement xmlAsset = xmlDocument.CreateElement("Assets");
                xmlRoot.AppendChild(xmlAsset);

                XmlElement xmlElement = null;
                XmlAttribute xmlAttribute = null;
                foreach(Resource resource in Resources.Values)
                {
                    xmlElement = xmlDocument.CreateElement("Resource");
                    
                    xmlAttribute = xmlDocument.CreateAttribute("Name");
                    xmlAttribute.Value = resource.Name;
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    
                    if(resource.Variant != null)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("Variant");
                        xmlAttribute.Value = resource.Variant;
                        xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    }
                    
                    if(resource.FileSystem != null)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("FileSystem");
                        xmlAttribute.Value = resource.FileSystem;
                        xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    }
                    
                    xmlAttribute = xmlDocument.CreateAttribute("LoadType");
                    xmlAttribute.Value = ((byte)resource.LoadType).ToString();
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    
                    xmlAttribute = xmlDocument.CreateAttribute("Packed");
                    xmlAttribute.Value = resource.Packed.ToString();
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);

                    string[] resourceGroups = resource.GetResourceGroups();
                    if(resourceGroups.Length > 0)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("ResourceGroups");
                        xmlAttribute.Value = string.Join(",",resourceGroups);
                        xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    }
                    xmlResource.AppendChild(xmlElement);
                }
                
                foreach(Asset asset in Assets.Values)
                {
                    xmlElement = xmlDocument.CreateElement("Asset");
                    
                    xmlAttribute = xmlDocument.CreateAttribute("GUID");
                    xmlAttribute.Value = asset.Guid;
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    
                    xmlAttribute = xmlDocument.CreateAttribute("ResourceName");
                    xmlAttribute.Value = asset.Resource.Name;
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    
                    if(!string.IsNullOrEmpty(asset.Resource.Variant))
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("ResourceVariant");
                        xmlAttribute.Value = asset.Resource.Variant;
                        xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    }

                    xmlAsset.AppendChild(xmlElement);
                }

                string collectionConfigDirectoryName = Path.GetDirectoryName(Utility.Path.ResourceCollectionConfigFilePath); 
                if(!Directory.Exists(collectionConfigDirectoryName))
                {
                    Directory.CreateDirectory(collectionConfigDirectoryName);
                }
                
                xmlDocument.Save(Utility.Path.ResourceCollectionConfigFilePath);
                AssetDatabase.Refresh();
                return true;
            }
            catch
            {
                if(File.Exists(Utility.Path.ResourceCollectionConfigFilePath))
                {
                    File.Delete(Utility.Path.ResourceCollectionConfigFilePath);
                }

                return false;
            }
        }
        #endregion
        
        #region 清理
        public void Clear()
        {
            Resources.Clear();
            Assets.Clear();
        }
        #endregion
    }
}