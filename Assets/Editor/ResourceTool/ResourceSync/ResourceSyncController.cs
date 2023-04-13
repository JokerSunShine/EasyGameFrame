using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace ResourceTool
{
    public class ResourceSyncController
    {
        #region 事件
        public event Action<int, int> OnLoadingResource = null;
        public event Action<int, int> OnLoadingAsset = null;
        public event Action<int, int, string> OnResourceDataChange = null;
        public event Action OnCompleted = null;
        #endregion
        
        #region 获取
        public string[] GetAllAssetBundleNames()
        {
            return AssetDatabase.GetAllAssetBundleNames();
        }
        
        public string[] GetUnUsedAssetBundleNames()
        {
            return AssetDatabase.GetUnusedAssetBundleNames();
        }
        
        public string[] GetUsedAssetBundleNames()
        {
            HashSet<string> hashSet = new HashSet<string>(GetAllAssetBundleNames());
            hashSet.ExceptWith(GetUnUsedAssetBundleNames());
            return hashSet.ToArray();
        }
        
        public string[] GetAssetPathsFromAssetBundle(string assetBundleName)
        {
            return AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
        }
        #endregion
        
        #region 功能
        public bool RemoveAllAssetBundleNames()
        {
            HashSet<string> allAssetNames = new HashSet<string>();
            string[] assetBundleNames = GetUsedAssetBundleNames();
            foreach(string assetBundleName in  assetBundleNames)
            {
                string[] assetNames = GetAssetPathsFromAssetBundle(assetBundleName);
                foreach(string assetName in assetNames)
                {
                    allAssetNames.Add(assetName);
                }
            }

            int assetIndex = 0;
            int assetNameCount = allAssetNames.Count;
            foreach(string assetName in allAssetNames)
            {
                AssetImporter assetImporter = AssetImporter.GetAtPath(assetName);
                if(assetImporter == null)
                {
                    if (OnCompleted != null)
                        OnCompleted();
                    return false;
                }

                assetImporter.assetBundleVariant = null;
                assetImporter.assetBundleName = null;
                assetImporter.SaveAndReimport();

                if (OnResourceDataChange != null)
                    OnResourceDataChange(++assetIndex,assetNameCount,assetName);
            }

            RemoveUnUsedAssetBundleNames();

            if (OnCompleted != null)
                OnCompleted();
            
            return true;
        }
        
        public void RemoveUnUsedAssetBundleNames()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
        
        public bool ResourceCollectionXmlToProject()
        {
            ResourceCollection resourceCollection = new ResourceCollection();
            resourceCollection.OnLoadingResource += (int index, int count) =>
            {
                if (OnLoadingResource != null)
                    OnLoadingResource(index,count);
            };
            resourceCollection.OnLoadingAsset += (int index, int count) =>
            {
                if (OnLoadingAsset != null)
                    OnLoadingAsset(index, count);
            };
            resourceCollection.OnLoadCompleted += ()=>
            {
                if (OnCompleted != null)
                    OnCompleted();
            };
            
            if(!resourceCollection.Load())
            {
                return false;
            }

            int assetIndex = 0, assetCount = 0;
            Resource[] resources = resourceCollection.GetResources();
            foreach(Resource resource in resources)
            {
                Asset[] assets = resource.GetAssets();
                assetIndex = 0;
                assetCount = assets.Length;
                foreach(Asset asset in assets)
                {
                    AssetImporter assetImporter = AssetImporter.GetAtPath(asset.Name);
                    if(assetImporter == null)
                    {
                        if (OnCompleted != null)
                            OnCompleted();

                        return false;
                    }

                    assetImporter.assetBundleName = resource.Name;
                    assetImporter.assetBundleVariant = resource.Variant;
                    assetImporter.SaveAndReimport();

                    if (OnResourceDataChange != null)
                        OnResourceDataChange(assetIndex, assetCount, asset.Name);
                }
            }

            if (OnCompleted != null)
                OnCompleted();
            return true;
        }
        
        public bool ResourceCollectionXmlFromProject()
        {
            ResourceCollection resourceCollection = new ResourceCollection();
            string[] assetBundleNames = GetUsedAssetBundleNames();
            foreach(string assetBundleName in assetBundleNames)
            {
                string name = assetBundleName;
                string variant = null;
                int lastDotPosition = assetBundleName.LastIndexOf('.');
                if(lastDotPosition > 0 && lastDotPosition < assetBundleName.Length - 1)
                {
                    name = assetBundleName.Substring(0, lastDotPosition);
                    variant = assetBundleName.Substring(lastDotPosition + 1);
                }
                
                if(!resourceCollection.AddResource(name,variant,null,LoadType.LoadFromFile,false))
                {
                    return false;
                }

                string[] assetPaths = GetAssetPathsFromAssetBundle(assetBundleName);
                foreach(string assetPath in assetPaths)
                {
                    string guid = AssetDatabase.AssetPathToGUID(assetPath);
                    if (string.IsNullOrEmpty(guid))
                        return false;

                    if (!resourceCollection.AddAsset(guid, name, variant))
                        return false;

                }
            }

            return resourceCollection.Save();
        }
        #endregion
    }
}