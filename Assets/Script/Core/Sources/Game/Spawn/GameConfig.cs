using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Game/" + nameof(GameConfig), fileName = nameof(GameConfig))]
    internal sealed class GameConfig : ScriptableObject
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemySpawnConfig _enemySpawnConfig;
        [SerializeField] private CoinsConfig _coinsConfig;
        public PlayerConfig Player => _playerConfig;
        public EnemySpawnConfig EnemySpawn => _enemySpawnConfig;
        public CoinsConfig Coins => _coinsConfig;
    }
}