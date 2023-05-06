using System;
using _3DMath;
using Framework;
using ResourceTool.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace ResourceTool
{
    [ResourcePage(1,"资源创建工具")]
    public class ResourceBuilder : ResourceToolBase
    {
        #region 数据
        private ResourceBuilderController m_ResourceBuilderController = null;
        private int m_BuildEventHandlerTypeNameIndex = 0;
        #endregion
        
        #region 初始化
        public override void Init()
        {
            base.Init();
            m_ResourceBuilderController = new ResourceBuilderController();
        }
        #endregion

        #region 刷新
        public override void OnRefresh()
        {
            if(m_ResourceBuilderController.LoadConfig())
            {
                CSDebug.Log("加载配置成功");
                m_BuildEventHandlerTypeNameIndex = m_ResourceBuilderController.GetBuildEventHandlerIndex();
                m_ResourceBuilderController.SetBuildEventHandlerByIndex(m_BuildEventHandlerTypeNameIndex);
            }
            else
                CSDebug.LogWarning("配置加载失败");
        }
        #endregion

        #region GUI
         public override void OnToolGUI()
        {
            EditorGUILayout.Space(EditorUtil.CommonSpace);
            EnvironmentInfoOnGUI();
            EditorGUILayout.Space(EditorUtil.CommonSpace);
            AssetBundleSettingOnGUI();
            EditorGUILayout.Space(EditorUtil.CommonSpace);
            BuildOnGUI();
            EditorGUILayout.Space(EditorUtil.CommonSpace);
            StartBuildOnGUI();
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
                        m_ResourceBuilderController.ZipSelected = EditorGUILayout.ToggleLeft("Zip All Resources",m_ResourceBuilderController.ZipSelected);
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
                                "Uncompressed AssetBundle", m_ResourceBuilderController.UncompressedAssetBundleSelected);
                            if(m_ResourceBuilderController.UncompressedAssetBundleSelected != uncompressedAssetBundleSelected)
                            {
                                m_ResourceBuilderController.UncompressedAssetBundleSelected = uncompressedAssetBundleSelected;
                                if (uncompressedAssetBundleSelected)
                                    m_ResourceBuilderController.ChunkBasedCompressionSelected = false;
                            }

                            bool disableWriteTypeTreeSelected = EditorGUILayout.ToggleLeft("Disable Write Type Tree",
                                m_ResourceBuilderController.DisableWriteTypeTreeSelected);
                            if(m_ResourceBuilderController.DisableWriteTypeTreeSelected != disableWriteTypeTreeSelected)
                            {
                                m_ResourceBuilderController.DisableWriteTypeTreeSelected = disableWriteTypeTreeSelected;
                                if (disableWriteTypeTreeSelected)
                                    m_ResourceBuilderController.IgnoreTypeTreeChangesSelected = false;
                            }
                            
                            m_ResourceBuilderController.DeterministicAssetBundleSelected = EditorGUILayout.ToggleLeft(
                                "Deterministic AssetBundle", m_ResourceBuilderController.DeterministicAssetBundleSelected);
                            
                            
                            m_ResourceBuilderController.ForceRebuildAssetBundleSelected = EditorGUILayout.ToggleLeft(
                                "ForceRebuild AssetBundle", m_ResourceBuilderController.ForceRebuildAssetBundleSelected);
                            
                            bool ignoreTypeTreeChangesSelected = EditorGUILayout.ToggleLeft("Ignore Type Tree Changes",
                                m_ResourceBuilderController.IgnoreTypeTreeChangesSelected);
                            if(m_ResourceBuilderController.IgnoreTypeTreeChangesSelected != ignoreTypeTreeChangesSelected)
                            {
                                m_ResourceBuilderController.IgnoreTypeTreeChangesSelected = ignoreTypeTreeChangesSelected;
                                if (ignoreTypeTreeChangesSelected)
                                    m_ResourceBuilderController.DisableWriteTypeTreeSelected = false;
                            }
                            
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                m_ResourceBuilderController.AppendHashToAssetBundleNameSelected = EditorGUILayout.ToggleLeft(
                                    "Append Hash To AssetBundle Name", m_ResourceBuilderController.AppendHashToAssetBundleNameSelected);
                            }
                            EditorGUI.EndDisabledGroup();
                            
                            bool chunkBasedCompressionSelected = EditorGUILayout.ToggleLeft(
                                "Chunk Based Compression", m_ResourceBuilderController.ChunkBasedCompressionSelected);
                            if(m_ResourceBuilderController.ChunkBasedCompressionSelected != chunkBasedCompressionSelected)
                            {
                                m_ResourceBuilderController.ChunkBasedCompressionSelected = chunkBasedCompressionSelected;
                                if (chunkBasedCompressionSelected)
                                    m_ResourceBuilderController.UncompressedAssetBundleSelected = false;
                            }
                            
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                m_ResourceBuilderController.StrictModeSelected = EditorGUILayout.ToggleLeft(
                                    "Strict Mode", m_ResourceBuilderController.StrictModeSelected);
                                
                                m_ResourceBuilderController.DryRunBuildSelected = EditorGUILayout.ToggleLeft(
                                    "Dry Run Build", m_ResourceBuilderController.DryRunBuildSelected);
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical();
                        {
                            m_ResourceBuilderController.DisableLoadAssetByFileNameSelected = EditorGUILayout.ToggleLeft(
                                "Disable Load Asset By File Name", m_ResourceBuilderController.DisableLoadAssetByFileNameSelected);
                            
                            m_ResourceBuilderController.DisableLoadAssetByFileNameWithExtensionSelected = EditorGUILayout.ToggleLeft(
                                "Disable Load Asset By FileName With Extension", m_ResourceBuilderController.DisableLoadAssetByFileNameWithExtensionSelected);
                            
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                m_ResourceBuilderController.AssetBundleStripUnityVersionSelected = EditorGUILayout.ToggleLeft(
                                    "AssetBundle Strip Unity Version", m_ResourceBuilderController.AssetBundleStripUnityVersionSelected);
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
            EditorGUILayout.LabelField("构建",EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("选择构建事件接口",GUILayout.Width(160f));
                    string[] names = m_ResourceBuilderController.GetBuildEventHandlerTypeNames();
                    int selectedIndex = EditorGUILayout.Popup(m_BuildEventHandlerTypeNameIndex, names);
                    if(selectedIndex != m_BuildEventHandlerTypeNameIndex)
                    {
                        m_BuildEventHandlerTypeNameIndex = selectedIndex;
                        string BuildEventHandlerTypeName = selectedIndex <= 0 ? String.Empty : names[selectedIndex];
                        if(m_ResourceBuilderController.SetBuildEventHandlerByName(BuildEventHandlerTypeName))
                            CSDebug.Log("设置构建事件成功");
                        else
                            CSDebug.LogWarning("设置构建事件失败");
                    }
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("内部资源版本",GUILayout.Width(160f));
                    m_ResourceBuilderController.InternalResourceVersion =
                        EditorGUILayout.IntField(m_ResourceBuilderController.InternalResourceVersion);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("资源版本",GUILayout.Width(160f));
                    GUILayout.Label(Utility.String.Format("{0} ({1})", m_ResourceBuilderController.ApplicationGameVersion, m_ResourceBuilderController.InternalResourceVersion.ToString()));
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("输出根目录",GUILayout.Width(160f));
                    m_ResourceBuilderController.OutputDirectory =
                        EditorGUILayout.TextField(m_ResourceBuilderController.OutputDirectory);
                    if(GUILayout.Button("选择路径...",GUILayout.Width(100f)))
                    {
                        string directory = EditorUtility.OpenFolderPanel("选择根目录",
                            m_ResourceBuilderController.OutputDirectory, string.Empty);
                        if (!string.IsNullOrEmpty(directory))
                            m_ResourceBuilderController.OutputDirectory = directory;
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Working Path",GUILayout.Width(160f));
                    GUILayout.Label(m_ResourceBuilderController.WorkingPath);

                    DrawOpenPath(m_ResourceBuilderController.WorkingPath);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Package Path",GUILayout.Width(160f));
                    GUILayout.Label(m_ResourceBuilderController.PackagePath);
                    
                    DrawOpenPath(m_ResourceBuilderController.PackagePath);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Full Path",GUILayout.Width(160f));
                    GUILayout.Label(m_ResourceBuilderController.FullPath);
                    
                    DrawOpenPath(m_ResourceBuilderController.FullPath);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Packed Path",GUILayout.Width(160f));
                    GUILayout.Label(m_ResourceBuilderController.PackedPath);
                    
                    DrawOpenPath(m_ResourceBuilderController.PackedPath);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Build Report Path",GUILayout.Width(160f));
                    GUILayout.Label(m_ResourceBuilderController.BuildReportPath);
                    
                    DrawOpenPath(m_ResourceBuilderController.BuildReportPath);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        
        private void StartBuildOnGUI()
        {
            string buildMessage = string.Empty;
            MessageType buildMessageType = MessageType.None;
            GetBuildMessage(out buildMessage,out buildMessageType);
            EditorGUILayout.HelpBox(buildMessage,buildMessageType);
            
            EditorGUILayout.Space(EditorUtil.CommonSpace);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(m_ResourceBuilderController.Platforms == Platform.Undefined || !m_ResourceBuilderController.IsValidOutputDirectory);
                {
                    if(GUILayout.Button("开始构建资源"))
                    {
                        
                    }
                }
                EditorGUI.EndDisabledGroup();
                if(GUILayout.Button("保存配置",GUILayout.Width(80f)))
                {
                    SaveConfig();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        
        
        private void DrawPlatform(Platform platform,string platformName)
        {
            m_ResourceBuilderController.SelectPlatform(platform,EditorGUILayout.ToggleLeft(platformName,m_ResourceBuilderController.PlatformIsSelected(platform)));
        }
        
        private void DrawOpenPath(string path)
        {
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(path) || !Directory.Exists(path));
            {
                if (GUILayout.Button("打开目录",GUILayout.Width(100)))
                    Utility.Folder.Execute(path);
            }
            EditorGUI.EndDisabledGroup();
        }
        #endregion
        
        #region 获取
        private void GetBuildMessage(out string buildMessage,out MessageType buildMessageType)
        {
            if(m_ResourceBuilderController.Platforms == Platform.Undefined)
            {
                buildMessage = "未选择平台";
                buildMessageType = MessageType.Error;
                return;
            }
            if(!m_ResourceBuilderController.IsValidOutputDirectory)
            {
                buildMessage = "输出地址无效";
                buildMessageType = MessageType.Error;
                return;
            }
            
            buildMessage = string.Empty;
            buildMessageType = MessageType.Info;
            if(Directory.Exists(m_ResourceBuilderController.PackagePath))
            {
                buildMessage += Utility.String.Format("{0}将被覆盖写入", m_ResourceBuilderController.PackagePath);
                buildMessageType = MessageType.Warning;
            }
            
            if (Directory.Exists(m_ResourceBuilderController.FullPath))
            {
                if (buildMessage.Length > 0)
                {
                    buildMessage += " ";
                }

                buildMessage += Utility.String.Format("{0}将被覆盖写入", m_ResourceBuilderController.FullPath);
                buildMessageType = MessageType.Warning;
            }
            
            if (Directory.Exists(m_ResourceBuilderController.PackedPath))
            {
                if (buildMessage.Length > 0)
                {
                    buildMessage += " ";
                }

                buildMessage += Utility.String.Format("{0}将被覆盖写入", m_ResourceBuilderController.PackedPath);
                buildMessageType = MessageType.Warning;
            }
            
            if(buildMessageType == MessageType.Warning)
            {
                return;
            }
            
            buildMessage = "准备就绪";
        }
        #endregion
       
        #region 配置数据保存
        private void SaveConfig()
        {
            if(m_ResourceBuilderController.SaveConfig())
                CSDebug.Log("成功保存数据");
            else
                CSDebug.LogError("数据保存失败");
        }
        #endregion

        #region 关闭
        public override void OnClose()
        {
        }
        #endregion

    }
}