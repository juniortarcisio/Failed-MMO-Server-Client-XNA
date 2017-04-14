using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Teste
{
    public abstract class ClientState
    {
        public Game1 game;
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void AfterDraw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

        public ClientState(Game1 game1)
        {
            this.game = game1;
        }

        ~ClientState()
        {
        }

    }
}
