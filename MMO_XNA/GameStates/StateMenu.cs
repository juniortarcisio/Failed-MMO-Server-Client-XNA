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
    public class StateMenu : ClientState
    {
        public Texture2D capaTexture;
        public Texture2D titleTexture; 
        public StateMenu(Game1 game1) : base(game1)
        {
            capaTexture = game.Content.Load<Texture2D>("bg1");
            titleTexture = game.Content.Load<Texture2D>("logo1ok");           
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(capaTexture, new Rectangle(0, 0, game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2,
                                                  game.GraphicsDevice.PresentationParameters.BackBufferHeight / 2), Color.White);
            
        }

        public override void AfterDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleTexture, new Vector2(game.GraphicsDevice.PresentationParameters.BackBufferWidth  - 300, 10), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
