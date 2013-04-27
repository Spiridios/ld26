using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        public PlayGameState(SpiridiGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            gameMap = new Scene(game);
            gameMap.LoadTiledMap("Map.tmx");
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
        }

    }


}