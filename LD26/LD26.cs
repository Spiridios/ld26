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
using Spiridios.SpiridiEngine;

namespace Spiridios.LD26
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LD26 : SpiridiGame
    {
        private TextRenderer messageTextRenderer;
        private string message;
        private double messageDisplayed;
        private const double messageDisplayTime = 2.0;
        private GameTime lastGameTime = null;



        public GameTime LastGameTime
        {
            get { return lastGameTime; }
            set { this.lastGameTime = value; }
        }

        public LD26()
        {
            this.LockFramerate = false;
            this.SetWindowSize(640, 480);
            this.NextState = new BootState(this, new TitleState(this));
            this.ClearColor = new Color(0x22, 0x22, 0x22);
            this.IsQuickExit = true;
            this.ShowFPS = false;
        }

        public static void DisplayMessage(string message)
        {
            LD26 instance = (LD26)SpiridiGame.Instance;
            instance.message = message;
            instance.messageDisplayed = instance.lastGameTime.TotalGameTime.TotalSeconds;

        }

        public void DrawMessage()
        {
            if (!String.IsNullOrWhiteSpace(this.message))
            {
                messageTextRenderer.DrawText(this.SpriteBatch, this.message, TextRenderer.CENTERED, (this.WindowHeight - 20) / 2);
                if ((this.lastGameTime.TotalGameTime.TotalSeconds - this.messageDisplayed) > messageDisplayTime)
                {
                    this.message = null;
                }
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Register key-bindings
            //this.InputManager.RegisterActionBinding("walkForward", Keys.W);
            //this.InputManager.RegisterActionBinding("walkLeft", Keys.A);
            //this.InputManager.RegisterActionBinding("walkBackward", Keys.S);
            //this.InputManager.RegisterActionBinding("walkRight", Keys.D);

            this.InputManager.RegisterActionBinding("walkForward", Keys.Up);
            this.InputManager.RegisterActionBinding("walkBackward", Keys.Down);
            this.InputManager.RegisterActionBinding("rotateLeft", Keys.Left);
            this.InputManager.RegisterActionBinding("rotateRight", Keys.Right);
            this.InputManager.RegisterActionBinding("doStuff", Keys.Space);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            this.DefaultTextRenderer = new TextRenderer(this, "TitleScreenFont", new Color(0xaa, 0xaa, 0xaa));
            this.messageTextRenderer = new TextRenderer(this, "MessageFont", new Color(0xaa, 0xaa, 0xaa));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
