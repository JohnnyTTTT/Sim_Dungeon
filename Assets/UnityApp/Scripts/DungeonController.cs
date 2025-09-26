using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
#if UNITY_EDITOR
using DungeonArchitect.Editors;
#endif
using DungeonArchitect.Flow.Domains.Tilemap;
using DungeonArchitect.Flow.Impl.GridFlow;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public class DungeonController : DungeonEventListener
    {
        public static DungeonController Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<DungeonController>();
                }
                return s_Instance;
            }

        }
        private static DungeonController s_Instance;

        public Dungeon dungeon;
        public GridFlowDungeonConfig dungeonConfig;
        public GridFlowDungeonModel dungeonModel;
        public GridFlowDungeonBuilder gridFlowDungeonBuilder;
        public PooledDungeonSceneProvider pooledDungeonSceneProvider;
        public GridFlowDungeonQuery gridFlowDungeonQuery;
        public GridFlowMinimap gridFlowMinimap;
        public BuildingItemSpawnListener buildingItemSpawnListener;
        public EasyGridBuilderProController easyGridBuilderProController;

        [Title("Disabler")]
        public DisablerController disablerController_SmallCell;
        public DisablerController disablerController_LargeCell;

        public bool worldDataInited;
        private RuntimeSimSceneObjectInstantiator m_RuntimeSimSceneObjectInstantiator;
        public Vector2Int largeTilemapSize;
        public Vector2Int smallTilemapSize;

        private void Start()
        {
            StartCoroutine(BuildDungeonPlaying());
        }

        private IEnumerator BuildDungeonPlaying()
        {
            Debug.Log("[-----System-----] : Dungeon Build Start");
            BindingService.MainGameViewModel.GameMode = GameMode.Loading;


            yield return new WaitForEndOfFrame();

            DestroyDungeon();

            RandomUtility.SetSeed((int)Instance.dungeon.Config.Seed);
            yield return new WaitForEndOfFrame();



            m_RuntimeSimSceneObjectInstantiator = new RuntimeSimSceneObjectInstantiator();
            dungeon.Build(m_RuntimeSimSceneObjectInstantiator);

            yield return new WaitForEndOfFrame();

            largeTilemapSize = new Vector2Int(dungeonModel.Tilemap.Width, dungeonModel.Tilemap.Height);
            var grid = SpawnManager.Instance.m_EasyGridBuilderPro_SmallCell.GetActiveGrid() as GridXZ;
            smallTilemapSize = new Vector2Int(grid.GetWidth(), grid.GetLength());

            worldDataInited = true;

            //disablerController_LargeCell.Init();
            //disablerController_SmallCell.Init();

            //var disablerLargeCell = new HashSet<Vector2Int>();

            //foreach (var item in ElementManager_LargeCell.Instance.GetAllElements())
            //{
            //    if (item.Data.CellType != FlowTilemapCellType.Floor)
            //    {
            //        disablerLargeCell.Add(item.coord);
            //        //foreach (var small in item.containedSmallCells)
            //        //{
            //        //    if (small != null)
            //        //    {
            //        //        disablerSmallCell.Add(small.coord);
            //        //    }
            //        //}
            //    }
            //}

            //var disablerSmallCell = new HashSet<Vector2Int>();
            //foreach (var item in ElementManager_SmallCell.Instance.GetAllElements())
            //{
            //    if (item.isBuildingValid)
            //    {
            //        disablerSmallCell.Add(item.coord);
            //    }
            //}
            //foreach (var edge in ElementManager_Edge.Instance.GetAllElements())
            //{
            //    if (edge.Data.EdgeType != FlowTilemapEdgeType.Empty)
            //    {
            //        foreach (var small in edge.containedSmallCells)
            //        {
            //            disablerSmallCell.Add(small.coord);
            //        }
            //    }
            //}


            //disablerController_LargeCell.AddDisablerCells(disablerLargeCell);
            //disablerController_SmallCell.AddDisablerCells(disablerSmallCell);

            BindingService.MainGameViewModel.GameMode = GameMode.Default;
            BindingService.MainGameViewModel.GridType = GridType.Nothing;
            Debug.Log("[-----System-----] : Dungeon Build End");

        }

        public void BuildDungeonEditor()
        {
            dungeon.Build(new RuntimeSimSceneObjectInstantiator());
        }

        public void DestroyDungeon()
        {
            dungeon.DestroyDungeon();
            InvalidAreaManager.Instance.Clear();
        }


        public override void OnPostDungeonLayoutBuild(Dungeon dungeon, DungeonModel model)
        {
        }

        public override void OnDungeonMarkersEmitted(Dungeon dungeon, DungeonModel model, LevelMarkerList markers)
        {
            var gridFlowDungeonModel = model as GridFlowDungeonModel;

            ElementManager_LargeCell.Instance.Init(gridFlowDungeonModel.Tilemap.Cells);
            ElementManager_Edge.Instance.Init(gridFlowDungeonModel.Tilemap.Edges);
            ElementManager_SmallCell.Instance.Init(SpawnManager.Instance.m_EasyGridBuilderPro_SmallCell);
            ElementManager_Region.Instance.Init();


            ElementManager_LargeCell.Instance.PostInit();
            ElementManager_Edge.Instance.PostInit();
            ElementManager_SmallCell.Instance.PostInit();
            ElementManager_Region.Instance.PostInit();

   
            Debug.Log("[-----System-----] : OnDungeonMarkersEmitted");
        }

        public override void OnPostDungeonBuild(Dungeon dungeon, DungeonModel model)
        {
            //var entities = FindObjectsOfType<Entity>();
            //foreach (var item in entities)
            //{
            //    item.UpdateData();
            //}
            SpawnManager.Instance.Init();
            InvalidAreaManager.Instance.UpdateMesh();

            Debug.Log("[-----System-----] : OnPostDungeonBuild");
        }

        public override void OnDungeonDestroyed(Dungeon dungeon)
        {
            ElementManager_LargeCell.Instance.UnInit();
            ElementManager_Edge.Instance.UnInit();
            ElementManager_Region.Instance.UnInit();
            ElementManager_SmallCell.Instance.UnInit();
            SpawnManager.Instance.UnInit();

        }
    }
}
