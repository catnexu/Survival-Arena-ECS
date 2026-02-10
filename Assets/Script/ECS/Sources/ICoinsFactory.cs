using System;
using UnityEngine;

namespace ECS
{
    public interface ICoinsFactory
    {
        event Action<ICoin, int, float> OnCoinCreated;
        void RequestCoin(Vector3 position);
    }
}