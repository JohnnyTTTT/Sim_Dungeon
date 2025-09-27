using SoulGames.EasyGridBuilderPro;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class InputManager : MonoBehaviour
    {
        private const string DEFAULT = "Default";
        private const string FLOOD_FILL = "Flood Fill";
        private const string SELECT = "Select";

        public event Action<Entity> OnEntitySelect;

        public static InputManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<InputManager>();
                }
                return s_Instance;
            }

        }
        private static InputManager s_Instance;

        [SerializeField] private LayerMask m_MouseCheckLayer;
        [SerializeField] private InputActionAsset m_InputActionsAsset;

        public Element_LargeCell hitLargeCell;
        public Element_SmallCell hitSmallCell;

        public InputAction floodFillAction;
        public InputAction selectAction;
        public Entity currentHover;
        public Entity currentSelect;

        private bool isPlacing;

        private void Start()
        {
            var map = m_InputActionsAsset.FindActionMap(DEFAULT);

            floodFillAction = map.FindAction(FLOOD_FILL);
            floodFillAction.Enable();

            selectAction = map.FindAction(SELECT);
            selectAction.Enable();

            //selectAction.performed += OnSelectPerformed;

            GridManager.Instance.OnActiveBuildableSOChanged += OnActiveBuildableSOChanged;
            if (GridManager.Instance.TryGetBuildableObjectDestroyer(out var buildableObjectDestroyer))
            {
                buildableObjectDestroyer.OnBuildableObjectDestroyedInternal += OnBuildableObjectDestroyedInternal;
            }
            if (GridManager.Instance.TryGetBuildableObjectMover(out var  buildableObjectMover))
            {
                buildableObjectMover.OnBuildableObjectEndMoving += OnBuildableObjectEndMovingByBuildableObjectMoverDelegate;
            }
          
        }

        private void OnBuildableObjectEndMovingByBuildableObjectMoverDelegate(BuildableObject buildableObject)
        {
            Debug.Log(1);
        }

        private void OnSelectPerformed(InputAction.CallbackContext obj)
        {
            if (currentHover != currentSelect)
            {
                if (currentSelect != null)
                {
                    currentSelect.ShowOutline(false);
                    currentSelect = null;
                }
                if (currentHover != null && currentHover.canSelect)
                {
                    currentSelect = currentHover;
                    currentSelect.ShowOutline(true);
                }
                BindingService.MainGameViewModel.SelectEntity = currentSelect;
            }
        }

        private void OnActiveBuildableSOChanged(EasyGridBuilderPro easyGridBuilderPro, BuildableObjectSO buildableObjectSO)
        {
            isPlacing = buildableObjectSO != null;
        }
        private void HandleHover()
        {
            if (PhysicsUtility.MouseRaycastHit(m_MouseCheckLayer, out var hit))
            {
                var point = hit.point;
                point.y = 0;
                hitLargeCell = ElementManager_LargeCell.Instance.GetElement(point);
                hitSmallCell = ElementManager_SmallCell.Instance.GetElement(point);

                if (hit.transform.TryGetComponent<Entity>(out var entity))
                {
                    if (currentHover != entity)
                    {
                        // 移除旧 hover 高亮（如果不是选中）
                        if (currentHover != null && currentHover != currentSelect)
                            currentHover.ShowOutline(false);

                        // 给新 hover 高亮（但不要覆盖选中的高亮）
                        if (entity != currentSelect)
                            entity.ShowOutline(true);

                        currentHover = entity;
                    }
                    return;
                }

            }

            // 鼠标没指向任何对象 → 取消 hover（但不影响选中）
            if (currentHover != null && currentHover != currentSelect)
            {
                currentHover.ShowOutline(false);
                currentHover = null;
            }
        }

        private void Update()
        {
            if (isPlacing )
            {

            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    HandleHover();
                    HandleClick();
                    HandleCancel();
                }

            }
        }

        private void OnBuildableObjectDestroyedInternal(EasyGridBuilderPro easyGridBuilderPro, BuildableObject buildableObject)
        {
            if (currentSelect != null && currentSelect.gameObject == buildableObject.gameObject)
            {
                currentSelect = null;
                BindingService.MainGameViewModel.SelectEntity = null;
            }
        }

        private void HandleClick()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            if (mouse.leftButton.wasPressedThisFrame)
            {
                // 如果有旧的选中对象，取消高亮
                if (currentSelect != null && currentSelect != currentHover)
                    currentSelect.ShowOutline(false);

                // 设置新的选中对象并保持高亮
                if (currentHover != null && currentHover.canSelect)
                {
                    currentSelect = currentHover;
                    currentSelect.ShowOutline(true);
                }
                else
                {
                    currentSelect = null;
                }

                BindingService.MainGameViewModel.SelectEntity = currentSelect;
                Debug.Log("Selected: " + currentSelect);

            }
        }

        private void HandleCancel()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                ClearSelection();
            }
        }

        public void ClearSelection()
        {
            if (currentSelect != null)
            {
                currentSelect.ShowOutline(false);
                currentSelect = null;
            }
        }
    }
}
