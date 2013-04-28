using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Spiridios.SpiridiEngine;
using Spiridios.SpiridiEngine.Audio;
using Spiridios.SpiridiEngine.Input;
using Spiridios.SpiridiEngine.Physics;
using Spiridios.SpiridiEngine.Scene;

namespace Spiridios.LD26
{
    public class SoundBehavior : Behavior, CollisionListener
    {
        public SoundBehavior(Actor actor)
            : base(actor)
        {
        }

        public override void Update(System.TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public void OnCollided(List<Collidable> activeCollidables)
        {
        }
    }

    public class SoundActor : Actor
    {
        private SoundEffect testEffect;
        private PositionedSound positionedEffect;
        private string message;

        public static SoundActor CreateActor(LevelData levelData, SceneLayer mapLayer, PlayerActor player)
        {
            SoundActor sa = new SoundActor(levelData.soundName, levelData.message);
            sa.Position = levelData.position;
            mapLayer.AddActor(sa);
            sa.Sound.Listener = player;

            return sa;
        }

        public SoundActor(String sound, string message)
            : base("Sound")
        {
            this.message = message;
            testEffect = SpiridiGame.Instance.Content.Load<SoundEffect>("drip");
            this.positionedEffect = new PositionedSound(testEffect);
            this.positionedEffect.AttenuationFactor = 0.1f;
            this.positionedEffect.Position = this.Position;

            SoundBehavior pb = new SoundBehavior(this);
            this.SetBehavior(LifeStage.ALIVE, pb);
            this.Collidable.AddCollisionListener(pb);
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// The generic "player is trying to interact with us" method.
        /// </summary>
        public void DoStuff()
        {
            lifeStage = Actor.LifeStage.DEAD;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            positionedEffect.Update(elapsedTime);
        }

        public PositionedSound Sound
        {
            get { return this.positionedEffect; }
        }

        public override Camera Camera
        {
            get { return base.Camera; }
            set
            {
                base.Camera = value;
                if (this.positionedEffect != null)
                {
                    this.positionedEffect.Camera = value;
                }
            }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                if (this.positionedEffect != null)
                {
                    this.positionedEffect.Position = value;
                }
            }
        }
    }

    public class LevelData
    {
        public LevelData(string soundName, Vector2 position, string message, string action, LevelData triggers)
        {
            this.soundName = soundName;
            this.position = position;
            this.message = message;
            this.action = action;
            this.triggers = triggers;
        }

        internal string soundName;
        internal Vector2 position;
        internal string message;
        internal string action;
        internal LevelData triggers;
    }
}
