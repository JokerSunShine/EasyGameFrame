using System;
using UnityEngine;

namespace Packages.FW_Common.Other
{
    public class DisposeOnDestroy:MonoBehaviour
    {
        public IDisposable disposable;
        
        public static void Add(GameObject gameObject,IDisposable request)
        {
            if (gameObject == null)
                return;
            var component = gameObject.AddComponent<DisposeOnDestroy>();
            component.disposable = request;
        }

        private void OnDestroy()
        {
            if(disposable != null)
            {
                disposable.Dispose();
                disposable = null;
            }
        }
    }
}