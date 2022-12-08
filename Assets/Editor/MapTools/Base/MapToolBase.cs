using UnityEditor;

namespace MapTools.Base
{
    public class MapToolBase : EditorWindow
    {
        public virtual void Init(){}
        public virtual void OnSelect(){}
        public virtual void OnDrawGUI(){}
        public virtual void OnClose(){}
    }
}