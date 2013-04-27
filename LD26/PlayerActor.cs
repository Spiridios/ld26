using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Spiridios.SpiridiEngine;
using Spiridios.SpiridiEngine.Input;
using Spiridios.SpiridiEngine.Physics;

namespace Spiridios.LD26
{
    public class PlayerBehavior : Behavior, CollisionListener
    {
        private const double WALK_SPEED = 20;
        private const double ROTATION_SPEED = 1;
        private InputManager inputManager;
        private Vector2 previousPosition = Vector2.Zero;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enemies">Reference to the enemies list.</param>
        public PlayerBehavior(Actor actor, InputManager inputManager)
            : base(actor)
        {
            this.inputManager = inputManager;
            previousPosition = Actor.Position;
        }

        public override void Update(System.TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            previousPosition = Actor.Position;
            Vector2 p = Actor.Position;
            if (inputManager.IsActive("walkNorth"))
            {
                p.Y -= (float)(WALK_SPEED * elapsedTime.TotalSeconds);
            }
            if (inputManager.IsActive("walkSouth"))
            {
                p.Y += (float)(WALK_SPEED * elapsedTime.TotalSeconds);
            }

            if (inputManager.IsActive("walkEast"))
            {
                p.X += (float)(WALK_SPEED * elapsedTime.TotalSeconds);
            }
            if (inputManager.IsActive("walkWest"))
            {
                p.X -= (float)(WALK_SPEED * elapsedTime.TotalSeconds);
            }

            if (inputManager.IsActive("rotateLeft"))
            {
                Actor.Rotation -= (float)(ROTATION_SPEED * elapsedTime.TotalSeconds);
            }
            if (inputManager.IsActive("rotateRight"))
            {
                Actor.Rotation += (float)(ROTATION_SPEED * elapsedTime.TotalSeconds);
            }

            Actor.Position = p;
        }

        public void OnCollided(List<Collidable> activeCollidables)
        {
            Actor.Position = previousPosition;
        }
    }

    public class PlayerActor : Actor
    {
        private AudioListener listener;
        private readonly Vector2 FORWARD = new Vector2(0, 1);
        private readonly Vector3 UP = new Vector3(0, 0, 1);

        public PlayerActor(InputManager inputManager, AudioListener listener)
            : base("Player")
        {
            this.listener = listener;
            listener.Up = UP;
            this.Position = new Vector2(10, 10);
            PlayerBehavior pb = new PlayerBehavior(this, inputManager);
            this.SetBehavior(LifeStage.ALIVE, pb);
            this.Collidable.AddCollisionListener(pb);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            listener.Position = new Vector3(this.Position, 1);
            listener.Forward = new Vector3(Vector2Ext.Rotate(FORWARD, this.Rotation), 0);

        }
    }
}
