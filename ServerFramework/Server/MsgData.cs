using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Server
{
    class MsgData
    {
        public Client client;
        public byte[] data = new byte[1024];
    }
}
