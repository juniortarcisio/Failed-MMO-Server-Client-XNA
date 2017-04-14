using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teste
{
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        public void SendLogin(String account, String password)
        {
            NetMsg msg = new NetMsg();

            msg.AddString(account);
            msg.AddString(password);

            SendPacket(msg);
        }

        public void SendMove(byte dir)
        {
            NetMsg msg = new NetMsg();
            msg.AddByte(0);
            msg.AddByte(dir);

            SendPacket(msg);
        }

        public void SendSay(String msgSay)
        {
            NetMsg msg = new NetMsg();
            msg.AddByte(1);
            msg.AddString(msgSay);

            SendPacket(msg);
        }

        public void SendPacket(NetMsg msg)
        {
            if (sock == null || !sock.Connected)
            {
                MessageBox(new IntPtr(0), "socket não conectado", "error SendMove()", 0);
                return;
            }

            try
            {
                sock.Send(msg.GetBuff, msg.Length, 0);
            }
            catch (Exception ex)
            {
                MessageBox(new IntPtr(0), ex.Message, "error SendMove()", 0);
            }
        }

    }
}
