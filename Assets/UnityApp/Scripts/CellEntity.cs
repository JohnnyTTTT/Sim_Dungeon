using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class CellEntity : MonoBehaviour
    {
        public BuildingPart floor;
        public List<BuildingPart> walls = new List<BuildingPart>();
    }
}
