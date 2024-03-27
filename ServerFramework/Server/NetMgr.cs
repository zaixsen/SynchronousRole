using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ServerFramework.Server
{
    class NetMgr : Singleton<NetMgr>
    {
        Socket socket;

        public List<Client> clients = new List<Client>();

        public void Init()
        {
            Console.WriteLine("服务器开启...");
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //创建连接地址
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 9999);
            socket.Bind(iPEndPoint);
            //最大连接人数
            socket.Listen(10);
            //开始异步连接
            socket.BeginAccept(OnAsyAccept, null);
        }


        /// <summary>
        ///发送给所有人包括自己 
        /// </summary>
        public void AsyAllSend(int msgId, byte[] content)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                AsySend(clients[i], msgId, content);
            }
        }


        /// <summary>
        ///发送给所有人 不包括自己 
        /// </summary>
        public void AsyAllSend(int id, int msgId, byte[] content)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].playerData.UserId != id)
                {
                    AsySend(clients[i], msgId, content);
                }
            }
        }

        public void AsySend(Client client, int msgId, byte[] content)
        {
            int len = 4 + content.Length;
            byte[] msg = new byte[0];
            msg = msg.Concat(BitConverter.GetBytes(len)).Concat(BitConverter.GetBytes(msgId)).Concat(content).ToArray();
            client.socketCli.BeginSend(msg, 0, msg.Length, SocketFlags.None, OnAsySend, client);
        }

        private void OnAsySend(IAsyncResult ar)
        {
            try
            {
                Client client = ar.AsyncState as Client;
                int len = client.socketCli.EndSend(ar);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OnAsyAccept(IAsyncResult ar)
        {
            try
            {
                //获取连接的客户端    连接终端
                Socket clientSocket = socket.EndAccept(ar);
                Console.WriteLine("连接成功");

                Client client = new Client();
                client.socketCli = clientSocket;

                clientSocket.BeginReceive(client.data, 0, client.data.Length, SocketFlags.None, OnAsyReceive, client);

                //继续连接
                socket.BeginAccept(OnAsyAccept, null);
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
                Client client = ar.AsyncState as Client;

                int len = client.socketCli.EndReceive(ar);
                //下线的时候会发送消息长度为零的信息
                if (len <= 0)
                {
                    //禁止收发
                    client.socketCli.Shutdown(SocketShutdown.Both);
                    //取消关联的所有东西
                    client.socketCli.Close();
                    return;
                }

                //消息解析
                if (len > 0)
                {
                    //原数据流数组
                    byte[] rData = new byte[len];
                    Buffer.BlockCopy(client.data, 0, rData, 0, len);
                    while (rData.Length > 4)
                    {
                        //消息头 消息号 消息内容
                        int msgCount = BitConverter.ToInt32(rData, 0);
                        int msgId = BitConverter.ToInt32(rData, 4);
                        byte[] data = new byte[msgCount - 4];

                        Buffer.BlockCopy(rData, 8, data, 0, msgCount - 4);

                        //消息号发送
                        MsgData msgData = new MsgData();
                        msgData.client = client;
                        msgData.data = data;

                        MessageCenter<MsgData>.Ins.Dispatch(msgId, msgData);

                        //更新消息   当前数组长度  
                        int reLen = rData.Length - 4 - msgCount;
                        byte[] surplusData = new byte[reLen];
                        Buffer.BlockCopy(rData, 4 + msgCount, surplusData, 0, reLen);
                        rData = surplusData;
                    }
                }
                //继续接收
                client.socketCli.BeginReceive(client.data, 0, client.data.Length, SocketFlags.None, OnAsyReceive, client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
