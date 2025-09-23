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
    public class DisablerManager : MonoBehaviour
    {
        public static DisablerManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<DisablerManager>();
                }
                return s_Instance;
            }

        }
        private static DisablerManager s_Instance;

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
            foreach (var cell in ElementManager_Cell.Instance.GetAllCells())
            {
                if (cell.Data.CellType != FlowTilemapCellType.Floor)
                {
                    m_OccupiedCellPositionLis.Add(cell.coord);
                }
            }

            var data = m_GridAreaDisablerData.GridAreaDataDictionary;
            if (data.TryGetValue(m_GridArea, out var gridAreaData))
            {
                gridAreaData.currentOccupiedEasyGridBuilderPro = EasyGridBuilderProXZ;
                gridAreaData.currentOccupiedGrid = EasyGridBuilderProXZ.GetActiveGrid();
                gridAreaData.currentOccupiedCellPositionList = m_OccupiedCellPositionLis;
            }
        }

        private void OnGridAreaDisablerInitialized(GridAreaDisabler gridAreaDisabler, GridAreaDisablerData gridAreaDisablerData)
        {
            m_GridAreaDisablerData = gridAreaDisablerData;
        }

    }
}
