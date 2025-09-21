//using DungeonArchitect;
//using DungeonArchitect.Flow.Domains.Tilemap;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Johnny.SimDungeon
//{
//    public class ElementManager_Edge : ElementManager
//    {
//        public static ElementManager_Edge Instance
//        {
//            get
//            {
//                if (s_Instance == null)
//                {
//                    s_Instance = FindFirstObjectByType<ElementManager_Edge>();
//                }
//                return s_Instance;
//            }

//        }
//        private static ElementManager_Edge s_Instance;
//        private Dictionary<IntVector2, Data_Edge> horizontalMap = new Dictionary<IntVector2, Data_Edge>();
//        private Dictionary<IntVector2, Data_Edge> verticalMap = new Dictionary<IntVector2, Data_Edge>();
//        public void Init(FlowTilemapEdgeDatabase edges)
//        {
//            if (Inited) return;
//            horizontalMap.Clear();
//            verticalMap.Clear();

//            foreach (var edge in edges)
//            {
//                var data = new Data_Edge(edge);
//                if (edge.HorizontalEdge)
//                {
//                    horizontalMap[edge.EdgeCoord] = data;
//                }
//                else
//                {
//                    verticalMap[edge.EdgeCoord] = data;
//                }
//            }
//            Inited = true;
//            Debug.Log($"[-----System-----] : DataManager_Edge inited , HorizontalMap count <{horizontalMap.Count}> - VerticalMap <{verticalMap.Count}>");
//        }

//        public Data_Edge GetHorizontal(IntVector2 cooed)
//        {
//            if (horizontalMap.TryGetValue(cooed, out var  data))
//            {
//                return data;
//            }
//            return null;
//        }

//        public Data_Edge GetVertical(IntVector2 cooed)
//        {
//            if (verticalMap.TryGetValue(cooed, out var data))
//            {
//                return data;
//            }
//            return null;
//        }

//        public void UnInit()
//        {
//            horizontalMap.Clear();
//            verticalMap.Clear();
//            Inited = false;
//        }

//        private void OnDrawGizmos()
//        {
//            if (drawGizmos)
//            {
//                foreach (var item in horizontalMap)
//                {

//                    if (item.Value.Data.EdgeType ==  FlowTilemapEdgeType.Wall || item.Value.Data.EdgeType == FlowTilemapEdgeType.Fence)
//                    {
//                        item.Value.DrawGizmos();
//                    }

//                }
//                foreach (var item in verticalMap)
//                {
//                    if (item.Value.Data.EdgeType == FlowTilemapEdgeType.Wall || item.Value.Data.EdgeType == FlowTilemapEdgeType.Fence)
//                    {
//                        item.Value.DrawGizmos();
//                    }
//                }
//            }
//        }

//    }
//}
