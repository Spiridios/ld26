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
        protected SoundEventData eventInfo;

        public SoundBehavior(Actor actor, SoundEventData eventInfo)
            : base(actor)
        {
            this.eventInfo = eventInfo;
        }

        public virtual void OnCollided(List<Collidable> activeCollidables)
        {
        }

        public virtual bool DoStuff() { return false; }
    }

    public class ActionSoundBehavior : SoundBehavior
    {
        public ActionSoundBehavior(Actor actor, SoundEventData eventInfo)
            : base(actor, eventInfo)
        {
        }

        public override void Update(System.TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public override void OnCollided(List<Collidable> activeCollidables)
        {
            base.OnCollided(activeCollidables);
        }

        public override bool DoStuff()
        {
            base.DoStuff();
            this.Actor.lifeStage = Actor.LifeStage.DYING;

            if (eventInfo.repeat)
            {
                ((SoundActor)this.Actor).Sound.Stop();
            }

            if (!String.IsNullOrEmpty(eventInfo.actionMessage))
            {
                LD26.DisplayMessage(eventInfo.actionMessage);
            }
            else
            {
                LD26.DisplayMessage("You did something");
            }
            return true;
        }
    }

    public class CollisionSoundBehavior : SoundBehavior
    {
        public CollisionSoundBehavior(Actor actor, SoundEventData eventInfo)
            : base(actor, eventInfo)
        {
        }

        public override void Update(System.TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public override void OnCollided(List<Collidable> activeCollidables)
        {
            base.OnCollided(activeCollidables);
            if (eventInfo.repeat)
            {
                ((SoundActor)this.Actor).Sound.Stop();
            }
            this.Actor.lifeStage = Actor.LifeStage.DYING;
        }
    }

    public class IntruderSoundBehavior : SoundBehavior
    {
        public IntruderSoundBehavior(Actor actor, SoundEventData eventInfo)
            : base(actor, eventInfo)
        {
        }

        public override void Update(System.TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public override void OnCollided(List<Collidable> activeCollidables)
        {
            base.OnCollided(activeCollidables);
            if (eventInfo.repeat)
            {
                ((SoundActor)this.Actor).Sound.Stop();
            }
            this.Actor.lifeStage = Actor.LifeStage.DYING;
            foreach (Collidable collidable in activeCollidables)
            {
                if (collidable.Tag == BulletBehavior.COLLIDABLE_TAG)
                {
                    ((LD26)SpiridiGame.Instance).IntruderDead = true;
                }
            }
        }
    }

    public class SoundActor : Actor
    {
        private SoundEffect testEffect;
        private PositionedSound positionedEffect;
        private SoundEventData eventInfo;
        private bool isGun = false;

        public static SoundActor CreateActor(SoundEventData soundEvent)
        {
            SoundActor sa = new SoundActor(soundEvent.soundName);
            sa.eventInfo = soundEvent;
            sa.Position = soundEvent.position;
            soundEvent.mapLayer.AddActor(sa);
            sa.Sound.Listener = soundEvent.player;

            if (soundEvent.eventType == SoundEventData.EventType.Activate)
            {
                ActionSoundBehavior pb = new ActionSoundBehavior(sa, soundEvent);
                sa.SetBehavior(LifeStage.ALIVE, pb);
                sa.Collidable.AddCollisionListener(pb);
            }
            else if (soundEvent.eventType == SoundEventData.EventType.Collision)
            {
                CollisionSoundBehavior pb = new CollisionSoundBehavior(sa, soundEvent);
                sa.SetBehavior(LifeStage.ALIVE, pb);
                sa.Collidable.AddCollisionListener(pb);
            }
            else if (soundEvent.eventType == SoundEventData.EventType.Intruder)
            {
                IntruderSoundBehavior pb = new IntruderSoundBehavior(sa, soundEvent);
                sa.SetBehavior(LifeStage.ALIVE, pb);
                sa.Collidable.AddCollisionListener(pb);
            }
            else if (soundEvent.eventType == SoundEventData.EventType.OneShot)
            {
                SoundBehavior pb = new SoundBehavior(sa, soundEvent);
                sa.SetBehavior(LifeStage.ALIVE, pb);
                sa.lifeStage = LifeStage.DYING;
            }

            if (soundEvent.repeat)
            {
                sa.Sound.PlayLooped();
            }
            else
            {
                sa.Sound.Play();
            }

            sa.isGun = soundEvent.isGun;
            return sa;
        }

        public SoundActor(String sound)
            : base("Sound")
        {
            testEffect = SpiridiGame.Instance.Content.Load<SoundEffect>(sound);
            this.positionedEffect = new PositionedSound(testEffect);
            this.positionedEffect.AttenuationFactor = 0.1f;
            this.positionedEffect.Position = this.Position;
        }

        public string Message
        {
            get { return eventInfo.message; }
        }

        public bool IsGun
        {
            get { return this.isGun; }
        }

        /// <summary>
        /// The generic "player is trying to interact with us" method.
        /// </summary>
        public bool DoStuff()
        {
            if (this.IsALive)
            {
                return ((SoundBehavior)this.CurrentBehavior).DoStuff();
            }
            else
            {
                return false;
            }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            positionedEffect.Update(elapsedTime);
            if (lifeStage == LifeStage.DYING && !positionedEffect.IsPlaying)
            {
                lifeStage = LifeStage.DEAD;
                HandleNextEvent();
            }
        }

        public void HandleNextEvent()
        {
            foreach(SoundEventData soundData in eventInfo.nextEvents)
            {
                SoundActor sa = SoundActor.CreateActor(soundData);
            }
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
}
