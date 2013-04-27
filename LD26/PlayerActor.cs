using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Spiridios.SpiridiEngine;
using Spiridios.SpiridiEngine.Audio;
using Spiridios.SpiridiEngine.Input;
using Spiridios.SpiridiEngine.Physics;

namespace Spiridios.LD26
{
    public class PlayerBehavior : Behavior, CollisionListener
    {
        private const double WALK_SPEED = 20;
        private const double ROTATION_SPEED = 1.5;
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
            Vector2 walkVector = Vector2.Zero;

            if (inputManager.IsActive("walkForward"))
            {
                walkVector.Y -= (float)(WALK_SPEED * elapsedTime.TotalSeconds);
            }
            if (inputManager.IsActive("walkBackward"))
            {
                walkVector.Y += (float)(WALK_SPEED * elapsedTime.TotalSeconds);
            }

            if (inputManager.IsActive("walkRight"))
            {
                walkVector.X += (float)(WALK_SPEED * elapsedTime.TotalSeconds);
            }
            if (inputManager.IsActive("walkLeft"))
            {
                walkVector.X -= (float)(WALK_SPEED * elapsedTime.TotalSeconds);
            }

            p = p + Vector2Ext.Rotate(walkVector, Actor.Rotation);

            if (inputManager.IsActive("rotateLeft"))
            {
                Actor.Rotation -= (float)(ROTATION_SPEED * elapsedTime.TotalSeconds);
            }
            if (inputManager.IsActive("rotateRight"))
            {
                Actor.Rotation += (float)(ROTATION_SPEED * elapsedTime.TotalSeconds);
            }
            if (Actor.Rotation > MathHelper.Pi)
            {
                Actor.Rotation -= MathHelper.TwoPi;
            }
            else if (Actor.Rotation < -MathHelper.Pi)
            {
                Actor.Rotation += MathHelper.TwoPi;
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
        private PositionedSound listener;

        public PlayerActor(InputManager inputManager, PositionedSound listener)
            : base("Player")
        {
            this.listener = listener;
            this.Position = new Vector2(10, 10);
            PlayerBehavior pb = new PlayerBehavior(this, inputManager);
            this.SetBehavior(LifeStage.ALIVE, pb);
            this.Collidable.AddCollisionListener(pb);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }
    }
}
