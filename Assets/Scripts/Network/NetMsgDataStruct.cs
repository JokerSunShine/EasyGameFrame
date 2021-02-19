using System;

public class NetMsgDataStruct
{
     public static Type GetNetMsgType(int msgId)
     {
          Type type = null;
          NetMsgDataStructDicCache.MsgDataStructDic.TryGetValue(msgId, out type);
          return type;
     }
}