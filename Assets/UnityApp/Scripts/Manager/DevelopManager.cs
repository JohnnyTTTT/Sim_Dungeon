using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum DevelopMode
    {
        None,
        Cell,
        Area,
    }
    public class DevelopManager : MonoBehaviour
    {
        public static DevelopManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<DevelopManager>();
                }
                return s_Instance;
            }
        }
        private static DevelopManager s_Instance;

        private static int s_BaseColor = Shader.PropertyToID("_BaseColor");

        public DevelopMode currentMode;
        [SerializeField] private GameObject m_LargeCellDetectionPrefab;
        [SerializeField] private GameObject m_SmallCellDetectionPrefab;
        [SerializeField] private LayerMask detectionLayer;
        private Element_LargeCell m_LasatDetectionCell;
        private List<GameObject> m_InstantiateLargeCells = new List<GameObject>();
        private List<GameObject> m_InstantiateSmallCells = new List<GameObject>();

        private void Update()
        {
            if (currentMode == DevelopMode.None) return;
            if (PhysicsUtility.MouseRaycastHit(detectionLayer, out var hit))
            {
                var position = new Vector3(hit.point.x, 0f, hit.point.z);
                var cell = ElementManager_LargeCell.Instance.GetElement(position);
                if (cell == null)
                {
                    Clear();
                }
                if (cell != m_LasatDetectionCell)
                {
                    m_LasatDetectionCell = cell;
                    Clear();
                    switch (currentMode)
                    {
                        case DevelopMode.None:
                            break;
                        case DevelopMode.Cell:
                            CreateLargeCellDetection(cell);
                            if (cell.neighbors[0] != null)
                                CreateLargeCellDetection(cell.neighbors[0], Color.green);
                            if (cell.neighbors[1] != null)
                                CreateLargeCellDetection(cell.neighbors[1], Color.blue);
                            if (cell.neighbors[2] != null)
                                CreateLargeCellDetection(cell.neighbors[2], Color.yellow);
                            if (cell.neighbors[3] != null)
                                CreateLargeCellDetection(cell.neighbors[3], Color.red);

                            //if (cell.containedSmallCells[0] != null)
                            //    CreateSmallCellDetection(cell.containedSmallCells[0], Color.green);
                            //if (cell.containedSmallCells[1] != null)
                            //    CreateSmallCellDetection(cell.containedSmallCells[1], Color.blue);
                            //if (cell.containedSmallCells[2] != null)
                            //    CreateSmallCellDetection(cell.containedSmallCells[2], Color.yellow);
                            //if (cell.containedSmallCells[3] != null)
                            //    CreateSmallCellDetection(cell.containedSmallCells[3], Color.red);

                            break;
                        case DevelopMode.Area:
                            var area = cell.region;
                            if (area != null)
                            {
                                foreach (var child in area.containedLargeCells)
                                {
                                    CreateLargeCellDetection(child);
                                }
                            }
                            break;
                    }
                }

            }
        }

        private void CreateLargeCellDetection(Element_LargeCell cell, Color? color = null)
        {
            var obj = Instantiate(m_LargeCellDetectionPrefab, cell.worldPosition, Quaternion.identity, transform);
            obj.name = cell.ToString();
            if (color != null)
            {
                obj.GetComponent<Renderer>().material.SetColor(s_BaseColor, color.Value);
            }
            m_InstantiateLargeCells.Add(obj);
        }

        private void CreateSmallCellDetection(Element_SmallCell cell, Color? color = null)
        {
            var obj = Instantiate(m_SmallCellDetectionPrefab, cell.worldPosition + new Vector3(0f, 0.6f, 0f), Quaternion.identity, transform);
            obj.name = cell.ToString();
            if (color != null)
            {
                obj.GetComponent<Renderer>().material.SetColor(s_BaseColor, color.Value);
            }
            m_InstantiateSmallCells.Add(obj);
        }

        private void Clear()
        {
            for (int i = m_InstantiateLargeCells.Count - 1; i >= 0; i--)
            {
                Destroy(m_InstantiateLargeCells[i]);
            }
            m_InstantiateLargeCells.Clear();

            for (int i = m_InstantiateSmallCells.Count - 1; i >= 0; i--)
            {
                Destroy(m_InstantiateSmallCells[i]);
            }
            m_InstantiateSmallCells.Clear();
        }
    }
}
