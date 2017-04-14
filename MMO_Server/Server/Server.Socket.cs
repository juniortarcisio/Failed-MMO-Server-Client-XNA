using System;
using System.Text;
using System.Threading;								// Sleeping
using System.Net;									// Used to local machine info
using System.Net.Sockets;							// Socket namespace
using System.Collections;							// Access to the Array list
using System.Data;
using DataMagic;

namespace GameServer
{
    partial class Server
    {
        TcpListener clientLoginListener = new TcpListener(IPAddress.Any, 7171);
        byte pIDcount = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        public void StartListenning()
        {
            clientLoginListener.Start();
            clientLoginListener.BeginAcceptSocket(new AsyncCallback(OnConnecting), clientLoginListener);

            Console.WriteLine("\n\nPress any key to close the server");
            Console.ReadLine();

            clientLoginListener.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        public void OnConnecting(IAsyncResult ar)
        {
            TcpListener sockListener = (TcpListener)ar.AsyncState;

            try
            {
                //Clona o socket e instância a classe client
                //---------------------------------------------------------------------
                Socket sockClone = sockListener.EndAcceptSocket(ar);
                Client client = new Client(pIDcount++, sockClone); //~~~Fixar o "playerID" com algum ID da database

                Console.WriteLine("Client {0}, connecting", sockClone.RemoteEndPoint);

                //Chama o evento inicial do client
                //---------------------------------------------------------------------
                client.sock.BeginReceive(client.buff, 0, client.buff.Length, SocketFlags.None, new AsyncCallback(OnFirstRecievedData), client);

                //Socket principal em listenning para outros clients que tentarem connectar
                //---------------------------------------------------------------------
                sockListener.BeginAcceptSocket(new AsyncCallback(OnConnecting), sockListener);
            }
            catch (Exception)
            {
                Console.WriteLine("Client {0}, OnConnecting failed", sockListener.LocalEndpoint);
                sockListener.Stop();
                sockListener = null;
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        public void OnFirstRecievedData(IAsyncResult ar)
        {
            Client client = (Client)ar.AsyncState;

            //Trata o login
            //---------------------------------------------------------------------
            NetMsg netMsg = new NetMsg(client.buff);

            string acc = netMsg.GetString();
            string pass = netMsg.GetString();

            Login(client, acc, pass);

            //Coloca em listenning novamente
            //---------------------------------------------------------------------
            try
            {
                client.sock.BeginReceive(client.buff, 0, client.buff.Length, SocketFlags.None, new AsyncCallback(OnRecievedData), client);
            }
            catch (Exception)
            {
                Console.WriteLine("Client {0} {1}, BeginReceive failed", client.sock.RemoteEndPoint, client.pName);
                Disconnect(client);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        public void OnRecievedData(IAsyncResult ar)
        {
            Client client = (Client)ar.AsyncState;

            #region "Trata possíveis erros"

            try
            {
                client.sock.EndReceive(ar);
            }
            catch
            {
                Console.WriteLine("Client {0} {1}, onreceive failed", client.sock.RemoteEndPoint, client.pName);
                Disconnect(client);
                return;
            }

            // If no data was recieved then the connection is probably dead
            if (client.buff.Length < 1)
            {
                Console.WriteLine("Client {0} {1}, disconnected", client.sock.RemoteEndPoint, client.pName);
                Disconnect(client);
                return;
            }

            #endregion

            //Trata conteúdo recebido
            //---------------------------------------------------------------------
            NetMsg netMsg = new NetMsg(client.buff);
            byte firstByte = netMsg.GetByte();

            switch (firstByte)
            {
                case 0:
                    Move(client, netMsg.GetByte());
                    break;
                case 1:
                    Say(client, netMsg.GetString());
                    break;
                default:
                    break;
            }

            //Coloca em listenning novamente
            //---------------------------------------------------------------------
            try
            {
                client.sock.BeginReceive(client.buff, 0, client.buff.Length, SocketFlags.None, new AsyncCallback(OnRecievedData), client);
            }
            catch (Exception)
            {
                Console.WriteLine("Client {0} {1}, BeginReceive failed", client.sock.RemoteEndPoint, client.pName);
                Disconnect(client);
                return;                
            }
        }


        public void SendPacket(Client client, NetMsg msg)
        {
            if (client.sock == null || !client.sock.Connected)
            {
                Console.WriteLine("SendPacket(): socket não conectado {0}", client.pName);
                Disconnect(client);
                return;
            }

            try
            {
                client.sock.Send(msg.GetBuff, msg.Length, 0);
            }
            catch
            {
                Console.WriteLine("Say {0} failed", client.sock.RemoteEndPoint);
                Disconnect(client);
            }
        }

    }
}
