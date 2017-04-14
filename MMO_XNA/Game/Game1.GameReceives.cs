using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Teste
{
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        public byte[,] map = { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

        public void ReceiveLogin(ref NetMsg netMsg)
        {
            byte len = netMsg.GetByte();

            if (len == 0x00)
            {
                Msg("Conta ou senha incorreta", "Arena of testes");
                sock.Disconnect(true);
                return;
            }

            //"entra na tela do jogo jogo"
            clientState = new StateGame(this);
            this.myForm.Invoke(new Action(() => { this.myForm.hideThings(); }));

            //Insere todos players
            for (int i = 0; i < len; i++)
            {
                byte pid = netMsg.GetByte();
                float x = netMsg.GetByte();
                float y = netMsg.GetByte();
                byte outfit = netMsg.GetByte();
                string name = netMsg.GetString();

                Components.Add(new Character(this, new Vector2(x, y), pid, name, outfit));
            }

            map = new byte[22, 17];

            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    map[i, j] = netMsg.GetByte();
                }
            }

        }

        public void ReceiveMove(ref NetMsg netMsg)
        {
            if (Components.Count == 0)
                return;

            byte pid = netMsg.GetByte();
            byte x = netMsg.GetByte();
            byte y = netMsg.GetByte();
            byte outfit = netMsg.GetByte();

            string name = netMsg.GetString();
            Character tChar = GetPlayerByPID(pid);

            if (tChar != null)
            {
                tChar.MoveTo(new Vector2(x, y));
            }
            else
            {
                tChar = new Character(this, new Vector2(x, y), pid, name, outfit);
                Components.Add(tChar);
            }
        }

        public void ReceiveSay(ref NetMsg netMsg)
        {
            byte posx = netMsg.GetByte();
            byte posy = netMsg.GetByte();
            string msgSay = netMsg.GetString();

            Message m = new Message(this, msgSay, new Vector2(posx*16, posy*16-20));
            this.Components.Add(m);
        }

        public void ReceiveDisconnect(ref NetMsg netMsg)
        {
            byte pid = netMsg.GetByte();
            Character tChar = GetPlayerByPID(pid);
            Components.Remove(tChar);
        }

    }

}
