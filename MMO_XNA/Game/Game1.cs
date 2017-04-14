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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        public ClientState clientState;
        Song music;

        public Form1 myForm;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D RTarget;
        
        public Game1(Form1 form, IntPtr drawSurface)
        { 
            myForm = form;
            form.game = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 264*2;
            graphics.PreferredBackBufferWidth = 346*2;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;

            InitializeForm(drawSurface);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        public Character GetPlayerByPID(byte pid)
        {
            foreach (Character character in Components)
            {
                if (character.pID == pid)
                    return character;
            }

            return null;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Add the SpriteBatch service
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            RTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight);

            music = Content.Load<Song>("3");
            MediaPlayer.Play(music);
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;
            this.clientState = new StateMenu(this);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            if (sock != null)
                if (sock.Connected)
                    sock.Disconnect(false);
        }
        
        protected override void Update(GameTime gameTime)
        {
            clientState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(RTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw
            spriteBatch.Begin();

            clientState.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);

            spriteBatch.End();
            
            /* DESCOMENTAR PRA DUPLICAR A TELA ALI EM CIMA TB */
            GraphicsDevice.SetRenderTarget(null);
            
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(RTarget, new Vector2(0,0), new Rectangle(0, 0,
                                                    GraphicsDevice.PresentationParameters.BackBufferWidth,
                                                    GraphicsDevice.PresentationParameters.BackBufferHeight),
                                Color.White, 0f, new Vector2(0,0), 2f, SpriteEffects.None, 1f);

            spriteBatch.End();

            spriteBatch.Begin();
            clientState.AfterDraw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
