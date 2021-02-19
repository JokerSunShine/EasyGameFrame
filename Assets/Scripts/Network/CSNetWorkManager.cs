using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Instrumentation;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class CSNetWorkManager : ManagerObject
{
    #region 数据
    private static CSNetWorkManager instance;
    public static CSNetWorkManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    private CSGameManager gameManager;

    /// <summary>
    /// 客户端
    /// </summary>
    private TcpClient Client;
    /// <summary>
    /// 拟定包体大小
    /// </summary>
    private const int READ_BUFFER_SIZE = 8192 * 2;
    private byte[] readBuf = new byte[READ_BUFFER_SIZE];
    private CircularBuffer<byte> ringBuf = new CircularBuffer<byte>(READ_BUFFER_SIZE, true);
    private static readonly object objThread = new object();
    /// <summary>
    /// 服务器最新数据更新时间
    /// </summary>
    private float latestReceivedServerMessageTime = 0;
    //每帧缓存一下自游戏开始的时间
    private float realTimeSinceStartupCache;
    /// <summary>
    /// 源服务器数据队列
    /// </summary>
    protected static Queue<NetworkMsg> mMsgEvents = new Queue<NetworkMsg>();
    /// <summary>
    /// 待推送消息队列
    /// </summary>
    protected static Queue<NetInfo> pushMsgQueue = new Queue<NetInfo>();
    /// <summary>
    /// 拆包线程
    /// </summary>
    private Thread UnpackMsgThread;
    /// <summary>
    /// 拆包线程状态
    /// </summary>
    public bool UnpackMsgThreadIsAlive
    {
        get
        {
            return UnpackMsgThread != null && UnpackMsgThread.IsAlive;
        }
    }
    /// <summary>
    /// 域名
    /// </summary>
    private string Host;
    /// <summary>
    /// 端口
    /// </summary>
    private int Port;
    /// <summary>
    /// 登录服务器类型
    /// </summary>
    private LoginServerType LoginServerType;
    /// <summary>
    /// 当前需要重连
    /// </summary>
    private bool IsReconnecting;
    #endregion

    #region 构造函数
    public CSNetWorkManager(CSGameManager gameManager) {
        instance = this;
        this.gameManager = gameManager;
    }
    #endregion

    #region 连接网络
    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="host">主机id</param>
    /// <param name="port">端口id</param>
    /// <param name="loginServerType">登录服务器类型</param>
    public void Connect(string host, int port, LoginServerType loginServerType = LoginServerType.GameServer)
    {
        if(UnpackMsgThreadIsAlive == false)
        {
            OpenUnpackThread();
        }
        if(Client != null)
        {
            CloseClient();
        }

        Host = host;
        Port = port;
        LoginServerType = loginServerType;

        Client = new TcpClient();
        Client.SendTimeout = 3000;
        Client.ReceiveTimeout = 3000;
        Client.NoDelay = true;
        
        try
        {
            Client.BeginConnect(Host, Port, new AsyncCallback(OnConnect), Client);
        }
        catch (System.Exception ex)
        {
            CSDebug.LogError("DayConnectClient Error");
            IsReconnecting = false;
        }
    }

    /// <summary>
    /// 启动拆包线程
    /// </summary>
    private void OpenUnpackThread()
    {
        CLoseUnpackThread();
        
        UnpackMsgThread = new Thread(UnpackAnalysisThread)
        {
            IsBackground = true
        };
        UnpackMsgThread.Priority = System.Threading.ThreadPriority.BelowNormal;
        UnpackMsgThread.Start();
    }
    
    /// <summary>
    /// 关闭拆包线程
    /// </summary>
    private void CLoseUnpackThread()
    {
        if(UnpackMsgThread != null)
        {
            UnpackMsgThread.Abort();
            UnpackMsgThread = null;
        }
    }
    
    /// <summary>
    /// 拆包解析线程
    /// </summary>
    private void UnpackAnalysisThread()
    {
        while (true)
        {
            if(mMsgEvents.Count <= 0)
            {
                return;
            }

            lock (objThread)
            {
                NetworkMsg msg = mMsgEvents.Dequeue();
                if(msg.msgId != 0)
                {
                    Type type = NetMsgDataStruct.GetNetMsgType(msg.msgId);
                    try
                    {
                        object curMsgData = ProtoBufManager.Deserializer(msg.data,type,msg.msgId);
                        lock(objThread)
                        {
                            pushMsgQueue.Enqueue(new NetInfo(msg.msgId,curMsgData));
                        }
                    }
                    catch(Exception)
                    {
                        lock(objThread)
                        {
                            pushMsgQueue.Enqueue(new NetInfo(msg.msgId,null));
                        }
                    }
                }
            }
            Thread.Sleep(1);
        }
    }
    
    /// <summary>
    /// 关闭客户端链接
    /// </summary>
    private void CloseClient()
    {
        CSDebug.Log("关闭服务器连接：Host = " + Host + " Port = " + Port.ToString());
        if(Client == null)
        {
            return;
        }
        Client.Close();
        Client = null;
    }
    
    /// <summary>
    /// 连接上服务器
    /// </summary>
    private void OnConnect(IAsyncResult ar)
    {
        try
        {
            TcpClient c = (TcpClient)ar.AsyncState;
            c.EndConnect(ar);

            if (Client != null && Client.Connected)
            {
                lock (objThread)
                {
                    ServerConnectedSuccess();
                }
                Client.GetStream().BeginRead(readBuf, 0, READ_BUFFER_SIZE, new AsyncCallback(ServerByteReceive), null);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("OnConnect Error");
            ServerConnectedFailed(e);
        }
    }
    
    public virtual void ServerConnectedSuccess()
    {
        
    }
    
    public virtual void ServerConnectedFailed(Exception e)
    {
        
    }
    #endregion

    #region 请求服务器数据
    /// <summary>
    /// 发送消息
    /// </summary>
    public void SendMsg(int msgId,System.Object msg = null)
    {
        if (Client == null || !Client.Connected) { return; }

        byte[] msgBytes;

        using (MemoryStream stream = new MemoryStream())
        {
            ProtoBuf.Serializer.Serialize(stream, msg);
            msgBytes = stream.ToArray();
        }
#if MSGDEBUG
        if (msg != null)
        {
            if (!msgDebugDic.ContainsKey(msg.GetType().ToString()))
            {
                msgDebugDic.Add(msg.GetType().ToString(), true);
            }
        }
#endif
        SendMsgByte(msgId, msgBytes);
    }
    
    /// <summary>
    /// 通过字节组发送消息
    /// </summary>
    /// <param name="msgId"></param>
    /// <param name="msgData"></param>
    public void SendMsgByte(int msgId,byte[] msgData)
    {
        if (msgData == null) { return; }
        byte[] packageBytes = null;
        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                //if (IsEncrypMsg(id))
                //msgData = UtilityEncrypt.AesNetEncrypt(msgData);
                BinaryWriter bw = new BinaryWriter(stream);
                int length = sizeof(uint) * 2 + sizeof(short) + msgData.Length;
                bw.Write((uint)IPAddress.HostToNetworkOrder(length));
                bw.Write((uint)IPAddress.HostToNetworkOrder(msgId));
                bw.Write(IPAddress.HostToNetworkOrder((short)0));
                bw.Write(msgData);
                packageBytes = stream.ToArray();
            }

            //UnityEngine.Debug.LogError(string.Format("发送消息ID:{0}  内容长度为:{1}  消息长度为:{2} -----   内容为:{3} ", msgId, msgData.Length, packageBytes.Length, System.Text.Encoding.UTF8.GetString(msgData)));

            BinaryWriter writer = new BinaryWriter(Client.GetStream());
            writer.Write(packageBytes);
            writer.Flush();
        }
        catch (Exception) { }
    }
    #endregion

    #region 接收服务器数据
    /// <summary>
    /// 接受主机数据
    /// </summary>
    /// <param name="ar"></param>
    private void ServerByteReceive(IAsyncResult ar)
    {
        try
        {
            int sizeRead = 0;

            lock (Client.GetStream())
            {   //读取字节流到缓冲区
                sizeRead = Client.GetStream().EndRead(ar);
            }

            if (sizeRead < 1)
            {
                //包尺寸有问题，断线处理
                //若包尺寸为0,则为服务器故意发送的断线消息,不进行重连;若包尺寸不为0,则为网络故障引起的,尝试重连
                Client = null;
                return;
            }

            ringBuf.Put(readBuf, 0, sizeRead);

            AnalyticalNetworkMsg();
            
            lock (Client.GetStream())
            {
                Array.Clear(readBuf, 0, readBuf.Length);   //清空数组
                Client.GetStream().BeginRead(readBuf, 0, READ_BUFFER_SIZE, new AsyncCallback(ServerByteReceive), null);
            }
        }
        catch (Exception ex)
        {
            // ServerDisconnect(ex.GetType() + "\n" + ex.Message + "\n" + ex.StackTrace, true);
        }
    }
    
    private void AnalyticalNetworkMsg()
    {
        while (ringBuf.Size >= sizeof(uint))
        {
            byte[] lengthByte = new byte[sizeof(uint)];

            ringBuf.CopyTo(0, lengthByte, 0, lengthByte.Length);

            uint msgLength = BitConverter.ToUInt32(lengthByte, 0);

            msgLength = NetworkToHostOrder(msgLength);

            if (ringBuf.Size >= msgLength)
            {
                byte[] msgdata = new byte[msgLength];

                ringBuf.Get(msgdata);

                if (msgLength == 0)
                {
                    ringBuf.Clear();
                    return;
                }

                using (MemoryStream stream = new MemoryStream(msgdata))
                {
                    BinaryReader br = new BinaryReader(stream);

                    NetworkMsg Nmsg = new NetworkMsg();

                    try
                    {
                        Nmsg.length = NetworkToHostOrder(br.ReadInt32());
                        Nmsg.msgId = NetworkToHostOrder(br.ReadInt32());
                        Nmsg.sequence = (short)NetworkToHostOrder(br.ReadInt16());
                    }
                    catch (Exception)
                    {
                        CSDebug.Log(" Nmsg.length ==null|| Nmsg.msgId == null");
                        throw;
                    }

                    // 10 is header length    length:4  msgId:4 sequence:2
                    byte[] data = br.ReadBytes((int)msgLength - 10);

                    //if (IsDecrypMsg(Nmsg.msgId))
                    //    data = UtilityEncrypt.AesNetDecrypt(data);

                    Nmsg.data = data;

                    lock (objThread)
                    {
                        //在接收到的了服务器的消息时更新该时间,以真实的保留接收到数据的时间而非分发消息的时间
                        latestReceivedServerMessageTime = realTimeSinceStartupCache;
                        mMsgEvents.Enqueue(Nmsg);
                    }
                }
            }
            else
            {
                break;
            }
        }
    }
        
    private uint NetworkToHostOrder(uint val)
    {
        byte[] array = BitConverter.GetBytes(val);
        Array.Reverse(array);
        return BitConverter.ToUInt32(array, 0);
    }
    
    private int NetworkToHostOrder(int val)
    {
        byte[] array = BitConverter.GetBytes(val);
        Array.Reverse(array);
        return BitConverter.ToInt32(array, 0);
    }
    #endregion
    
    #region 数据包数据结构
    /// <summary>
    /// 数据包元数据
    /// </summary>
    public struct NetworkMsg
    {
        public int length; //长度
        public int msgId;//id
        public byte[] data;//正文
        public short sequence;

        public NetworkMsg(int _length, int _msgId, byte[] _data, short sequence)
        {
            this.length = _length;
            this.msgId = _msgId;
            this.data = _data;
            this.sequence = sequence;
        }
    }
    
    /// <summary>
    /// 数据包解析后数据
    /// </summary>
    public struct NetInfo
    {
        public int msgId;//id
        public object obj;
        public NetInfo(int _msgid, object _obj)
        {
            this.msgId = _msgid;
            this.obj = _obj;
        }
    }
    #endregion
}
