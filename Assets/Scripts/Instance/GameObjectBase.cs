namespace Instance
{
    using UnityEngine;

    public class GameObjectBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T mInstance = default(T);
        public static T Instance
        {
            get
            {
                return mInstance;
            }
        }

        private static Transform mCahcheTrans = null;
        public static UnityEngine.Transform CahcheTrans
        {
            get { return mCahcheTrans; }
        }

        protected virtual bool IsDonotDestroy
        {
            get
            {
                return false;
            }
        }

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            if (IsDonotDestroy)
            {
                if (CahcheTrans == null)
                {
                    if (CSDebug.LogSwitch)
                    {
                        CSDebug.LogError(name);
                    }
                }
            }
        }

        public static void CreateInstance(Transform parent)
        {
            if (mInstance != default(T))
            {
                return;
            }

            GameObject go = new GameObject(typeof(T).ToString());
            if (parent != null)
            {
                go.transform.parent = parent;
            }
            mInstance = go.AddComponent<T>();
            mCahcheTrans = go.transform;
        }

        public virtual void Destroy()
        {
            if (CahcheTrans != null)
            {
                if (!IsDonotDestroy)
                {
                    UnityEngine.Object.Destroy(CahcheTrans.gameObject);
                    mCahcheTrans = null;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            mInstance = default(T);
            mCahcheTrans = null;
        }
    }
}