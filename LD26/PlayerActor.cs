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
    public class PlayerDyingBehavior : Behavior
    {
        public PlayerDyingBehavior(Actor actor)
            : base(actor)
        {
        }

        public override void Update(System.TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            SoundManager.Instance.PlaySound("gunshot");
            Actor.lifeStage = Actor.LifeStage.DEAD;
        }
    }

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

            if (((PlayerActor)Actor).HasGun && ((PlayerActor)Actor).WantToShoot)
            {

                Vector2 vel = Vector2Ext.Rotate(new Vector2(0, -100), Actor.Rotation);

                Actor bullet = BulletBehavior.CreateBullet("Bullet", this.Actor.Position, vel, "gunshot", SpiridiGame.Instance);
                ((PlayerActor)Actor).MapLayer.AddActor(bullet);

                ((PlayerActor)Actor).WantToShoot = false;
                ((PlayerActor)Actor).ClipCount--;

                if (((PlayerActor)Actor).ClipCount < 0)
                {
                    Actor.lifeStage = Actor.LifeStage.DYING;
                }
            }
            else if (((PlayerActor)Actor).HasGun && inputManager.IsTriggered("doStuff"))
            {
                ((PlayerActor)Actor).WantToShoot = true;
            }

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
                if (collidabe.Tag == TileMapLayer.COLLIDABLE_TAG)
                {
                    LD26.DisplayMessage("You feel a wall and stop moving");
                    Actor.Position = previousPosition;
                }
                else if (collidabe.Tag == Actor.COLLIDABLE_TAG)
                {
                    // TODO: this really seems like it belongs in SoundActor.
                    SoundActor sa = (SoundActor)collidabe.Owner;
                    string message = sa.Message;
                    if (!String.IsNullOrWhiteSpace(message))
                    {
                        LD26.DisplayMessage(sa.Message);
                    }
                    else
                    {
                        LD26.DisplayMessage("You bump into something strange");
                    }

                    if (inputManager.IsTriggered("doStuff"))
                    {
                        bool didStuff = sa.DoStuff();
                        if (didStuff && ((PlayerActor)Actor).WantToShoot)
                        {
                            ((PlayerActor)Actor).WantToShoot = false;
                        }

                        if (sa.IsGun)
                        {
                            ((PlayerActor)Actor).HasGun = true;
                        }

                    }
                }
                else
                {
                    //LD26.DisplayMessage("You feel something strange under your feet");
                }

            }
        }
    }

    public class PlayerActor : Actor
    {
        public bool HasGun { get; set; }
        public bool WantToShoot { get; set; }
        public int ClipCount { get; set; }
        public SceneLayer MapLayer { get; set; }

        public PlayerActor(InputManager inputManager, SceneLayer mapLayer)
            : base("Player")
        {
            this.MapLayer = mapLayer;
            this.HasGun = false;
            this.WantToShoot = false;
            this.ClipCount = 4;
            this.Position = new Vector2(10, 10);
            PlayerBehavior pb = new PlayerBehavior(this, inputManager);
            this.SetBehavior(LifeStage.ALIVE, pb);
            this.Collidable.AddCollisionListener(pb);

            this.SetBehavior(LifeStage.DYING, new PlayerDyingBehavior(this));

        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }
    }
}
