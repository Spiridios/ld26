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
        private List<SoundEventData> levelData = new List<SoundEventData>();

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
            SoundManager.Instance.AddSound("gunshot", "gunshot.wav");

            gameMap = new Scene(game);
            gameMap.LoadTiledMap("Map.tmx");
            gameMap.Camera.Center(new Vector2(100, 100));
            SceneLayer mapLayer = gameMap.GetLayer("Map");

            SetupLevelData(mapLayer);
        }

        private void SetupLevelData(SceneLayer mapLayer)
        {
            player = new PlayerActor(this.inputManager);
            player.Position = new Vector2(20, 20);
            mapLayer.AddActor(player);

            levelData.Add(new SoundEventData(mapLayer, player, "gunshot", new Vector2(20, 20)).SetMessage("You feel no pain, but you can no longer see").SetRepeat(false));

            SoundEventData shuffleLoop =
            new SoundEventData(mapLayer, player, "shuffle", new Vector2(120, 160))
            .SetRepeat(false)
            .SetEventType(SoundEventData.EventType.OneShot);

            shuffleLoop.AddNextEvent(
            new SoundEventData(mapLayer, player, "breathing", new Vector2(90, 10))
            .SetEventType(SoundEventData.EventType.Collision)
            .AddNextEvent(
            new SoundEventData(mapLayer, player, "shuffle", new Vector2(60, 40))
            .SetRepeat(false)
            .SetEventType(SoundEventData.EventType.OneShot)
            .AddNextEvent(
            new SoundEventData(mapLayer, player, "breathing", new Vector2(10, 180))
            .SetEventType(SoundEventData.EventType.Collision)
            .AddNextEvent(shuffleLoop)
            )));

            levelData.Add(

            new SoundEventData(mapLayer, player, "drip", new Vector2(10, 100))
            .SetMessage("You feel a cold wet metal pipe with a knob")
            .SetEventType(SoundEventData.EventType.Activate)
            .AddNextEvent(

            new SoundEventData(mapLayer, player, "valve", new Vector2(10, 100))
            .SetRepeat(false)
            .SetMessage("You turn the knob")
            .AddNextEvent(
            
            new SoundEventData(mapLayer, player, "keys", new Vector2(100,180))
            .AddNextEvent(shuffleLoop


            )
            .AddNextEvent(
            new SoundEventData(mapLayer, player, "drop", new Vector2(100, 180))
            .SetMessage("Something heavy landed near your feet")
            .SetRepeat(false)
            .SetEventType(SoundEventData.EventType.Activate)
            .SetActionMessage("You pick up a small gun")
            .SetIsGun(true)

            ))));

            foreach (SoundEventData levelItem in levelData)
            {
                SoundActor sa = SoundActor.CreateActor(levelItem);
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