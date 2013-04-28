using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Spiridios.SpiridiEngine;
using Spiridios.SpiridiEngine.Audio;
using Spiridios.SpiridiEngine.Input;
using Spiridios.SpiridiEngine.Scene;
using System;
using System.Collections.Generic;

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

            int lineHeight = 40;
            int numLines = 4;
            int firstLine = (game.WindowHeight - (numLines * lineHeight)) / 2;
            game.DrawText("Conversion", TextRenderer.CENTERED, firstLine);
            game.DrawText("By Spiridios for Ludum Dare 26", TextRenderer.CENTERED, firstLine + (1 * lineHeight));
            game.DrawText("Press Any Key To Start", TextRenderer.CENTERED, firstLine + (3 * lineHeight));
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
        private List<LevelData> levelData = new List<LevelData>();

        public PlayGameState(SpiridiGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            // Load any continents 
            this.game.ImageManager.AddImage("Player", "Player.png");
            this.game.ImageManager.AddImage("Tileset", "Tileset.png");
            this.game.ImageManager.AddImage("Sound", "Sound.png");

            gameMap = new Scene(game);
            gameMap.LoadTiledMap("Map.tmx");
            gameMap.Camera.Center(new Vector2(100, 100));
            SceneLayer mapLayer = gameMap.GetLayer("Map");

            player = new PlayerActor(this.inputManager);
            player.Position = new Vector2(10, 10);
            mapLayer.AddActor(player);

            SetupLevelData(mapLayer);
        }

        private void SetupLevelData(SceneLayer mapLayer)
        {
            levelData.Add(new LevelData("drip", new Vector2(100, 100), "You feel a cold wet metal pipe with a knob", "You turn the knob", null));

            foreach (LevelData levelItem in levelData)
            {
                SoundActor sa = SoundActor.CreateActor(levelItem, mapLayer, player);
                // TODO: will this work?
                sa.Sound.PlayLooped();
            }

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            gameMap.Draw(game.SpriteBatch);
            ((LD26)this.game).DrawMessage();
            game.DrawFPS();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ((LD26)this.game).LastGameTime = gameTime;
            gameMap.Update(gameTime.ElapsedGameTime);
        }

    }


}