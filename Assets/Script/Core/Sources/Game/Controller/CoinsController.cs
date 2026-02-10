using System;
using ECS;
using Infrastructure;
using Navigation;
using UnityEngine;

namespace Core
{
    internal sealed class CoinsController : ICoinsCreator
    {
        private readonly IPoolService _poolService;
        private readonly IRandomizer _randomizer;
        private readonly INavigationGridService _navService;
        private bool _started;
        private CoinsConfig _config;
        
        public event Action<ICoin, int, float> OnCoinCreated;

        public CoinsController(IPoolService poolService, IRandomizer randomizer, INavigationGridService navService)
        {
            _poolService = poolService;
            _randomizer = randomizer;
            _navService = navService;
        }

        public void Start(CoinsConfig config)
        {
            _started = true;
            _config = config;
        }

        public void RequestCoin(Vector3 position)
        {
            if (!_started)
                return;
            if (_randomizer.GetRandom() < _config.DropChance)
                return;
            position = _navService.GetNearestPassible(NavigationGridType.Humanoid, position);
            var instance = _poolService.Instantiate<CoinView>(_config.Prefab.gameObject, position, Quaternion.identity);
            OnCoinCreated?.Invoke(new Coin(_poolService, instance), _randomizer.GetRandom(_config.ValueRange.x, _config.ValueRange.y),
                _config.PickUpDistance);
        }

        private sealed class Coin : ICoin
        {
            private readonly IPoolService _poolService;
            private readonly CoinView _view;
            public Transform Transform => _view.transform;

            public Coin(IPoolService poolService, CoinView view)
            {
                _poolService = poolService;
                _view = view;
            }

            public void Destroy()
            {
                _poolService.Destroy(_view.gameObject);
            }
        }
    }
}