using Framework;
using ResourceTool.Base;
using UnityEditor;
using UnityEngine;

namespace ResourceTool
{
    [ResourcePage(2,"资源同步工具")]
    public class ResourceSync : ResourceToolBase
    {
        #region 数据
        public ResourceSyncController controller = null;
        #endregion
        
        public override void Init()
        {
            base.Init();
            controller = new ResourceSyncController();
            controller.OnLoadingResource += OnLoadingResource;
            controller.OnLoadingAsset += OnLoadingAsset;
            controller.OnResourceDataChange += OnResourceDataChange;
            controller.OnCompleted += OnCompleted;
        }

        public override void OnToolGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(EditorUtil.ButtonSpace);
            if(GUILayout.Button("移除所有Asset Bundle配置",GUILayout.Height(EditorUtil.ButtonHegiht)))
            {
                if (!controller.RemoveAllAssetBundleNames())
                    CSDebug.LogError("移出所有AssetBundle名字出错");
                else
                    CSDebug.Log("成功移出所有AssetBundle名字");
                AssetDatabase.Refresh();
            }
            
            GUILayout.Space(EditorUtil.ButtonSpace);
            if(GUILayout.Button("ResourceCollection.xml同步到工程",GUILayout.Height(EditorUtil.ButtonHegiht)))
            {
                if(!controller.ResourceCollectionXmlToProject())
                    CSDebug.LogError("AssetBudnle数据同步到工程出错");
                else
                    CSDebug.Log("成功将AssetBundle数据同步到工程");
                AssetDatabase.Refresh();
            }

            GUILayout.Space(EditorUtil.ButtonSpace);
            if(GUILayout.Button("工程Asset Bundle配置同步到ResourceCollection.xml",GUILayout.Height(EditorUtil.ButtonHegiht)))
            {
                if(!controller.ResourceCollectionXmlFromProject())
                    CSDebug.LogError("保存工程AssetBundle数据出错");
                else
                    CSDebug.Log("成功保存工程AssetBundle数据");
                AssetDatabase.Refresh();
            }
            GUILayout.EndVertical();
        }
        
        #region 回调
        private void OnLoadingResource(int index,int totalCount)
        {
            EditorUtility.DisplayProgressBar("Loading Resources",Utility.String.Format("Loading Resource,{0}/{1} loading...",index.ToString(),totalCount.ToString()),(float)index / totalCount);
        }
        
        private void OnLoadingAsset(int index,int totalCount)
        {
            EditorUtility.DisplayProgressBar("Loading Assets",Utility.String.Format("Loading Asset,{0}/{1} loading...",index.ToString(),totalCount.ToString()),(float)index / totalCount);
        }
        
        private void OnResourceDataChange(int index,int totalCount,string name)
        {
            EditorUtility.DisplayProgressBar("Analysis Assets",Utility.String.Format("{1}/{2} : {0} loading...",name,index.ToString(),totalCount.ToString()),(float)index / totalCount);
        }
        
        private void OnCompleted()
        {
            EditorUtility.ClearProgressBar();
        }
        #endregion

        public override void OnClose()
        {
        }
    }
}