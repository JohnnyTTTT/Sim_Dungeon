using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public static class RandomUtility
    {
        private static int s_Seed = 0;
        private static System.Random s_Rng;

        public static void SetSeed(int seed)
        {
            s_Seed = seed;
            s_Rng = new System.Random(s_Seed);
        }

        private static float NextFloat()
        {
            return (float)s_Rng.NextDouble();
        }

        public static float GetRandomFloat(IntVector2 coord, float max)
        {
            if (max < 0f)
                throw new ArgumentOutOfRangeException(nameof(max), "max must be >= 0");

            var hash = s_Seed;
            hash = HashCombine(hash, coord.x.GetHashCode());
            hash = HashCombine(hash, coord.y.GetHashCode());

            var rng = new System.Random(hash);

            return (float)rng.NextDouble() * max;
        }

        public static int GetRandomInt(IntVector2 coord, int max)
        {
            if (max < 0)
                throw new ArgumentOutOfRangeException(nameof(max), "max must be >= 0");

            var hash = s_Seed;
            hash = HashCombine(hash, coord.x.GetHashCode());
            hash = HashCombine(hash, coord.y.GetHashCode());

            var rng = new System.Random(hash);

            return rng.Next(max + 1);
        }

        public static Quaternion GetRandomDirection(IntVector2 coord)
        {
            var dirIndex = GetRandomInt(coord, 4);

            Vector3 dir = Vector3.forward;
            switch (dirIndex)
            {
                case 0: dir = DirectionUtility.dirUp; break;
                case 1: dir = DirectionUtility.dirDown; break;
                case 2: dir = DirectionUtility.dirRight; break;
                case 3: dir = DirectionUtility.dirLeft; break;
            }

            return Quaternion.LookRotation(dir, Vector3.up);
        }

        private static int HashCombine(int h1, int h2)
        {
            unchecked
            {
                return ((h1 << 5) + h1) ^ h2;
            }
        }

        public static BuildableObjectSO.RandomPrefabs UpdateBuildableObjectSORandomPrefab(IntVector2 coord, BuildableObjectSO buildableObjectSO)
        {
            var totalProbability = 0f;
            foreach (var randomPrefab in buildableObjectSO.randomPrefabs)
            {
                totalProbability += randomPrefab.probability;
            }
            var randomPoint = GetRandomFloat(coord, totalProbability);

            var currentProbability = 0f;
            foreach (var randomPrefab in buildableObjectSO.randomPrefabs)
            {
                currentProbability += randomPrefab.probability;
                if (randomPoint <= currentProbability) return randomPrefab;
            }
            return null;
        }

        public static FourDirectionalRotation GetRandomFourDirectionalRotation(IntVector2 coord)
        {
            var values = System.Enum.GetValues(typeof(FourDirectionalRotation));
            var index = GetRandomInt(coord,values.Length);
            return (FourDirectionalRotation)values.GetValue(index);
        }

        public static T GetRandomElement<T>(IntVector2 coord, T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new System.Exception("数组为空，无法随机取值");
            }

            var index = GetRandomInt(coord, array.Length-1);
            return array[index];
        }


    }
}
