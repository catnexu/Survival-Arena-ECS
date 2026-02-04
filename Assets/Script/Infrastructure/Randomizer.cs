using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Infrastructure
{
    internal sealed class Randomizer : IRandomizer
    {
        private Random _random = new();

        public void SetSeed(string seedString)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] hash = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(seedString));
            int computedSeed = BitConverter.ToInt32(hash, 0);
            _random = new Random(computedSeed);
        }

        public int GetRandom(int minInclusive, int maxExclusive) => _random.Next(minInclusive, maxExclusive);

        public float GetRandom(float minInclusive, float maxInclusive)
        {
            return (float) (minInclusive + (maxInclusive - minInclusive) * _random.NextDouble());
        }

        public float GetRandom() => (float) _random.NextDouble();

        public bool GetRandomBool() => GetRandom(0, 2) == 0;

        public T GetRandom<T>(IReadOnlyList<T> list) => list[GetRandom(0, list.Count)];

        public Vector2 OnUnitCircle() => InUnitCircle().normalized;
        public Vector2 InUnitCircle() => new(GetRandom(-1f, 1f), GetRandom(-1f, 1f));

        public Vector3 OnUnitSphere() => InUnitSphere().normalized;
        public Vector3 InUnitSphere() => new(GetRandom(-1f, 1f), GetRandom(-1f, 1f), GetRandom(-1f, 1f));
    }
}