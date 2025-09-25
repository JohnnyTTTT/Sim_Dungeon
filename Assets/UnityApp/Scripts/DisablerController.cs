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
        [SerializeField] private EasyGridBuilderProXZ m_EasyGridBuilderProXZ;
        [SerializeField] private GridArea m_GridArea;
        private GridAreaDisablerData m_GridAreaDisablerData;
        private Material m_GridMaterial;
        private Vector2Int m_GridSize;
        private List<Vector2Int> m_OccupiedCellPositionLis = new List<Vector2Int>();

        public Color temp_ShowColor;
        public Color temp_HideColor;

        private void Start()
        {
            GridAreaDisabler.OnGridAreaDisablerInitialized += OnGridAreaDisablerInitialized;

        }

        private void OnDestroy()
        {
            m_OccupiedCellPositionLis.Clear();
        }

        public void Init()
        {
            m_OccupiedCellPositionLis.Clear();
            Temp_UpdateGrid();
        }

        public void AddDisablerCells(IEnumerable<Vector2Int> positions)
        {
            m_OccupiedCellPositionLis.AddRange(positions);
            Temp_UpdateGrid();
        }

        public void RemoveDisablerCells(IEnumerable<Vector2Int> positions)
        {
            m_OccupiedCellPositionLis.RemoveAll(cell => positions.Contains(cell));
            Temp_UpdateGrid();
        }

        public void AddDisablerCell()
        {


        }

        public void Temp_UpdateGrid()
        {
            if (m_GridMaterial == null)
            {
                m_GridMaterial = m_EasyGridBuilderProXZ.GetComponentInChildren<Renderer>().sharedMaterial;
            }
            if (m_GridSize.x <= 0)
            {
                var grid = m_EasyGridBuilderProXZ.GetActiveGrid() as GridXZ;
                m_GridSize = new Vector2Int(grid.GetWidth(), grid.GetLength());
            }
            var gridWidth = m_GridSize.x;
            var gridLength = m_GridSize.y;
            var generatedTexture = m_GridMaterial.GetTexture(Shader.PropertyToID("_Generated_Texture")) as Texture2D;
            var colors = new Color[gridWidth * gridLength];
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridLength; z++)
                {
                    var position = new Vector2Int(x, z);
                    var adjustedZ = gridLength - 1 - z;     // Flip z
                    var adjustedX = gridWidth - 1 - x;      // Flip x
                    var index = adjustedZ * gridWidth + adjustedX;
                    if (m_OccupiedCellPositionLis.Contains(position))
                    {
                        colors[index] = temp_HideColor;
                    }
                    else
                    {
                        colors[index] = temp_ShowColor;
                    }
                }
            }
            generatedTexture.SetPixels(colors);
            generatedTexture.Apply();
        }

        private void OnGridAreaDisablerInitialized(GridAreaDisabler gridAreaDisabler, GridAreaDisablerData gridAreaDisablerData)
        {
            m_GridAreaDisablerData = gridAreaDisablerData;
            var data = m_GridAreaDisablerData.GridAreaDataDictionary;
            if (data.TryGetValue(m_GridArea, out var gridAreaData))
            {
                gridAreaData.currentOccupiedEasyGridBuilderPro = m_EasyGridBuilderProXZ;
                gridAreaData.currentOccupiedGrid = m_EasyGridBuilderProXZ.GetActiveGrid();
                gridAreaData.currentOccupiedCellPositionList = m_OccupiedCellPositionLis;
            }
        }

    }
}
