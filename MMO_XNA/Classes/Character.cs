using System;
using System.Collections.Generic;
using System.Linq;

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
    public class Character : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public byte pID;

        TimeSpan fromTime;
        Vector2 fromPosition;
        bool isMoving = false;

        public Vector2 position;

        public int facing;
        public string name;

        private Texture2D charTex;
       // private Texture2D charMask;

        private const int SPRITESIZE = 24;

        public Character(Game1 game, Vector2 newPosition, byte newPID, string name, byte outfit) : base(game)
        {
            pID = newPID;

            if (outfit == 1)
            {
                charTex = ((StateGame) game.clientState).CharacterTexture;
                //charMask = ((StateGame)game.clientState).CharacterMask;
            }
            else
            {
                charTex = ((StateGame)game.clientState).CharacterBarbarian;
            }

            position = newPosition;
            facing = 0;
            this.name = name;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (isMoving)
            {
                fromTime = gameTime.TotalGameTime;
                isMoving = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            Game1 game1 = (Game1) Game;


            Vector2 pos = new Vector2(position.X * 16, position.Y * 16);

            float x;
            float y;
            
            double ticks = (gameTime.TotalGameTime - this.fromTime).TotalSeconds;
            Rectangle drawRect;

            if (ticks < 0.2)
            {
                ticks = ticks / 0.2;
                x = fromPosition.X * 16f + ((position.X - fromPosition.X) * 16f * (float)ticks);
                y = fromPosition.Y * 16f + ((position.Y - fromPosition.Y) * 16f * (float)ticks);

                if (ticks < 0.33)
                    drawRect = new Rectangle(facing * SPRITESIZE, 1 * SPRITESIZE, SPRITESIZE, SPRITESIZE);
                else if (ticks < 0.66)
                    drawRect = new Rectangle(facing * SPRITESIZE, 0, SPRITESIZE, SPRITESIZE);
                else
                    drawRect = new Rectangle(facing * SPRITESIZE, 2 * SPRITESIZE, SPRITESIZE, SPRITESIZE);
            }
            else
            {
                x = position.X * 16;
                y = position.Y * 16;
                drawRect = new Rectangle(facing * SPRITESIZE, 0, SPRITESIZE, SPRITESIZE);
            }

            sBatch.Draw(charTex, new Vector2(x,y), drawRect, Color.White);
            sBatch.DrawString(((StateGame)game1.clientState).myFont, this.name, new Vector2(x, y - 13), Color.Yellow);
            sBatch.DrawString(((StateGame)game1.clientState).myFont, "____", new Vector2(x, y - 12), Color.Red);
            sBatch.DrawString(((StateGame)game1.clientState).myFont, "____", new Vector2(x, y - 10), Color.Blue);

            // muda cor ali no fim
            //if (this.pID % 3 == 0)
            //    sBatch.Draw(charMask, pos, drawRect, Color.PowderBlue);
            //else if (this.pID % 3 == 2)
            //    sBatch.Draw(charMask, pos, drawRect, Color.ForestGreen);
            //else
            //    sBatch.Draw(charMask, pos, drawRect, Color.Firebrick);

            base.Draw(gameTime);
        }

        public void MoveTo(Vector2 newPosition)
        {
            this.fromPosition.X = this.position.X;
            this.fromPosition.Y = this.position.Y;
            this.isMoving = true;

            this.position.X = newPosition.X;
            this.position.Y = newPosition.Y;
            //Game.Tick();
        }

    }
}
