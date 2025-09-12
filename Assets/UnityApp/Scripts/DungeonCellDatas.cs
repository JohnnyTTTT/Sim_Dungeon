using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Johnny.SimDungeon
{
    public class DungeonCellDatas : MonoBehaviour
    {
        public Dictionary<FlowTilemapCell, CellEntity> cellsMap = new Dictionary<FlowTilemapCell, CellEntity>();
        public Dictionary<Vector2Int, CellEntity> subCellsMap = new Dictionary<Vector2Int, CellEntity>();
        public List<CellEntity> canBuildCells = new List<CellEntity>();
        [SerializeField] private GameObject m_CellEntityPrefab;
        [SerializeField] private Transform m_CellEntityParent;
        public bool showGizmo;
        private void Start()
        {

            GridManager.Instance.OnBuildableObjectPlaced += OnBuildableObjectPlaced;
        }

        private void OnBuildableObjectPlaced(EasyGridBuilderPro easyGridBuilderPro, BuildableObject buildableObject)
        {
            var buildableGridObject = buildableObject as BuildableGridObject;
            var positions = buildableGridObject.GetObjectCellPositionList();
            foreach (var item in positions)
            {
                Debug.Log(item);
            }
        }

        public void Init(FlowTilemapCellDatabase datas)
        {
            cellsMap.Clear();
            subCellsMap.Clear();
            foreach (var cell in datas)
            {
                var entity = Instantiate(m_CellEntityPrefab);
                var info = entity.GetComponent<CellEntity>();
                info.Init(cell, m_CellEntityParent, showGizmo);
                cellsMap.Add(cell, info);
                foreach (var subCellCoord in info.subCellCoords)
                {
                    subCellsMap.Add(subCellCoord, info);
                }
            }

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

        public void CheckCellEntites()
        {
            canBuildCells.Clear();
            foreach (var item in cellsMap)
            {
                if (item.Value.canBuildOn)
                {
                    canBuildCells.Add(item.Value);
                }
            }
        }


    }
}