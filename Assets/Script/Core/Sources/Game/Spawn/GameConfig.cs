using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Game/" + nameof(GameConfig), fileName = nameof(GameConfig))]
    internal sealed class GameConfig : ScriptableObject
    {
        [SerializeField] private PlayerConfig _playerConfig;

        public PlayerConfig PlayerUnit => _playerConfig;
    }
}