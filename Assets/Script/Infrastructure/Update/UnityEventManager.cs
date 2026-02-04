using System;

namespace Infrastructure
{
    internal sealed class UnityEventManager
    {
        public enum EventType
        {
            Update = 0,
            FixedUpdate = 1,
            LateUpdate = 2,
        }

        private readonly UnityEventHolder _eventHolder = new();

        public void Dispose()
        {
            _eventHolder.Dispose();
        }

        public IDisposable Subscribe(EventType type, Action action)
        {
            switch (type)
            {
                case EventType.Update:
                    _eventHolder.OnUpdate += action;
                    break;
                case EventType.FixedUpdate:
                    _eventHolder.OnFixedUpdate += action;
                    break;
                case EventType.LateUpdate:
                    _eventHolder.OnLateUpdate += action;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return new DisposableActionHolder(() => { Unsubscribe(type, action); });
        }

        private void Unsubscribe(EventType eventTypeType, Action action)
        {
            switch (eventTypeType)
            {
                case EventType.Update:
                    _eventHolder.OnUpdate -= action;
                    break;
                case EventType.FixedUpdate:
                    _eventHolder.OnFixedUpdate -= action;
                    break;
                case EventType.LateUpdate:
                    _eventHolder.OnLateUpdate -= action;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventTypeType), eventTypeType, null);
            }
        }
    }
}