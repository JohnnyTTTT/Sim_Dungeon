using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    [CreateAssetMenu(menuName = "Johnny/Build System/Room Info", order = 10)]
    public class RoomInfo : ScriptableObject
    {
        public RoomType roomType;
        public BuildableEdgeObjectSO[] walls;

    }
}
