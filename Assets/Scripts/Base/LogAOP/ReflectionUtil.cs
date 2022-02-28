using System;
using System.Reflection;

namespace Instance.LogAOP
{
    public class ReflectionUtil
    {
        public static T GetAttribute<T>(MethodInfo method) where T:Attribute
        {
            var attrs = method.GetCustomAttributes(typeof(T), false);
            if(attrs.Length != 0)
            {
                var attribute = attrs[0] as T;
                if(attribute != null)
                {
                    return attribute;
                }
            }

            return null;
        }
    }
}