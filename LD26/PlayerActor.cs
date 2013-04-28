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
    public class PlayerBehavior : Behavior, CollisionListener
    {
        private const double WALK_SPEED = 20;
        private const double ROTATION_SPEED = 1.5;
        private InputManager inputManager;
        private Vector2 previousPosition = Vector2.Zero;

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
            foreach (Collidable collidabe in activeCollidables)
            {
                if (!String.IsNullOrWhiteSpace(collidabe.Tag))
                {
                    if (collidabe.Tag == TileMapLayer.COLLIDABLE_TAG)
                    {
                        ((LD26)SpiridiGame.Instance).DisplayMessage("You feel a wall and stop moving");
                        Actor.Position = previousPosition;
                    }
                    else
                    {
                        ((LD26)SpiridiGame.Instance).DisplayMessage(collidabe.Tag);
                    }
                }
                else
                {
                    ((LD26)SpiridiGame.Instance).DisplayMessage("You feel something strange under your feet");
                }

            }
        }
    }

    public class PlayerActor : Actor
    {
        public PlayerActor(InputManager inputManager)
            : base("Player")
        {
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
