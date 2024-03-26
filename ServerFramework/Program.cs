using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerFramework.Server;

namespace ServerFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            HeroSelectMgr.Ins.Init();
            NetMgr.Ins.Init();

            Console.Read();
        }
    }
}
