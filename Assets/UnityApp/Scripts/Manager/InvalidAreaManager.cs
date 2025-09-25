using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class InvalidAreaManager : MonoBehaviour
    {
        public static InvalidAreaManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<InvalidAreaManager>();
                }
                return s_Instance;
            }

        }
        private static InvalidAreaManager s_Instance;


        [SerializeField] private float cellSize = 2f;
        //[SerializeField] private GameObject m_InvalidArea;
        [SerializeField] private MeshFilter m_InvalidAreaMesh;
        private Mesh m_CurrentMesh;

        private void Start()
        {
            //m_InvalidAreaMesh = m_InvalidArea.GetComponent<MeshFilter>();

        }

        public void Clear()
        {
            m_InvalidAreaMesh.sharedMesh.Clear();

        }

        public void UpdateMesh()
        {
            m_CurrentMesh = new Mesh();
            m_CurrentMesh.name = "InvalidAreaMesh";


            var verts = new List<Vector3>();
            var tris = new List<int>();

            var width = DungeonController.Instance.tilemapSize.x;
            var hight = DungeonController.Instance.tilemapSize.y;
            int index = 0;
            for (int y = 0; y < hight; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var coord = new Vector2Int(x, y);
                    var cell = ElementManager_LargeCell.Instance.GetElement(coord);
                    if (cell.Data.CellType == FlowTilemapCellType.Floor) continue;

                    float px = x * cellSize;
                    float py = y * cellSize;

                    // 一个格子的4顶点 (放在 y=0 平面上)
                    verts.Add(new Vector3(px, 0, py));
                    verts.Add(new Vector3(px + cellSize, 0, py));
                    verts.Add(new Vector3(px + cellSize, 0, py + cellSize));
                    verts.Add(new Vector3(px, 0, py + cellSize));

                    // 两个三角形
                    tris.Add(index + 0);
                    tris.Add(index + 2);
                    tris.Add(index + 1);

                    tris.Add(index + 0);
                    tris.Add(index + 3);
                    tris.Add(index + 2);

                    index += 4;
                }
            }

            m_CurrentMesh.Clear();
            m_CurrentMesh.SetVertices(verts);
            m_CurrentMesh.SetTriangles(tris, 0);
            m_CurrentMesh.RecalculateNormals();

            m_InvalidAreaMesh.sharedMesh = m_CurrentMesh;
        }


    }
}
