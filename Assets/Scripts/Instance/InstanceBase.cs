using System;
public class InstanceBase<T>: IDisposable where T :class
{
    private static T instance;
    private static readonly object syncObject = new object();
    public static T Instance {
        get {
            if (instance == null)
            {
                lock (syncObject)
                {
                    if (instance == null)
                    {
                        instance = CreateInstance<T>();
                    }
                }
            }
            return instance;
        }
    }

    public static T CreateInstance<T>() {
        return (T)Activator.CreateInstance(typeof(T), true);
    }

    public virtual void Dispose() { }
}