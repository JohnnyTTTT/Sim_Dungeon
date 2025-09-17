using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public static class RandomUtility
    {
        private static System.Random s_Rng;

        public static void SetSeed(int seed)
        {
            var intSeed = unchecked(seed);
            s_Rng = new System.Random(intSeed);
        }

        public static float NextFloat()
        {
            return (float)s_Rng.NextDouble();
        }

        public static float GetRandomFloat(float max)
        {
            return (float)(s_Rng.NextDouble() * max);
        }

        public static BuildableObjectSO.RandomPrefabs UpdateBuildableObjectSORandomPrefab(BuildableObjectSO buildableObjectSO)
        {
            var totalProbability = 0f;
            foreach (var randomPrefab in buildableObjectSO.randomPrefabs)
            {
                totalProbability += randomPrefab.probability;
            }
            var randomPoint = GetRandomFloat(totalProbability);

            var currentProbability = 0f;
            foreach (var randomPrefab in buildableObjectSO.randomPrefabs)
            {
                currentProbability += randomPrefab.probability;
                if (randomPoint <= currentProbability) return randomPrefab;
            }
            return null;
        }

        public static FourDirectionalRotation GetRandomFourDirectionalRotation()
        {
            var values = System.Enum.GetValues(typeof(FourDirectionalRotation));
            return (FourDirectionalRotation)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }

        public static bool Chance(float probability)
        {
            return UnityEngine.Random.value < probability;
        }

        public static T GetRandomElement<T>(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new System.Exception("数组为空，无法随机取值");
            }

            var index = UnityEngine.Random.Range(0, array.Length);
            return array[index];
        }
    }
}
