using UnityEngine;

namespace Unit
{
    [RequireComponent(typeof(Rigidbody))]
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private GameObject[] _childToSetLayer;
        public void Initialize(int layer)
        {
            for (int i = 0; i < _childToSetLayer.Length; i++)
            {
                _childToSetLayer[i].layer = layer;
            }
        }
    }
}