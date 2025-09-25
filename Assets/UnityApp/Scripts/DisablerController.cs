using DungeonArchitect.Flow.Domains.Tilemap;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class DisablerController : MonoBehaviour
    {

        [SerializeField] private EasyGridBuilderProXZ EasyGridBuilderProXZ;
        [SerializeField] private GridArea m_GridArea;
        private GridAreaDisablerData m_GridAreaDisablerData;
        private List<Vector2Int> m_OccupiedCellPositionLis = new List<Vector2Int>();

        private void Start()
        {
            GridAreaDisabler.OnGridAreaDisablerInitialized += OnGridAreaDisablerInitialized;
        }

        public void Init()
        {
            m_OccupiedCellPositionLis.Clear();
            //foreach (var cell in ElementManager_Cell.Instance.GetAllCells())
            //{
            //    if (cell.Data.CellType != FlowTilemapCellType.Floor)
            //    {
            //        m_OccupiedCellPositionLis.Add(cell.coord);
            //    }
            //}

            var data = m_GridAreaDisablerData.GridAreaDataDictionary;
            if (data.TryGetValue(m_GridArea, out var gridAreaData))
            {
                gridAreaData.currentOccupiedEasyGridBuilderPro = EasyGridBuilderProXZ;
                gridAreaData.currentOccupiedGrid = EasyGridBuilderProXZ.GetActiveGrid();
                gridAreaData.currentOccupiedCellPositionList = m_OccupiedCellPositionLis;
            }
        }

        public void AddDisablerCells(IEnumerable<Element_SmallCell> containedSmallCells)
        {
            var positions = containedSmallCells.Select(x => x.coord);
            m_OccupiedCellPositionLis.AddRange(positions);
        }

        public void AddDisablerCell()
        {


        }



        private void OnGridAreaDisablerInitialized(GridAreaDisabler gridAreaDisabler, GridAreaDisablerData gridAreaDisablerData)
        {
            m_GridAreaDisablerData = gridAreaDisablerData;
        }

    }
}
