using UnityEngine;

namespace Johnny.SimDungeon
{
    [CreateAssetMenu(menuName = "Johnny/Build System/Replaceable Object SO", order = 10)]
    public class ReplaceableObjectSO : ScriptableObject
    {
        public bool randomModel = false;
        public GameObject[] Models;
    }
}
