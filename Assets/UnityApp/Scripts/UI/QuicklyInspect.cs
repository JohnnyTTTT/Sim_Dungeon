using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace Johnny.SimDungeon
{
    public class QuicklyInspectWindow :OdinEditorWindow
    {
        [ToggleLeft]
        public bool showCellGizmo;
        [ToggleLeft]
        public bool showEdgeGizmo;
        [ToggleLeft]
        public bool showRoomGizmo;
        [ToggleLeft]
        public bool showTileGizmo;

        [MenuItem("Tools/Johnny/Quickly Inspect Window")]
        private static void Open()
        {
            GetWindow<QuicklyInspectWindow >().Show();
        }

        [Button]
        private void BuildDungeon()
        {
            DungeonController.Instance.BuildDungeon();
        }
        [Button]
        private void DestroyDungeon()
        {
            DungeonController.Instance.DestroyDungeon();
        }
        [Button]
        private void ApplyTheme()
        {
            DungeonController.Instance.ApplyTheme();
        }

   
        private void Update()
        {
            if (DataManager_Cell.Instance.drawGizmos != showCellGizmo)
            {
                DataManager_Cell.Instance.drawGizmos = showCellGizmo;
            }
            if (DataManager_Edge.Instance.drawGizmos != showEdgeGizmo)
            {
                DataManager_Edge.Instance.drawGizmos = showEdgeGizmo;
            }
            if (DataManager_Room.Instance.drawGizmos != showRoomGizmo)
            {
                DataManager_Room.Instance.drawGizmos = showRoomGizmo;
            }
            if (DataManager_Tile.Instance.drawGizmos != showTileGizmo)
            {
                DataManager_Tile.Instance.drawGizmos = showTileGizmo;
            }
        }
    }
}
#endif