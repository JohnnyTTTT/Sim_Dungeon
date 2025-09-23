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
        [SerializeField] private GameObject cellDetectionPrefab;
        [SerializeField] private LayerMask detectionLayer;
        private Element_Cell m_LasatDetectionCell;
        private List<GameObject> m_Instantiates = new List<GameObject>();

        private void Update()
        {
            if (currentMode == DevelopMode.None) return;
            if (PhysicsUtility.MouseRaycastHit(detectionLayer, out var hit))
            {
                var position = new Vector3(hit.point.x, 0f, hit.point.z);
                var cell = ElementManager_Cell.Instance.GetElement(position);
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
                            CreateCellDetection(cell);
                            CreateCellDetection(cell.neighbors[0], Color.green);
                            CreateCellDetection(cell.neighbors[1], Color.blue);
                            CreateCellDetection(cell.neighbors[2], Color.yellow);
                            CreateCellDetection(cell.neighbors[3], Color.red);
                            break;
                        case DevelopMode.Area:
                            var area = cell.region;
                            if (area != null)
                            {
                                foreach (var child in area.containedCells)
                                {
                                    CreateCellDetection(child);
                                }
                            }
                            break;
                    }
                }

            }
        }

        private void CreateCellDetection(Element_Cell cell, Color? color = null)
        {
            var position = CoordUtility.TileCoordToWorldPosition(cell.Data.TileCoord);
            var obj = Instantiate(cellDetectionPrefab, position, Quaternion.identity, transform);
            obj.name = cell.ToString();
            if (color != null)
            {
                obj.GetComponent<Renderer>().material.SetColor(s_BaseColor, color.Value);
            }
            m_Instantiates.Add(obj);
        }

        private void Clear()
        {
            for (int i = m_Instantiates.Count - 1; i >= 0; i--)
            {
                Destroy(m_Instantiates[i]);
            }
            m_Instantiates.Clear();
        }
    }
}
