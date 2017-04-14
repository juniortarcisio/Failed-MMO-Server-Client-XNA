using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer
{
    partial class Server
    {
        public void Login(Client client, String account, String password)
        {
            NetMsg netMsg;

            #region "database logic commented"
            //DataMagic.DataMagic dm = new DataMagic.DataMagic();

            //dm.Params.Add("@acc", acc);
            //dm.Params.Add("@pass", pass);

            //DataTable dt;
            //try
            //{
            //    dt = (DataTable)dm.GetTable("Arena..[GAME_Login]");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return;
            //}

            //if (dt.Rows.Count == 0)
            //{
            //    netMsg = new NetMsg();
            //    netMsg.AddByte(0x00);
            //    client.m_sock.Send(netMsg.GetBuff, netMsg.Length, 0);

            //    Console.WriteLine("Client {0}, invalid login", client.m_sock.RemoteEndPoint);
            //    client.m_sock.Close();
            //    return;
            //}

            #endregion

            //if (password != "hunted")
            //{
            //    netMsg = new NetMsg();
            //    netMsg.AddByte(0x00);
            //    client.sock.Send(netMsg.GetBuff, netMsg.Length, 0);

            //    Console.WriteLine("Client {0}, has a invalid login", client.sock.RemoteEndPoint);
            //    client.sock.Close();
            //    return;
            //}


            //Adiciona client a lista
            //---------------------------------------------------------------------
            client.pName = account; //(string) dt.Rows[0]["LGN_Account"];
            client.outfit = 2;      //(byte)dt.Rows[0]["LGN_Outfit"];

            _clientList.Add(client);
            Console.WriteLine("Client {0} {1}, has login with success", client.sock.RemoteEndPoint, client.pName);

            //Envia players em tela
            //---------------------------------------------------------------------
            netMsg = new NetMsg();
            netMsg.AddByte((byte)_clientList.Count);

            for (int i = 0; i < _clientList.Count; i++)
            {
                Client c = (Client)_clientList[i];

                netMsg.AddByte(c.pID);
                netMsg.AddByte(c.x);
                netMsg.AddByte(c.y);
                netMsg.AddByte(c.outfit);
                netMsg.AddString(c.pName);
            }

            //Map prototype
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    netMsg.AddByte(map[i, j]);
                    //if ((i % 2 == 0 && j % 2 == 0) || (i % 2 == 1 && j % 2 == 1))
                    //    netMsg.AddByte(0x01);
                    //else
                    //    netMsg.AddByte(0x00);
                }
            }

            SendPacket(client, netMsg);
        }

        public void Move(Client client, byte moveDir)
        {
            if (moveDir == 0 && client.x > 0)
                client.x -= 1;
            else if (moveDir == 1 && client.x < 20)
                client.x += 1;
            else if (moveDir == 2 && client.y > 1)
                client.y -= 1;
            else if (moveDir == 3 && client.y < 15)
                client.y += 1;

            foreach (Client clientSend in _clientList)
            {
                NetMsg netMsg = new NetMsg();

                netMsg.AddByte(0);
                netMsg.AddByte(client.pID);
                netMsg.AddByte(client.x);
                netMsg.AddByte(client.y);
                netMsg.AddByte(client.outfit);
                netMsg.AddString(client.pName);

                SendPacket(clientSend, netMsg);
            }
        }

        public void Say(Client client, String message)
        {
            message = client.pName + ": " + message;

            foreach (Client clientSend in _clientList)
            {
                NetMsg netMsg = new NetMsg();

                netMsg.AddByte(1);
                netMsg.AddByte(client.x);
                netMsg.AddByte(client.y);
                netMsg.AddString(message);

                SendPacket(clientSend, netMsg);
            }
        }

        public void Disconnect(Client client)
        {
            if (client.sock.Connected)
                client.sock.Close();

            _clientList.Remove(client);

            // Send the recieved data to all clients (including sender for echo)
            foreach (Client clientSend in _clientList)
            {
                if (client.pID == clientSend.pID)
                    continue;
               
                    NetMsg netMsg = new NetMsg();

                    netMsg.AddByte(0x02);
                    netMsg.AddByte(client.pID);

                    SendPacket(clientSend, netMsg);
            }
        }


    }
}
