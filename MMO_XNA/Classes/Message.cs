using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Message : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public string text;
        public Vector2 position;
        TimeSpan startTime;
        bool started = false;
        Game1 game;

        public Message(Game1 game, String text, Vector2 position) : base(game)
        {
            this.text = text;
            this.position = position;
            this.game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!started)
            {
                startTime = gameTime.TotalGameTime;
                started = true;
            }
            else
            {
                if ((gameTime.TotalGameTime - startTime).TotalSeconds > 5)
                {
                    game.Components.Remove(this);
                    this.Dispose();
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            Game1 game1 = (Game1)Game;

            sBatch.DrawString(((StateGame)game1.clientState).myFont, text, position, Color.Yellow);

            base.Draw(gameTime);
        }
    }
}
