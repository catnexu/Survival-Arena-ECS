using UnityEngine;

namespace Unit
{
    public abstract class UnitConfig : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        public GameObject Prefab => _prefab;
        public abstract UnitType Type { get; }
    }
}