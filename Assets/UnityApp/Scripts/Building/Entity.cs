using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity : MonoBehaviour
    {
        public IntVector2 lastCoord;
        public bool drawGizmos;
        public virtual bool TryReplace(BuildableObjectSO temelpte)
        {
            return false;
        }

        public virtual void UpdateData()
        { 
        
        }
    }
}
