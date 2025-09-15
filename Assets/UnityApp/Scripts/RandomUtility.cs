using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public static class RandomUtility 
    {
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
