using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Teste
{
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        private Socket sock;						// Server connection
        private byte[] buff = new byte[16384];	// Recieved data buffer
        string account;
        string password;
        
        public void Connect(String account, String password)
        {
            if (sock != null)
            {
                if (sock.Connected)
                {
                    Msg("Você já está conectado!", "Mano");
                    return;
                }
            }

            this.account = account;
            this.password = password;

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Blocking = false;
            IPAddress ip = Dns.GetHostByName("localhost").AddressList[0];
            sock.BeginConnect(new IPEndPoint(ip, 7171), new AsyncCallback(OnConnecting), sock);
        }
        
        //Primeira conexão
        private void OnConnecting(IAsyncResult ar)
        {            
            try
            {
                if (sock.Connected)
                {
                    SendLogin(account, password);
                    sock.BeginReceive(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(OnFirstRecievedData), sock);
                }
                else
                    MessageBox(new IntPtr(0), "Unable to connect to remote machine", "erro OnConnect()", 0);
            }
            catch (Exception ex)
            {
                MessageBox(new IntPtr(0), ex.Message, "error OnConnect()", 0);
            }
        }

        //Primeira recepção de dados
        private void OnFirstRecievedData(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;

            try
            {
                int nBytesRec = sock.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    NetMsg netMsg = new NetMsg(buff);
                    ReceiveLogin(ref netMsg);
                    sock.BeginReceive(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(OnRecievedData), sock);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    MessageBox(new IntPtr(0), "Client {0}, disconnected", "byteContent", 0);
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox(new IntPtr(0), ex.Message, "error OnConnectionRecievedData()", 0);
            }
        }
        
        private void OnRecievedData(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;

            try
            {
                int nBytesRec = sock.EndReceive(ar);

                if (nBytesRec > 0)
                {
                    NetMsg netMsg = new NetMsg(buff);
                    byte firstByte = netMsg.GetByte();

                    switch (firstByte)
                    {
                        case 0x00:
                            ReceiveMove(ref netMsg);
                            break;
                        case 0x01:
                            ReceiveSay(ref netMsg);
                            break;
                        case 0x02:
                            ReceiveDisconnect(ref netMsg);
                            break;
                        default:
                            break;
                    }

                    sock.BeginReceive(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(OnRecievedData), sock);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    MessageBox(new IntPtr(0), "Client {0}, disconnected", "byteContent", 0);
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox(new IntPtr(0), ex.Message, "error OnRecievedData()", 0);
            }
        }

        //private void ShowBuffData(int len)
        //{
        //    String byteContent = "";

        //    for (int i = 0; i < len; i++)
        //    {
        //        byteContent += "buff[" + i.ToString() + "]: " + buff[i].ToString() + "\n";
        //    }

        //    MessageBox(new IntPtr(0), byteContent, "byteContent", 0);
        //}


    }
}
