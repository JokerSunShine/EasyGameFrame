using System;
using System.IO;
public class ProtoBufManager:I_ProtoBufManager
{
    public static void Serializer<T>(Stream destination, T instance)
    {
#if UNITY_EDITOR
        ProtoBuf.Serializer.Serialize(destination,instance);
#endif
    }
    
    public static object Deserializer(byte[] buffs,Type type,int msgId)
    {
        object obj = null;
        if(type != null)
        {
#if UNITY_EDITOR
            MemoryStream stream = new MemoryStream(buffs);
            obj = ProtoBuf.Serializer.Deserialize<Type>(stream);    
#endif
        }
        return obj;
    }
}