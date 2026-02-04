using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
    public interface IRandomizer
    {
        int GetRandom(int minInclusive, int maxExclusive);
        float GetRandom(float minInclusive, float maxInclusive);
        float GetRandom();
        bool GetRandomBool();
        void SetSeed(string seedString);
        Vector2 OnUnitCircle();
        Vector2 InUnitCircle();
        Vector3 OnUnitSphere();
        Vector3 InUnitSphere();
    }
}