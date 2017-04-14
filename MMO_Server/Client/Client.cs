using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;	

namespace GameServer
{
    internal class Client
    {
        public Socket sock;
        public byte[] buff = new byte[16384];

        public byte x = 7;
        public byte y = 7;
        public byte pID;
        public string pName = "";
        public byte outfit = 0;

        public Client(byte newPID, Socket sock)
        {
            this.pID = newPID;
            this.sock = sock;

        }
    }
}
