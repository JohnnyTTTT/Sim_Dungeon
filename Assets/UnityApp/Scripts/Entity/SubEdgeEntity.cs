using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class SubEdgeEntity : MonoBehaviour
    {
        public GameObject model;
        public ReplaceableObjectSO replaceableObjectSO;
        [HideInInspector] public CellEntity cellEntity;
        public GameObject upper;
        public GameObject lower;

    }
}
