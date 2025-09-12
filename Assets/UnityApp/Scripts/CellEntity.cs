using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class CellEntity : MonoBehaviour
    {
        public BuildingPart floor;
        public BuildingPart walleast;
        public List<BuildingPart> walls = new List<BuildingPart>();
        public int[,] subCell = new int[4, 4];
    }
}
