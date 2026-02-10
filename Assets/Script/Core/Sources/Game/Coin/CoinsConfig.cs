using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    [Serializable]
    internal struct CoinsConfig
    {
        [FormerlySerializedAs("_coinPrefab"),SerializeField] private CoinView _prefab;
        [SerializeField] private float _dropChance;
        [SerializeField] private Vector2Int _valueRange;
        [SerializeField] private float _pickUpDistance;

        public CoinView Prefab => _prefab;
        public float DropChance => _dropChance;
        public Vector2Int ValueRange => _valueRange;
        public float PickUpDistance => _pickUpDistance;
    }
}