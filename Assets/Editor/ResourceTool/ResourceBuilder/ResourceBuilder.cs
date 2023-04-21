using ResourceTool.Base;
using UnityEditor;
using UnityEngine;

namespace ResourceTool
{
    [ResourcePage(1,"资源创建工具")]
    public class ResourceBuilder : ResourceToolBase
    {
        #region 数据
        private ResourceBuilderController resourceBuilderController = null;
        #endregion
        
        public override void Init()
        {
            base.Init();
            resourceBuilderController = new ResourceBuilderController();
        }

        public override void OnRefresh()
        {
            
        }

        public override void OnToolGUI()
        {
            EditorGUILayout.Space(EditorUtil.CommonSpace);
            EnvironmentInfoOnGUI();
            EditorGUILayout.Space(EditorUtil.CommonSpace);
            AssetBundleSettingOnGUI();
            EditorGUILayout.Space(EditorUtil.CommonSpace);
            
        }
        
        private void EnvironmentInfoOnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("环境信息",EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("工程名称",GUILayout.Width(160f));
                        EditorGUILayout.LabelField(PlayerSettings.productName);
                    }
                    EditorGUILayout.EndHorizontal();
            
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("公司名",GUILayout.Width(160f));
                        EditorGUILayout.LabelField(PlayerSettings.companyName);
                    }
                    EditorGUILayout.EndHorizontal();
            
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("构建应用程序标识符",GUILayout.Width(160f));
                        EditorGUILayout.LabelField(PlayerSettings.applicationIdentifier);
                    }
                    EditorGUILayout.EndHorizontal();
            
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Unity版本",GUILayout.Width(160f));
                        EditorGUILayout.LabelField(Application.unityVersion);
                    }
                    EditorGUILayout.EndHorizontal();
            
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("构建应用版本",GUILayout.Width(160f));
                        EditorGUILayout.LabelField(Application.version);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        
        private void AssetBundleSettingOnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField("平台",EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal("box");
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            DrawPlatform(Platform.Windows,"Windows");
                            DrawPlatform(Platform.Windows64,"Windows x64");
                            DrawPlatform(Platform.MacOS,"MacOS");
                            DrawPlatform(Platform.Linux,"Linux");
                            DrawPlatform(Platform.IOS,"IOS");
                            DrawPlatform(Platform.Android,"Android");
                            DrawPlatform(Platform.WindowsStore,"Windows Store");
                            DrawPlatform(Platform.WebGL,"WebGL");
                        }
                        EditorGUILayout.EndVertical();
                        
                        EditorGUILayout.BeginVertical();
                        {
                            DrawPlatform(Platform.WindowsStore,"Windows Store");
                            DrawPlatform(Platform.WebGL,"WebGL");
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginVertical("box");
                    {
                        resourceBuilderController.ZipSelected = EditorGUILayout.ToggleLeft("Zip All Resources",resourceBuilderController.ZipSelected);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField("AssetBundle 选项",EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal("box");
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            bool uncompressedAssetBundleSelected = EditorGUILayout.ToggleLeft(
                                "Uncompressed AssetBundle", resourceBuilderController.UncompressedAssetBundleSelected);
                            if(resourceBuilderController.UncompressedAssetBundleSelected != uncompressedAssetBundleSelected)
                            {
                                resourceBuilderController.UncompressedAssetBundleSelected = uncompressedAssetBundleSelected;
                                if (uncompressedAssetBundleSelected)
                                    resourceBuilderController.ChunkBasedCompressionSelected = false;
                            }

                            bool disableWriteTypeTreeSelected = EditorGUILayout.ToggleLeft("Disable Write Type Tree",
                                resourceBuilderController.DisableWriteTypeTreeSelected);
                            if(resourceBuilderController.DisableWriteTypeTreeSelected != disableWriteTypeTreeSelected)
                            {
                                resourceBuilderController.DisableWriteTypeTreeSelected = disableWriteTypeTreeSelected;
                                if (disableWriteTypeTreeSelected)
                                    resourceBuilderController.IgnoreTypeTreeChangesSelected = false;
                            }
                            
                            resourceBuilderController.DeterministicAssetBundleSelected = EditorGUILayout.ToggleLeft(
                                "Deterministic AssetBundle", resourceBuilderController.DeterministicAssetBundleSelected);
                            
                            
                            resourceBuilderController.ForceRebuildAssetBundleSelected = EditorGUILayout.ToggleLeft(
                                "ForceRebuild AssetBundle", resourceBuilderController.ForceRebuildAssetBundleSelected);
                            
                            bool ignoreTypeTreeChangesSelected = EditorGUILayout.ToggleLeft("Ignore Type Tree Changes",
                                resourceBuilderController.IgnoreTypeTreeChangesSelected);
                            if(resourceBuilderController.IgnoreTypeTreeChangesSelected != ignoreTypeTreeChangesSelected)
                            {
                                resourceBuilderController.IgnoreTypeTreeChangesSelected = ignoreTypeTreeChangesSelected;
                                if (ignoreTypeTreeChangesSelected)
                                    resourceBuilderController.DisableWriteTypeTreeSelected = false;
                            }
                            
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                resourceBuilderController.AppendHashToAssetBundleNameSelected = EditorGUILayout.ToggleLeft(
                                    "Append Hash To AssetBundle Name", resourceBuilderController.AppendHashToAssetBundleNameSelected);
                            }
                            EditorGUI.EndDisabledGroup();
                            
                            bool chunkBasedCompressionSelected = EditorGUILayout.ToggleLeft(
                                "Chunk Based Compression", resourceBuilderController.ChunkBasedCompressionSelected);
                            if(resourceBuilderController.ChunkBasedCompressionSelected != chunkBasedCompressionSelected)
                            {
                                resourceBuilderController.ChunkBasedCompressionSelected = chunkBasedCompressionSelected;
                                if (chunkBasedCompressionSelected)
                                    resourceBuilderController.UncompressedAssetBundleSelected = false;
                            }
                            
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                resourceBuilderController.StrictModeSelected = EditorGUILayout.ToggleLeft(
                                    "Strict Mode", resourceBuilderController.StrictModeSelected);
                                
                                resourceBuilderController.DryRunBuildSelected = EditorGUILayout.ToggleLeft(
                                    "Dry Run Build", resourceBuilderController.DryRunBuildSelected);
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical();
                        {
                            resourceBuilderController.DisableLoadAssetByFileNameSelected = EditorGUILayout.ToggleLeft(
                                "Disable Load Asset By File Name", resourceBuilderController.DisableLoadAssetByFileNameSelected);
                            
                            resourceBuilderController.DisableLoadAssetByFileNameWithExtensionSelected = EditorGUILayout.ToggleLeft(
                                "Disable Load Asset By FileName With Extension", resourceBuilderController.DisableLoadAssetByFileNameWithExtensionSelected);
                            
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                resourceBuilderController.AssetBundleStripUnityVersionSelected = EditorGUILayout.ToggleLeft(
                                    "AssetBundle Strip Unity Version", resourceBuilderController.AssetBundleStripUnityVersionSelected);
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        
        private void BuildOnGUI()
        {
            EditorGUILayout.LabelField("构建");
            EditorGUILayout.BeginVertical("box");
            {
                
            }
            EditorGUILayout.EndVertical();
        }
        
        private void DrawPlatform(Platform platform,string platformName)
        {
            resourceBuilderController.SelectPlatform(platform,EditorGUILayout.ToggleLeft(platformName,resourceBuilderController.PlatformIsSelected(platform)));
        }

        public override void OnClose()
        {
        }
    }
}