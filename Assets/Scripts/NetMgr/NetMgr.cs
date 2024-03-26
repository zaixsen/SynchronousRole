using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class NetMgr : Singleton<NetMgr>
{
    Socket socket;

    byte[] data = new byte[1024];

    Queue<byte[]> datas = new Queue<byte[]>();

    public void Init()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.BeginConnect("127.0.0.1", 9999, OnAsyContent, null);
    }

    private void OnAsyContent(IAsyncResult ar)
    {
        try
        {
            socket.EndConnect(ar);

            Console.WriteLine("连接成功");

            //接收消息

            socket.BeginReceive(data, 0, data.Length, SocketFlags.None, OnAsyReceive, null);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void AsySend(int msgId, byte[] content)
    {
        int len = 4 + content.Length;
        byte[] msg = new byte[0];
        msg = msg.Concat(BitConverter.GetBytes(len)).Concat(BitConverter.GetBytes(msgId)).Concat(content).ToArray();
        socket.BeginSend(msg, 0, msg.Length, SocketFlags.None, OnAsySend, null);
    }

    private void OnAsySend(IAsyncResult ar)
    {
        try
        {
            int len = socket.EndSend(ar);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private void OnAsyReceive(IAsyncResult ar)
    {
        try
        {
            int len = socket.EndReceive(ar);

            if (len == 0)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return;
            }

            //消息解析
            if (len > 0)
            {
                //原数据流数组
                byte[] rData = new byte[len];
                Buffer.BlockCopy(data, 0, rData, 0, len);
                while (rData.Length > 4)
                {
                    //消息头 消息号 消息内容
                    int msgCount = BitConverter.ToInt32(rData, 0);

                    byte[] content = new byte[msgCount];
                    Buffer.BlockCopy(rData, 4, content, 0, msgCount);
                    //客户端消息在主线程中调用
                    datas.Enqueue(content);

                    //更新消息   当前数组长度
                    int reLen = rData.Length - 4 - msgCount;
                    byte[] surplusData = new byte[reLen];
                    Buffer.BlockCopy(rData, 4 + msgCount, surplusData, 0, reLen);
                    rData = surplusData;
                }
            }

            //继续接收
            socket.BeginReceive(data, 0, data.Length, SocketFlags.None, OnAsyReceive, null);

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void NetUpData()
    {
        if (datas.Count > 0)
        {
            byte[] data = datas.Dequeue();

            int msgId = BitConverter.ToInt32(data, 0);

            Debug.Log(msgId);
            byte[] content = new byte[data.Length - 4];
            Buffer.BlockCopy(data, 4, content, 0, content.Length);

            //消息号发送
            MessageCenter<byte[]>.Ins.Dispatch(msgId, content);
        }
    }
}
