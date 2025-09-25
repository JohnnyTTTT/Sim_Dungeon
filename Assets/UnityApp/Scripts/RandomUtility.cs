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

        public static float GetRandomFloat(Vector2Int coord, float max)
        {
            if (max < 0f)
                throw new ArgumentOutOfRangeException(nameof(max), "max must be >= 0");

            var hash = s_Seed;
            hash = HashCombine(hash, coord.x.GetHashCode());
            hash = HashCombine(hash, coord.y.GetHashCode());

            var rng = new System.Random(hash);

            return (float)rng.NextDouble() * max;
        }

        public static int GetRandomInt(Vector2Int coord, int max)
        {
            if (max < 0)
                throw new ArgumentOutOfRangeException(nameof(max), "max must be >= 0");

            var hash = s_Seed;
            hash = HashCombine(hash, coord.x.GetHashCode());
            hash = HashCombine(hash, coord.y.GetHashCode());

            var rng = new System.Random(hash);

            return rng.Next(max + 1);
        }

        public static Direction GetRandomDirection(Vector2Int coord)
        {
            int index = GetRandomInt(coord, 4);

            switch (index)
            {
                case 0: return Direction.Up;
                case 1: return Direction.Down;
                case 2: return Direction.Right;
                case 3: return Direction.Left;
            }
            return Direction.Up;
        }

        public static FourDirectionalRotation GetRandomFourDirectionalRotation(Vector2Int coord)
        {
            var values = System.Enum.GetValues(typeof(FourDirectionalRotation));
            var index = GetRandomInt(coord, values.Length);
            return (FourDirectionalRotation)values.GetValue(index);
        }

        public static Quaternion GetRandomRotation(Vector2Int coord)
        {
            // 用坐标作为随机种子，确保同一个格子每次结果一致
            var seed = coord.x * 73856093 ^ coord.y * 19349663;
            var random = new System.Random(seed);

            // 四个方向：0=北，1=东，2=南，3=西
            var index = random.Next(0, 4);

            switch (index)
            {
                case 0: return Quaternion.LookRotation(Vector3.forward);   // 北
                case 1: return Quaternion.LookRotation(Vector3.right);     // 东
                case 2: return Quaternion.LookRotation(Vector3.back);      // 南
                case 3: return Quaternion.LookRotation(Vector3.left);      // 西
            }

            return Quaternion.identity;
        }

        private static int HashCombine(int h1, int h2)
        {
            unchecked
            {
                return ((h1 << 5) + h1) ^ h2;
            }
        }

        public static BuildableObjectSO.RandomPrefabs UpdateBuildableObjectSORandomPrefab(Vector2Int coord, BuildableObjectSO buildableObjectSO)
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



        public static T GetRandomElement<T>(Vector2Int coord, T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new System.Exception("数组为空，无法随机取值");
            }

            var index = GetRandomInt(coord, array.Length - 1);
            return array[index];
        }


    }
}
