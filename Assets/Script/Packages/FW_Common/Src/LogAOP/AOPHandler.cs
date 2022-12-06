using System.Reflection;
// using System.Runtime.Remoting.Messaging;

namespace Instance.LogAOP
{
    // public class AOPHandler:IMessageSink
    // {
    //     private readonly IMessageSink _nextSink;
    //     
    //     public AOPHandler(IMessageSink nextSink)
    //     {
    //         _nextSink = nextSink;
    //     }
    //     
    //     public IMessageSink NextSink
    //     {
    //         get
    //         {
    //             return _nextSink;
    //         }
    //     }
    //     
    //     public IMessage SyncProcessMessage(IMessage msg)
    //     {
    //         IMessage message = null;
    //         var callMessage = msg as IMethodCallMessage;
    //         if(callMessage != null)
    //         {
    //             var attr = ReflectionUtil.GetAttribute<AOPMethodAttribute>(callMessage.MethodBase as MethodInfo);
    //             if(attr != null)
    //             {
    //                 PreProceed(msg);
    //                 // message = _nextSink.SyncProcessMessage(msg);
    //                 PostProceed(msg);
    //             }
    //             message = _nextSink.SyncProcessMessage(msg);
    //         }
    //         else
    //         {
    //             message = _nextSink.SyncProcessMessage(msg);
    //         }
    //         //message 指向对应的方法，如果返回null，则无法调用方法
    //         return message;
    //     }
    //     
    //     public IMessageCtrl AsyncProcessMessage(IMessage msg,IMessageSink replySink)
    //     {
    //         return null;
    //     }
    //     
    //     public void PreProceed(IMessage msg)
    //     {
    //         var message = msg as IMethodMessage;
    //         var paramss = message.Args;
    //     }
    //     
    //     public void PostProceed(IMessage msg)
    //     {
    //         var message = msg as IMethodReturnMessage;
    //         var param = message.ReturnValue;
    //     }
    // }
}