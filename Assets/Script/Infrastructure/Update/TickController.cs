using System;
using R3;

namespace Infrastructure
{
    internal sealed class TickController : ITickController, IDisposable
    {
        private readonly UnityEventManager _eventManager;
        private readonly Controllers _controllers;
        private readonly CompositeDisposable _compositeDisposable;
        private float _secondElapsedTime;
        private float _currentTime;

        public ITimeManager TimeManager { get; }

        public TickController(ITimeManager timeManager)
        {
            TimeManager = timeManager;
            _eventManager = new UnityEventManager();
            _controllers = new Controllers(32);
            _compositeDisposable = new CompositeDisposable();
            _compositeDisposable.Add(_eventManager.Subscribe(UnityEventManager.EventType.Update, Update));
            _compositeDisposable.Add(_eventManager.Subscribe(UnityEventManager.EventType.FixedUpdate, FixedUpdate));
            _compositeDisposable.Add(_eventManager.Subscribe(UnityEventManager.EventType.LateUpdate, LateUpdate));
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _controllers.Destroy();
            _eventManager.Dispose();
        }

        public IDisposable AddController(IController controller)
        {
            _controllers.Add(controller);
            return new DisposableActionHolder(() => RemoveController(controller));
        }

        private void RemoveController(IController controller) => _controllers.Remove(controller);

        private void Update()
        {
            UpdateControllers();
            UnscaledUpdateControllers();
        }

        private void LateUpdate()
        {
            LateUpdateControllers();
        }

        private void FixedUpdate()
        {
            _secondElapsedTime += TimeManager.FixedDeltaTime;
            if (_secondElapsedTime >= 1f)
            {
                _secondElapsedTime -= 1f;
                EachSecondUpdateControllers();
            }

            FixedUpdateControllers();
        }

        private void UpdateControllers() => _controllers.Update(TimeManager.DeltaTime);

        private void UnscaledUpdateControllers() => _controllers.UnscaledUpdate(TimeManager.UnscaledDeltaTime);

        private void FixedUpdateControllers() => _controllers.FixedUpdate(TimeManager.FixedDeltaTime);

        private void LateUpdateControllers() => _controllers.LateUpdate(TimeManager.DeltaTime);

        private void EachSecondUpdateControllers() => _controllers.EachSecondUpdate();
    }
}