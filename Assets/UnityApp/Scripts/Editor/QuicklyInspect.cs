using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
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
            DungeonController.Instance.BuildDungeonEditor();
        }
        [Button]
        private void DestroyDungeon()
        {
            DungeonController.Instance.DestroyDungeon();
        }

        [Button]
        private void GC()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            Resources.UnloadUnusedAssets();
        }

        [Button]
        private void OpenMainScene()
        {
            string scenePath = "Assets/UnityApp/Scenes/MainGame/MainGame Lasted.unity";
            EditorSceneManager.OpenScene(scenePath);
        }

        private void Update()
        {
            if (ElementManager_LargeCell.Instance!= null && ElementManager_LargeCell.Instance.drawGizmos != showCellGizmo)
            {
                ElementManager_LargeCell.Instance.drawGizmos = showCellGizmo;
            }
            if (ElementManager_Edge.Instance != null && ElementManager_Edge.Instance.drawGizmos != showEdgeGizmo)
            {
                ElementManager_Edge.Instance.drawGizmos = showEdgeGizmo;
            }
            if (ElementManager_Region.Instance != null && ElementManager_Region.Instance.drawGizmos != showRoomGizmo)
            {
                ElementManager_Region.Instance.drawGizmos = showRoomGizmo;
            }
            if (ElementManager_SmallCell.Instance != null && ElementManager_SmallCell.Instance.drawGizmos != showTileGizmo)
            {
                ElementManager_SmallCell.Instance.drawGizmos = showTileGizmo;
            }
        }
    }
}
#endif