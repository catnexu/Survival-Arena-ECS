using System.Collections.Generic;

namespace Infrastructure
{
    public sealed class Controllers
    {
        private readonly List<IUpdate> _updateControllers;
        private readonly List<IUnscaledUpdate> _unscaledUpdateControllers;
        private readonly List<IFixedUpdate> _fixedControllers;
        private readonly List<ILateUpdate> _lateControllers;
        private readonly List<IEachSecondUpdate> _eachSecondControllers;

        public Controllers(int capacity = 16)
        {
            _updateControllers = new List<IUpdate>(capacity);
            _unscaledUpdateControllers = new List<IUnscaledUpdate>(capacity);
            _fixedControllers = new List<IFixedUpdate>(capacity);
            _lateControllers = new List<ILateUpdate>(capacity);
            _eachSecondControllers = new List<IEachSecondUpdate>(capacity);
        }

        public void Add(IController controller)
        {
            if (controller == null)
                return;
            _updateControllers.TryAdd(controller);
            _unscaledUpdateControllers.TryAdd(controller);
            _fixedControllers.TryAdd(controller);
            _lateControllers.TryAdd(controller);
            _eachSecondControllers.TryAdd(controller);
        }

        public void Remove(IController controller)
        {
            if (controller == null)
                return;
            _updateControllers.TryRemove(controller);
            _unscaledUpdateControllers.TryRemove(controller);
            _fixedControllers.TryRemove(controller);
            _lateControllers.TryRemove(controller);
            _eachSecondControllers.TryRemove(controller);
        }

        public void Update(float deltaTime)
        {
            for (int i = _updateControllers.Count - 1; i >= 0; i--)
            {
                _updateControllers[i].UpdateController(deltaTime);
            }
        }

        public void UnscaledUpdate(float unscaledDeltaTime)
        {
            for (int i = _unscaledUpdateControllers.Count - 1; i >= 0; i--)
            {
                _unscaledUpdateControllers[i].UpdateController(unscaledDeltaTime);
            }
        }
        

        public void FixedUpdate(float deltaTime)
        {
            for (int i = _fixedControllers.Count - 1; i >= 0; i--)
            {
                _fixedControllers[i].UpdateController(deltaTime);
            }
        }

        public void LateUpdate(float deltaTime)
        {
            for (int i = _lateControllers.Count - 1; i >= 0; i--)
            {
                _lateControllers[i].UpdateController(deltaTime);
            }
        }
        
        public void EachSecondUpdate()
        {
            for (int i = _eachSecondControllers.Count - 1; i >= 0; i--)
            {
                _eachSecondControllers[i].UpdateController();
            }
        }

        public void Destroy()
        {
            _updateControllers.Clear();
            _unscaledUpdateControllers.Clear();
            _fixedControllers.Clear();
            _lateControllers.Clear();
            _eachSecondControllers.Clear();
        }
    }
}