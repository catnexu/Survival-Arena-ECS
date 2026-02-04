using UnityEngine;

namespace Infrastructure
{
    internal sealed class TimeManager : ITimeManager
    {
        public float DeltaTime => Time.deltaTime;
        public float UnscaledDeltaTime => Time.unscaledDeltaTime;
        public float FixedDeltaTime => Time.fixedDeltaTime;
        public float CurrentTime => Time.time;
        public float UnscaledTime => Time.unscaledTime;
    }
}