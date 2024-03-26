using PlayerInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Server
{
    class Client
    {
        public Socket socketCli;
        public byte[] data = new byte[1024];

        public PlayerData playerData;
    }
}
