using ResourceTool.Base;
using UnityEngine;

namespace ResourceTool
{
    [ResourcePage(2,"资源同步工具")]
    public class ResourceSync : ResourceToolBase
    {
        public override void Init()
        {
            base.Init();
        }

        public override void OnRefresh()
        {
            
        }

        public override void OnToolGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(EditorUtil.ButtonSpace);
            if(GUILayout.Button("移除所有Asset Bundle配置",GUILayout.Height(EditorUtil.ButtonHegiht)))
            {
                
            }
            
            GUILayout.Space(EditorUtil.ButtonSpace);
            if(GUILayout.Button("ResourceCollection.xml同步到工程",GUILayout.Height(EditorUtil.ButtonHegiht)))
            {
                
            }

            GUILayout.Space(EditorUtil.ButtonSpace);
            if(GUILayout.Button("工程Asset Bundle配置同步到ResourceCollection.xml",GUILayout.Height(EditorUtil.ButtonHegiht)))
            {
                
            }
            GUILayout.EndVertical();
        }

        public override void OnClose()
        {
        }
    }
}