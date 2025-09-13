using DungeonArchitect.Flow.Domains.Tilemap;
using Johnny.SimDungeon;
using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EasyGridBuilderProController : MonoBehaviour
{
    [SerializeField] private GridManager m_GridManager;
    [SerializeField] private EasyGridBuilderProXZ m_EasyGridBuilderPro;
    [SerializeField] private GridAreaDisablerManager m_GridAreaDisablerManager;
    [SerializeField] private Camera m_Camera;
    public UnityEvent<bool> OnBuildModeChanged;

    //Temp
    public Color temp_HideColor;
    private Texture2D temp_GeneratedTexture;



    public Vector2Int v;
    public bool BuildMode;


    public Transform target;
    public BuildableGridObjectSO prefab;











    public void Temp_UpdateGrid(Dictionary<Vector2Int, CellEntity> subCellsMap)
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

                colors[index] = subCellsMap[position].CanBuildOn() ? new Color(255, 255, 255, 255) : temp_HideColor; ;
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

    private void MoveOnGroundPlane()
    {
        if (m_Camera == null) return;

        var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        var ray = m_Camera.ScreenPointToRay(screenCenter);
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float enter))
        {
            var hitPoint = ray.GetPoint(enter);
            hitPoint.x = Mathf.Round(hitPoint.x);
            hitPoint.z = Mathf.Round(hitPoint.z);
            hitPoint.y = 0.1f;

            transform.position = hitPoint;
        }
    }
    public void SetAllDisable(bool buildMode)
    {
        BuildMode = buildMode;
        if (BuildMode)
        {
            var index = m_EasyGridBuilderPro.GetActiveVerticalGridIndex();
            if (m_EasyGridBuilderPro.TryInitializeBuildableGridObjectSinglePlacement(target.position, prefab, FourDirectionalRotation.North, true, true, index, true, out BuildableGridObject buildableGridObject, null, null))
            {
                buildableGridObject.transform.parent = target;

            }
            return;


            //m_EasyGridBuilderPro.GetObjectGridSettings().gridShowColor = color;

            if (m_GridManager.TryGetGridAreaDisablerManager(out GridAreaDisablerManager gridAreaDisablerManager))
            {
                var grid = m_EasyGridBuilderPro.GetActiveGrid() as GridXZ;
                var width = grid.GetWidth();
                var length = grid.GetLength();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < length; y++)
                    {
                        var position = new Vector2Int(x, y);
                        gridAreaDisablerManager.GetCurrentOccupiedGridAreaDisablersCellPositionList().Add(position);
                        //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellColor(position, color, false, m_EasyGridBuilderPro.GetActiveGrid());
                        //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellColor(position, color, false, m_EasyGridBuilderPro.GetActiveGrid());
                    }
                }
            }
        }
        else
        {

        }
        OnBuildModeChanged?.Invoke(BuildMode);
        //   var grid = m_EasyGridBuilderPro.GetActiveGrid() as GridXZ;
        //var width = grid.GetWidth();
        //var length = grid.GetLength();

        //if (buildMode)
        //{

        //    m_EasyGridBuilderPro.GetObjectGridSettings().gridShowColor = color;
        //    //for (int x = 0; x < width; x++)
        //    //{
        //    //    for (int y = 0; y < length; y++)
        //    //    {
        //    //        var position = new Vector2Int(x,y);
        //    //        grid.SetRuntimeObjectGridGeneratedTextureCellColor(position, color);
        //    //        //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellColor(position, color, false, m_EasyGridBuilderPro.GetActiveGrid());
        //    //        //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellColor(position, color, false, m_EasyGridBuilderPro.GetActiveGrid());
        //    //    }
        //    //}
        //}
        //else
        //{
        //    //for (int x = 0; x < width; x++)
        //    //{
        //    //    for (int y = 0; y < length; y++)
        //    //    {
        //    //        var position = new Vector2Int(x, y);
        //    //        //m_EasyGridBuilderPro.SetRuntimeObjectGridGeneratedTextureCellToDefault(position, false, m_EasyGridBuilderPro.GetActiveGrid());
        //    //    }
        //    //}
        //}

    }


}
