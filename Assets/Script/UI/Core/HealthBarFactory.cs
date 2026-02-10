using Infrastructure;
using UnityEngine;

namespace UI
{
    internal sealed class HealthBarFactory : IHealthBarFactory
    {
        private readonly HealthBarView _prefab;
        private readonly IPoolService _poolService;

        public HealthBarFactory(HealthBarView prefab, IPoolService poolService)
        {
            _prefab = prefab;
            _poolService = poolService;
        }

        public IHealthBarView CreateHealthBar()
        {
            return new ViewProvider(_poolService, _poolService.Instantiate<HealthBarView>(_prefab.gameObject, Vector3.zero, Quaternion.identity));
        }

        private sealed class ViewProvider : IHealthBarView
        {
            private readonly IPoolService _poolService;
            private readonly HealthBarView _view;
            public Transform Transform => _view.transform;

            public ViewProvider(IPoolService poolService, HealthBarView view)
            {
                _poolService = poolService;
                _view = view;
            }

            public void UpdateHealth(float current, float max)
            {
                _view.UpdateHealth(current, max);
            }

            public void Destroy()
            {
                _poolService.Destroy(_view.gameObject);
            }
        }
    }
}