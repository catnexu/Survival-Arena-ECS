using UnityEngine;

namespace ECS
{
    public interface ICoin
    {
        Transform Transform { get; }
        void Destroy();
    }
}