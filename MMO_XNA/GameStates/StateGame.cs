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
    public class StateGame : ClientState
    {
        public SpriteFont myFont;
        public Texture2D CharacterTexture;
        public Texture2D CharacterMask;
        public Texture2D BackgroundTexture;
        public Texture2D BackgroundTexture2;

        public Texture2D CharacterBarbarian;

        TimeSpan lastPressing;

        public StateGame(Game1 game1): base(game1)
        {
            CharacterTexture = game.Content.Load<Texture2D>("teste");
            CharacterMask = game.Content.Load<Texture2D>("testeMask");
            CharacterBarbarian = game.Content.Load<Texture2D>("Barbarian");
            BackgroundTexture = game.Content.Load<Texture2D>("Grass");
            BackgroundTexture2 = game.Content.Load<Texture2D>("water");
            myFont = game.Content.Load<SpriteFont>("myFont");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 17; j++)
                {

                    if (game.map == null)
                    {
                        spriteBatch.Draw(BackgroundTexture, new Vector2(i * 16, j * 16), Color.White);
                        continue;
                    }

                    if (game.map[i, j] == null || game.map[i, j] == 0x00)
                        spriteBatch.Draw(BackgroundTexture, new Vector2(i * 16, j * 16), Color.White);
                    else
                        spriteBatch.Draw(BackgroundTexture2, new Vector2(i * 16, j * 16), Color.White);  
                }
            }
        }

        public override void AfterDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (lastPressing == null)
                lastPressing = gameTime.TotalGameTime;

            if (game.Components.Count > 0)
            {

                if ((gameTime.TotalGameTime - lastPressing).Milliseconds > 200)
                {
                    if (keyState.IsKeyDown(Keys.Left))
                    {
                        //tChar.position.X -= 1f;
                        //tChar.facing = 2;
                        game.SendMove(0);
                        lastPressing = gameTime.TotalGameTime;
                    }
                    else if (keyState.IsKeyDown(Keys.Right))
                    {
                        //tChar.position.X += 1f;
                        //tChar.facing = 0;
                        game.SendMove(1);
                        lastPressing = gameTime.TotalGameTime;
                    }
                    else if (keyState.IsKeyDown(Keys.Up))
                    {
                        //tChar.position.Y -= 1f;
                        //tChar.facing = 3;
                        game.SendMove(2);
                        lastPressing = gameTime.TotalGameTime;
                    }
                    else if (keyState.IsKeyDown(Keys.Down))
                    {
                        //tChar.position.Y += 1f;
                        //tChar.facing = 1;
                        game.SendMove(3);
                        lastPressing = gameTime.TotalGameTime;
                    }
                }
            }

            if (keyState.IsKeyDown(Keys.Escape))
            {
                this.game.Exit();
            }
        }
    }
}
