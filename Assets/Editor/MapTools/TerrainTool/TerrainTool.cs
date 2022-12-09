using System;
using MapTools.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapTools.TerrainTool
{
    [PageAttribute(1,"地形工具")]
    public class TerrainTool : MapToolBase
    {
        #region 数据
        private string[] sceneName;
        private int chooseIndex;
        public int ChooseIndex
        {
            get
            {
                return chooseIndex;
            }
            set
            {
                if(value != chooseIndex)
                {
                    MapToolUtility.OpenScene(sceneName[value], SceneResType.ArtScene);
                    RefreshSceneResouerce();
                }
                chooseIndex = value;
            }
        }
        #endregion
        
        #region 初始化
        public override void Init()
        {
            base.Init();
            sceneName = MapToolUtility.GetSceneNames();
            chooseIndex = 0;
        }
        #endregion
        
        #region 刷新
        public override void OnRefresh()
        {
            
        }
        
        private void RefreshSceneResouerce()
        {
            
        }
        #endregion
        
        #region GUI
        public override void OnToolGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                ChooseIndex = EditorGUILayout.Popup("Scene", ChooseIndex, sceneName, GUILayout.Width(250));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        #endregion
        
        #region Destroy
        public override void OnClose()
        {
        }
        #endregion
    }
}