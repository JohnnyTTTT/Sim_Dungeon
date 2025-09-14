using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Johnny.SimDungeon
{
    public class EntitiyManager_Cell : EntityManager<FlowTilemapCell, CellEntity>
    {
        public static EntitiyManager_Cell Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<EntitiyManager_Cell>();
                }
                return s_Instance;
            }

        }
        private static EntitiyManager_Cell s_Instance;

        private void Start()
        {
            GridManager.Instance.OnBuildableObjectPlaced += OnBuildableObjectPlaced;
        }

        private void OnBuildableObjectPlaced(EasyGridBuilderPro easyGridBuilderPro, BuildableObject buildableObject)
        {
            //var buildableGridObject = buildableObject as BuildableGridObject;
            //var positions = buildableGridObject.GetObjectCellPositionList();
            //foreach (var item in positions)
            //{
            //    Debug.Log(item);
            //}
        }

        public override void Regist(CellEntity cellEntity)
        {
            map.Add(cellEntity.Data, cellEntity);
        }

        public CellEntity GetCellEntitly(FlowTilemapCell cell)
        {
            if (map.TryGetValue(cell, out var entitly))
            {
                return entitly;
            }
            return null;
        }

        public CellEntity GetCellEntitly(IntVector2 coord)
        {
            var cell = DungeonController.Instance.GetCellFromTileCoord(coord);
            return GetCellEntitly(cell);
        }

        public CellEntity GetCellEntitly(Vector3 worldPosition)
        {
            var cell = DungeonController.Instance.GetCellFromWorldPosition(worldPosition);
            return GetCellEntitly(cell);
        }

    }
}