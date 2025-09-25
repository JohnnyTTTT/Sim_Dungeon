
using SoulGames.EasyGridBuilderPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Johnny.SimDungeon
{
    public enum GridType
    {
        Undefined,
        Nothing,
        Large,
        Small,
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


        //Temp
        public Color temp_HideColor;
        private Texture2D temp_GeneratedTexture;
        private GridManager m_GridManager;

        private void Start()
        {
            m_GridManager = GridManager.Instance;
        }





      

        public void Temp_UpdateGrid(Dictionary<Vector2Int, Element_LargeCell> subCellsMap)
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
