using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum DevelopMode
    {
        None,
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
                switch (currentMode)
                {
                    case DevelopMode.None:
                        break;
                    case DevelopMode.Area:
                        var cell = ElementManager_Cell.Instance.GetElement(position);
                        if (cell == null)
                        {
                            Clear();
                        }
                        else if (cell != m_LasatDetectionCell)
                        {
                            Clear();
                            m_LasatDetectionCell = cell;
                            var area = cell.area;
                            if (area != null)
                            {
                                foreach (var child in area.containedCells)
                                {
                                    CreateCellDetection(child);
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void CreateCellDetection(Element_Cell cell)
        {
            var position = CoordUtility.TileCoordToWorldPosition(cell.Data.TileCoord);
            var obj = Instantiate(cellDetectionPrefab, position, Quaternion.identity, transform);
            obj.name = cell.ToString();
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
