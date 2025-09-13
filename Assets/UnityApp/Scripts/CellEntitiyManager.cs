using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Johnny.SimDungeon
{
    public class CellEntitiyManager : MonoBehaviour
    {
        public static CellEntitiyManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<CellEntitiyManager>();
                }
                return s_Instance;
            }

        }
        private static CellEntitiyManager s_Instance;

        public Dictionary<FlowTilemapCell, CellEntity> cellsMap = new Dictionary<FlowTilemapCell, CellEntity>();
        public Dictionary<Vector2Int, CellEntity> subCellsMap = new Dictionary<Vector2Int, CellEntity>();
        [SerializeField] private GameObject m_CellEntityPrefab;
        public bool showGizmo;
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

        public void Init(FlowTilemapCellDatabase datas)
        {
            cellsMap.Clear();
            subCellsMap.Clear();
            foreach (var cell in datas)
            {
                var entity = Instantiate(m_CellEntityPrefab);
                var info = entity.GetComponent<CellEntity>();
                info.Init(cell, transform, showGizmo);
                cellsMap.Add(cell, info);
                foreach (var subCellCoord in info.subCellCoords)
                {
                    subCellsMap.Add(subCellCoord, info);
                }
            }

        }

        public CellEntity GetCellEntitly(FlowTilemapCell cell)
        {
            if (cellsMap.TryGetValue(cell, out var entitly))
            {
                return entitly;
            }
            return null;
        }

        public void DestroyCellEntites()
        {
            foreach (var item in cellsMap)
            {
                if (Application.isPlaying)
                {
                    Destroy(item.Value.gameObject);
                }
                else
                {
                    DestroyImmediate(item.Value.gameObject);
                }
            }
        }

    }
}