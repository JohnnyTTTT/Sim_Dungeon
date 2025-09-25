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
        public DisablerController ddisablerController_LargeCell;

        public bool worldDataInited;
        private RuntimeSimSceneObjectInstantiator m_RuntimeSimSceneObjectInstantiator;
        public IntVector2 tilemapSize;

        private void Start()
        {
            m_RuntimeSimSceneObjectInstantiator = new RuntimeSimSceneObjectInstantiator();
            StartCoroutine(PostStart());
        }

        private IEnumerator PostStart()
        {
            Debug.Log("[-----System-----] : Dungeon Build Start");
            yield return new WaitForEndOfFrame();
            BindingService.MainGameViewModel.GameMode = GameMode.Loading;
            DestroyDungeon();
            yield return new WaitForEndOfFrame();
            BuildDungeon();
            tilemapSize = new IntVector2(dungeonModel.Tilemap.Width, dungeonModel.Tilemap.Height);
            yield return new WaitForEndOfFrame();
            worldDataInited = true;
            RandomUtility.SetSeed((int)Instance.dungeon.Config.Seed);
            yield return new WaitForEndOfFrame();
            BindingService.MainGameViewModel.GameMode = GameMode.Default;
            yield return new WaitForEndOfFrame();
            disablerController_SmallCell.Init();
            ddisablerController_LargeCell.Init();

        }


        public void BuildDungeon()
        {
            m_RuntimeSimSceneObjectInstantiator = new RuntimeSimSceneObjectInstantiator();
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
            ElementManager_Region.Instance.Init(gridFlowDungeonModel.Tilemap.Cells);
            ElementManager_SmallCell.Instance.Init(SpawnManager.Instance.m_EasyGridBuilderProSize1);
            ElementManager_LargeCell.Instance.PostInit();
            ElementManager_Edge.Instance.PostInit();

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
