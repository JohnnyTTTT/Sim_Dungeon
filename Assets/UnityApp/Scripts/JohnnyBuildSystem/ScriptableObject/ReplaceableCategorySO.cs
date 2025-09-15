using UnityEngine;

namespace Johnny.SimDungeon
{
    [CreateAssetMenu(menuName = "Johnny/Build System/Replaceable Category SO", order = 100)]
    public class ReplaceableCategorySO : ScriptableObject
    {
        public string categoryName;
        public Sprite icon;
    }
}
