using UnityEngine;

namespace Johnny.SimDungeon
{
    [CreateAssetMenu(menuName = "Johnny/Build System/Replaceable Object SO", order = 10)]
    public class ReplaceableObjectSO : ScriptableObject
    {
        public string objectName;
        public bool randomModel = false;
        public Sprite icon;
        public GameObject[] Models;

    }
}
