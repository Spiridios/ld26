using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Spiridios.SpiridiEngine.Scene;

namespace Spiridios.LD26
{
    public class SoundEventData
    {
        public enum EventType
        {
            Collision, Activate, OneShot, Intruder
        };

        public SoundEventData(SceneLayer mapLayer, PlayerActor player, string soundName, Vector2 position)
        {
            this.mapLayer = mapLayer;
            this.player = player;
            this.soundName = soundName;
            this.position = position;
            this.eventType = EventType.Collision;
        }

        public SoundEventData SetMessage(string message)
        {
            this.message = message;
            return this;
        }

        public SoundEventData SetActionMessage(string message)
        {
            this.actionMessage = message;
            return this;
        }

        public SoundEventData SetRepeat(bool repeat)
        {
            this.repeat = repeat;
            return this;
        }

        public SoundEventData AddNextEvent(SoundEventData nextEvent)
        {
            this.nextEvents.Add(nextEvent);
            return this;
        }

        public SoundEventData SetEventType(EventType eventType)
        {
            this.eventType = eventType;
            return this;
        }

        public SoundEventData SetIsGun(bool isGun)
        {
            this.isGun = isGun;
            return this;
        }

        internal EventType eventType;
        internal SceneLayer mapLayer;
        internal PlayerActor player;
        internal string soundName;
        internal Vector2 position;
        internal string message;
        internal string actionMessage;
        internal bool repeat = true;
        internal List<SoundEventData> nextEvents = new List<SoundEventData>();
        internal bool isGun = false;
    }
}
