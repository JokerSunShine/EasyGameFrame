using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class CSNetWorkManager : AbstractManager
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
    private const int READ_BUFFER_SIZE = 8192 * 2;
    private byte[] readBuf = new byte[READ_BUFFER_SIZE];
    private CircularBuffer<byte> ringBuf = new CircularBuffer<byte>(READ_BUFFER_SIZE, true);
    private static readonly object objThread = new object();
    private float latestReceivedServerMessageTime = 0;
    //每帧缓存一下自游戏开始的时间
    private float realTimeSinceStartupCache;
    protected static Queue<NetworkMsg> mMsgEvents = new Queue<NetworkMsg>();
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
    public void Connect(string host, string port, LoginServerType loginServerType)
    {
        
    }

    /// <summary>
    /// 启动拆包线程
    /// </summary>
    private void OpenUnpackThread()
    {
        
    }
    
    /// <summary>
    /// 连接上服务器
    /// </summary>
    private void OnConnect()
    {
        
    }
    #endregion

    #region 请求服务器数据
    
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
            //if (Client == null)
            //{
            //    ServerDisconnect("", false);
            //    return;
            //}

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
    #endregion
}
