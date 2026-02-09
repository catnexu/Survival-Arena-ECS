using UnityEngine;

namespace Unit
{
    [CreateAssetMenu(menuName = "Unit/" + nameof(UnitConfig), fileName = nameof(UnitConfig))]
    public abstract class UnitConfig : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        public GameObject Prefab => _prefab;
        public abstract UnitType Type { get; }
    }
}