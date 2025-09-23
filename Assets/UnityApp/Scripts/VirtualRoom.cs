using DungeonArchitect;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class VirtualRoom : MonoBehaviour
    {
        [SerializeField] private GameObject m_Ground;
        [SerializeField] private GameObject m_WallLeft;
        [SerializeField] private GameObject m_WallUp;
        [SerializeField] private GameObject m_WallRight;
        [SerializeField] private GameObject m_WallDown;

        public void CalculateEdges(List<IntVector2> roomCoords)
        {
            //var coord = DungeonController.Instance.WorldPositionToTileCoord(transform.position);

            //var coordLeft = new IntVector2(coord.x-1, coord.y);
            //m_WallLeft.SetActive(!roomCoords.Contains(coordLeft));

            //var coordUp = new IntVector2(coord.x, coord.y + 1);
            //m_WallUp.SetActive(!roomCoords.Contains(coordUp));

            //var coordRight = new IntVector2(coord.x+1, coord.y);
            //m_WallRight.SetActive(!roomCoords.Contains(coordRight));

            //var coordDown = new IntVector2(coord.x, coord.y-1);
            //m_WallDown.SetActive(!roomCoords.Contains(coordDown));
        }


    }
}
