//using DungeonArchitect;
//using DungeonArchitect.Flow.Domains.Tilemap;
//using Loxodon.Framework.Binding;
//using SoulGames.EasyGridBuilderPro;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.InputSystem;

//namespace Johnny.SimDungeon
//{
//    public class LandManagement : MonoBehaviour
//    {
//        public static LandManagement Instance
//        {
//            get
//            {
//                if (s_Instance == null)
//                {
//                    s_Instance = FindFirstObjectByType<LandManagement>();
//                }
//                return s_Instance;
//            }

//        }
//        private static LandManagement s_Instance;













//        private void UpdateOrCreateGroundEntity(Element_Cell cell)
//        {
//            if (cell.Data.CellType != FlowTilemapCellType.Empty)
//            {
//                SpawnManager.Instance.UpdateOrCreateGroundEntity(cell);
//                SpawnManager.Instance.DestroyCeilingEntity(cell);
//            }
//            else
//            {

//            }
//        }

//        private void UpdateOrCreateEdgeEntity(List<Element_Edge> edges)
//        {
//            foreach (var edge in edges)
//            {
//                SpawnManager.Instance.UpdateOrCreateEdgeEntity(edge);
//            }
//        }

//    }
//}
