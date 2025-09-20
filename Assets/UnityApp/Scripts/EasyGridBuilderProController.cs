
using SoulGames.EasyGridBuilderPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Johnny.SimDungeon
{
    public enum GridType
    {
        SizeOne,
        SizeTwo,
    }
    public class EasyGridBuilderProController : MonoBehaviour
    {
        public static EasyGridBuilderProController Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<EasyGridBuilderProController>();
                }
                return s_Instance;
            }

        }
        private static EasyGridBuilderProController s_Instance;



        private GridManager gridManager;



        public EasyGridBuilderProXZ m_EasyGridBuilderProSize1;
        public EasyGridBuilderProXZ m_EasyGridBuilderProSize2;
        public GridType currentGridType;

        [SerializeField] private GridAreaDisablerManager m_GridAreaDisablerManager;




        public UnityEvent<bool> OnBuildModeChanged;

        //Temp
        public Color temp_HideColor;
        private Texture2D temp_GeneratedTexture;



        public Vector2Int v;
        public bool BuildMode;


        public Transform target;
        public BuildableGridObjectSO prefab;
        private void Start()
        {
            StartCoroutine(PostStart());
        }

        private IEnumerator PostStart()
        {
            yield return new WaitForEndOfFrame();
            gridManager = GridManager.Instance;
            m_EasyGridBuilderProSize2.gameObject.SetActive(false);
            m_EasyGridBuilderProSize1.gameObject.SetActive(false);
        }





        public bool ReplaceCorner(Entity_Corner corner, BuildableFreeObjectSO temelpte, BuildableFreeObject old, out BuildableFreeObject spawned)
        {
            //if (!TryDestroyBuildableFreeObject(old))
            //{
            //    spawned = null;
            //    return false;
            //}
            BindingService.MainPanelViewModel.GridType = GridType.SizeTwo;
            var worldPosition = corner.transform.position;
            var random = RandomUtility.UpdateBuildableObjectSORandomPrefab(temelpte);
            var verticalGridIndex = BindingService.MainPanelViewModel.ActiveEasyGridBuilderPro.GetActiveVerticalGridIndex();
            var direction = RandomUtility.GetRandomFourDirectionalRotation();
            return TryInitializeBuildableFreeObjectSinglePlacement(worldPosition, temelpte,
                direction, EightDirectionalRotation.North, 0f, Vector3.zero, true, verticalGridIndex, true, out spawned, random, null);
        }

        public bool TryInitializeBuildableFreeObjectSinglePlacement(Entity entity, BuildableFreeObjectSO temelpte,  out BuildableFreeObject spawned)
        {
            BindingService.MainPanelViewModel.GridType = GridType.SizeTwo;
            var worldPosition = entity.transform.position;
            var worldRatation = entity.transform.rotation.eulerAngles.y;
            var verticalGridIndex = BindingService.MainPanelViewModel.ActiveEasyGridBuilderPro.GetActiveVerticalGridIndex();
            var prefabs = RandomUtility.UpdateBuildableObjectSORandomPrefab(temelpte);
            return TryInitializeBuildableFreeObjectSinglePlacement(worldPosition, temelpte,
                FourDirectionalRotation.North, EightDirectionalRotation.North, worldRatation, Vector3.zero, true, verticalGridIndex, true, out spawned, prefabs, null);
        }

        public bool ReplaceGround(Entity_Ground element_Ground, BuildableGridObjectSO temelpte, out BuildableGridObject buildable)
        {
            BindingService.MainPanelViewModel.GridType = GridType.SizeTwo;
            var worldPosition = element_Ground.transform.position;
            var random = RandomUtility.UpdateBuildableObjectSORandomPrefab(temelpte);
            var verticalGridIndex = BindingService.MainPanelViewModel.ActiveEasyGridBuilderPro.GetActiveVerticalGridIndex();
            var direction = RandomUtility.GetRandomFourDirectionalRotation();
            return TryInitializeBuildableGridObjectSinglePlacement(worldPosition, temelpte, direction
                , true, true, verticalGridIndex, true, out buildable, random, null);
        }



        public bool TryDestroyBuildableFreeObject(BuildableFreeObject buildable)
        {
            //if (gridManager.TryGetBuildableObjectMover(out var buildableObjectMover))
            //{
            //    buildableObjectMover.mo
            //}
            //
            if (gridManager.TryGetBuildableObjectDestroyer(out var destroyer))
            {
                //destroyer. SetInputDestroyBuildableObject(buildable);
                if (destroyer.TryDestroyBuildableFreeObject(buildable, true))
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryInitializeBuildableFreeObjectSinglePlacement(Vector3 worldPosition, BuildableFreeObjectSO buildableFreeObjectSO, FourDirectionalRotation fourDirectionalDirection,
            EightDirectionalRotation eightDirectionalDirection, float freeRotation, Vector3 hitNormals, bool ignoreCustomConditions, int verticalGridIndex, bool byPassEventsAndMessages,
            out BuildableFreeObject spawnnedBuildableFreeObject, BuildableObjectSO.RandomPrefabs buildableObjectSORandomPrefab = null, BuildableFreeObject originalBuildableFreeObject = null)
        {
            return BindingService.MainPanelViewModel.ActiveEasyGridBuilderPro.TryInitializeBuildableFreeObjectSinglePlacement(worldPosition, buildableFreeObjectSO, fourDirectionalDirection,
                eightDirectionalDirection, freeRotation, hitNormals, ignoreCustomConditions, verticalGridIndex, byPassEventsAndMessages
                , out spawnnedBuildableFreeObject, buildableObjectSORandomPrefab, originalBuildableFreeObject);
        }

        public bool TryInitializeBuildableEdgeObjectSinglePlacement(Vector3 worldPosition, BuildableEdgeObjectSO buildableEdgeObjectSO, FourDirectionalRotation fourDirectionalDirection,
            bool isBuildableEdgeObjectFlipped, bool ignoreCustomConditions, bool ignoreReplacement, int verticalGridIndex, bool byPassEventsAndMessages, out BuildableEdgeObject spawnnedBuildableEdgeObject,
            BuildableObjectSO.RandomPrefabs buildableObjectSORandomPrefab = null, BuildableEdgeObject originalBuildableEdgeObject = null)
        {

            return BindingService.MainPanelViewModel.ActiveEasyGridBuilderPro.TryInitializeBuildableEdgeObjectSinglePlacement(worldPosition, buildableEdgeObjectSO,
                 FourDirectionalRotation.West, isBuildableEdgeObjectFlipped, ignoreCustomConditions, ignoreReplacement, verticalGridIndex, byPassEventsAndMessages, out spawnnedBuildableEdgeObject,
               buildableObjectSORandomPrefab, originalBuildableEdgeObject);
        }

        public bool TryInitializeBuildableGridObjectSinglePlacement(Vector3 worldPosition, BuildableGridObjectSO buildableGridObjectSO,
            FourDirectionalRotation direction, bool ignoreBuildConditions, bool ignoreReplacement,
            int activeVerticalGridIndex, bool byPassEventsAndMessages, out BuildableGridObject buildableGridObject,
            BuildableObjectSO.RandomPrefabs buildableObjectSORandomPrefab = null, BuildableGridObject originalBuildableGridObject = null)
        {

            return BindingService.MainPanelViewModel.ActiveEasyGridBuilderPro.TryInitializeBuildableGridObjectSinglePlacement(worldPosition, buildableGridObjectSO,
                direction, ignoreBuildConditions, ignoreReplacement, activeVerticalGridIndex, byPassEventsAndMessages, out buildableGridObject,
               buildableObjectSORandomPrefab, originalBuildableGridObject);

        }



        public void Temp_UpdateGrid(Dictionary<Vector2Int, Data_Cell> subCellsMap)
        {
            var activeGridBuilder = GridManager.Instance.GetActiveEasyGridBuilderPro();
            var grid = activeGridBuilder.GetActiveGrid() as GridXZ;
            var gridWidth = grid.GetWidth();
            var gridLength = grid.GetLength();
            var mat = activeGridBuilder.GetComponentInChildren<Renderer>().sharedMaterial;
            temp_GeneratedTexture = mat.GetTexture(Shader.PropertyToID("_Generated_Texture")) as Texture2D;
            var colors = new Color[gridWidth * gridLength];
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridLength; z++)
                {

                    var position = new Vector2Int(x, z);

                    var adjustedZ = gridLength - 1 - z;     // Flip z
                    var adjustedX = gridWidth - 1 - x;      // Flip x
                    var index = adjustedZ * gridWidth + adjustedX;

                    //colors[index] = subCellsMap[position].CanBuildOn() ? new Color(255, 255, 255, 255) : temp_HideColor; 
                }
            }
            temp_GeneratedTexture.SetPixels(colors);
            temp_GeneratedTexture.Apply();
            //foreach (var cell in cellsMap)
            //{
            //    var color = cell.Value.canBuildOn ? new Color(255, 255, 255, 255) : temp_HideColor;
            //    foreach (var cellPosition in cell.Value.subCellCoords)
            //    {
            //        if (cellPosition.x >= 0 && cellPosition.x < gridWidth && cellPosition.y >= 0 && cellPosition.y < gridLength)
            //        {
            //            int adjustedZ = gridLength - 1 - cellPosition.y;     // Flip z
            //            int adjustedX = gridWidth - 1 - cellPosition.x;      // Flip x
            //            temp_GeneratedTexture.SetPixel(adjustedX, adjustedZ, color);
            //        }
            //    }
            //    temp_GeneratedTexture.Apply();
            //}
        }

        public void SetRuntimeObjectGridGeneratedTextureCellColor(Vector2Int cellPosition, Color color)
        {

        }

        private void Update()
        {
            //MoveOnGroundPlane();
        }


        //public void SetAllDisable(bool buildMode)
        //{
        //    BuildMode = buildMode;
        //    if (BuildMode)
        //    {
        //        var index = m_EasyGridBuilderPro.GetActiveVerticalGridIndex();
        //        if (m_EasyGridBuilderPro.TryInitializeBuildableGridObjectSinglePlacement(target.position, prefab, FourDirectionalRotation.North, true, true, index, true, out BuildableGridObject buildableGridObject, null, null))
        //        {
        //            buildableGridObject.transform.parent = target;

        //        }
        //        return;


        //        //m_EasyGridBuilderPro.GetObjectGridSettings().gridShowColor = color;

        //        if (m_GridManager.TryGetGridAreaDisablerManager(out GridAreaDisablerManager gridAreaDisablerManager))
        //        {
        //            var grid = m_EasyGridBuilderPro.GetActiveGrid() as GridXZ;
        //            var width = grid.GetWidth();
        //            var length = grid.GetLength();
        //            for (int x = 0; x < width; x++)
        //            {
        //                for (int y = 0; y < length; y++)
        //                {
        //                    var position = new Vector2Int(x, y);
        //                    gridAreaDisablerManager.GetCurrentOccupiedGridAreaDisablersCellPositionList().Add(position);
        //                    //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellColor(position, color, false, m_EasyGridBuilderPro.GetActiveGrid());
        //                    //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellColor(position, color, false, m_EasyGridBuilderPro.GetActiveGrid());
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {

        //    }
        //    OnBuildModeChanged?.Invoke(BuildMode);
        //    //   var grid = m_EasyGridBuilderPro.GetActiveGrid() as GridXZ;
        //    //var width = grid.GetWidth();
        //    //var length = grid.GetLength();

        //    //if (buildMode)
        //    //{

        //    //    m_EasyGridBuilderPro.GetObjectGridSettings().gridShowColor = color;
        //    //    //for (int x = 0; x < width; x++)
        //    //    //{
        //    //    //    for (int y = 0; y < length; y++)
        //    //    //    {
        //    //    //        var position = new Vector2Int(x,y);
        //    //    //        grid.SetRuntimeObjectGridGeneratedTextureCellColor(position, color);
        //    //    //        //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellColor(position, color, false, m_EasyGridBuilderPro.GetActiveGrid());
        //    //    //        //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellColor(position, color, false, m_EasyGridBuilderPro.GetActiveGrid());
        //    //    //    }
        //    //    //}
        //    //}
        //    //else
        //    //{
        //    //    //for (int x = 0; x < width; x++)
        //    //    //{
        //    //    //    for (int y = 0; y < length; y++)
        //    //    //    {
        //    //    //        var position = new Vector2Int(x, y);
        //    //    //        //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellToDefault(position, false, m_EasyGridBuilderPro.GetActiveGrid());
        //    //    //    }
        //    //    //}
        //    //}

        //}


    }
}
