using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class Entity : MonoBehaviour
    {
        public IntVector2 lastCoord;
        public bool drawGizmos;
        public GameObject telempte;
        public virtual bool TryReplace(BuildableObjectSO temelpte, BuildableObjectSO.RandomPrefabs prefabs = null)
        {

            return false;
        }

        public virtual void UpdateData()
        {

        }
        public void DestroyTelempte()
        {
            if (telempte != null)
            {
                Destroy(telempte);
            }
        }
    }

}
