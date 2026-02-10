using UnityEngine;

namespace UI
{
    public interface IHealthBarView
    {
        Transform Transform { get; }
        void UpdateHealth(float current, float max);
        void Destroy();
    }
}