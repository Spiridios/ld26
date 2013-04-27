using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Spiridios.SpiridiEngine;
using Spiridios.SpiridiEngine.Input;
using Spiridios.SpiridiEngine.Scene;
using System;

namespace Spiridios.LD26
{

    public class TitleState : State
    {
        public TitleState(SpiridiGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            game.DrawText("Ludum Dare 26", TextRenderer.CENTERED, (game.WindowHeight - 10) / 2);
            game.DrawText("Press Any Key To Start", TextRenderer.CENTERED, (game.WindowHeight + 30) / 2);
            game.DrawFPS();
        }

        public override void KeyUp(KeyboardEvent keyState)
        {
            base.KeyUp(keyState);
            game.NextState = new PlayGameState(game);
            game.NextState.Initialize();
        }

        public override void KeyDown(KeyboardEvent keyState)
        {
            base.KeyDown(keyState);
        }
    }

    public class PlayGameState : State
    {
        private Scene gameMap;
        private PlayerActor player;
        private SoundEffect testEffect;
        private SoundEffectInstance soundEffectInstance;
        private AudioEmitter emitter;
        private AudioListener listener;

        public PlayGameState(SpiridiGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            this.game.ImageManager.AddImage("Player", "Player.png");
            this.game.ImageManager.AddImage("Tileset", "Tileset.png");

            emitter = new AudioEmitter();
            listener = new AudioListener();

            testEffect = this.game.Content.Load<SoundEffect>("Pickup_Coin3");
            soundEffectInstance = testEffect.CreateInstance();
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Apply3D(listener, emitter);
            soundEffectInstance.Play();
            emitter.Position = new Vector3(100,100,1);

            gameMap = new Scene(game);
            gameMap.LoadTiledMap("Map.tmx");
            gameMap.Camera.Center(new Vector2(100, 100));

            player = new PlayerActor(this.inputManager, listener);
            player.Position = new Vector2(10, 10);
            SceneLayer mapLayer = gameMap.GetLayer("Map");
            mapLayer.AddActor(player);

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            gameMap.Draw(game.SpriteBatch);
            game.DrawFPS();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            gameMap.Update(gameTime.ElapsedGameTime);
            soundEffectInstance.Apply3D(listener, emitter);
        }

    }


}