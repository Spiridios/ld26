using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Spiridios.SpiridiEngine;
using Spiridios.SpiridiEngine.Audio;
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
        private PositionedSound positionedEffect;

        public PlayGameState(SpiridiGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            this.game.ImageManager.AddImage("Player", "Player.png");
            this.game.ImageManager.AddImage("Tileset", "Tileset.png");
            this.game.ImageManager.AddImage("Sound", "Sound.png");

            testEffect = this.game.Content.Load<SoundEffect>("Pickup_Coin3");
            this.positionedEffect = new PositionedSound(testEffect);
            this.positionedEffect.Position = new Vector2(100,100);
            this.positionedEffect.DebugImage = new Image("Sound");

            gameMap = new Scene(game);
            gameMap.LoadTiledMap("Map.tmx");
            gameMap.Camera.Center(new Vector2(100, 100));
            this.positionedEffect.Camera = gameMap.Camera;

            player = new PlayerActor(this.inputManager, positionedEffect);
            player.Position = new Vector2(10, 10);
            SceneLayer mapLayer = gameMap.GetLayer("Map");
            mapLayer.AddActor(player);
            positionedEffect.Listener = player;
            positionedEffect.PlayLooped();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            gameMap.Draw(game.SpriteBatch);
            positionedEffect.Draw(game.SpriteBatch);
            game.DrawFPS();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            gameMap.Update(gameTime.ElapsedGameTime);
            positionedEffect.Update(gameTime.ElapsedGameTime);
        }

    }


}