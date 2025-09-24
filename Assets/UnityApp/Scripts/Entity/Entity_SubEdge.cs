using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_SubEdge : MonoBehaviour
    {
        public Entity_Wall parent;
        public Entity_SubEdge relativeEdge;

        public GameObject fullMode;
        public GameObject halfMode;
        public GameObject slopeMode;



        //private void Start()
        //{
        //    SetWallCutType(WallCutType.None);
        //}

        //public void SetWallCutType(WallCutType type)
        //{
        //    if (fullMode && halfMode && slopeMode)
        //    {
        //        switch (type)
        //        {
        //            case WallCutType.None:
        //                fullMode.gameObject.SetActive(true);
        //                halfMode.gameObject.SetActive(false);
        //                slopeMode.gameObject.SetActive(false);
        //                break;
        //            case WallCutType.Half:
        //                fullMode.gameObject.SetActive(false);
        //                halfMode.gameObject.SetActive(true);
        //                slopeMode.gameObject.SetActive(false);
        //                break;
        //            case WallCutType.Slope:
        //                fullMode.gameObject.SetActive(false);
        //                halfMode.gameObject.SetActive(false);
        //                slopeMode.gameObject.SetActive(true);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
    }
}
