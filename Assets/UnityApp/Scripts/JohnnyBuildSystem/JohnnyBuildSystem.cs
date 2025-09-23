using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public class JohnnyBuildSystem : MonoBehaviour
    {
        private const string KEYBOARD = "Keyboard";
        private const string MULTI_SELECTION = "Multi Selection";
        private const string CANCEL_INPUTBUILABLE = "Cancle Input Buildable";
        public static JohnnyBuildSystem Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<JohnnyBuildSystem>();
                }
                return s_Instance;
            }

        }
        private static JohnnyBuildSystem s_Instance;

        [SerializeField] private InputActionAsset inputActionsAsset;
        private InputAction multipleSelectionAction;
        private InputAction cancleInputBuildableAction;
        private bool multipleSelection;

        [SerializeField] private BuildableObjectSO currentBuildable;
        [SerializeField] private Entity_SubEdge m_LastPreviewEdge;
        [SerializeField] private List<Entity_SubEdge> m_LastPreviewRoomEdges;

        private void OnEnable()
        {
            var map = inputActionsAsset.FindActionMap(KEYBOARD);

            multipleSelectionAction = map.FindAction(MULTI_SELECTION);
            multipleSelectionAction.performed += MultiSelectionHoldActionPerformed;
            multipleSelectionAction.canceled += MultiSelectionHoldActionCancelled;
            multipleSelectionAction.Enable();


            //cancleInputBuildableAction = map.FindAction(CANCEL_INPUTBUILABLE);
            //cancleInputBuildableAction.started += CancleInputBuildableActionPerformed;
        }

        private void CancleInputBuildableActionPerformed(InputAction.CallbackContext obj)
        {
            Debug.Log(99);
            //GridManager.Instance.getinpu().getinput
            //if ()
        }

        public void SetInputActiveBuildableObjectSO(BuildableObjectSO buildableObjectSO)
        {
            currentBuildable = buildableObjectSO;
        }

        private void MultiSelectionHoldActionPerformed(InputAction.CallbackContext context)
        {
            multipleSelection = true;
        }

        private void MultiSelectionHoldActionCancelled(InputAction.CallbackContext context)
        {
            multipleSelection = false;
        }

        private void Update()
        {
            //if (currentBuildable != null)
            //{
            //    var mousePos = Mouse.current.position.ReadValue();
            //    var ray = DungeonController.Instance.m_Camera.ScreenPointToRay(mousePos);
            //    var mask = DungeonController.Instance.m_GroundMask;

            //    Entity_SubEdge newHitEdge = null;

            //    if (Physics.Raycast(ray, out var hit, 1000f, mask))
            //    {
            //        if (hit.transform.TryGetComponent<Entity>(out var entity) && entity is Entity_SubEdge edgeEntity)
            //        {
            //            newHitEdge = edgeEntity;
            //        }
            //    }

            //    if (newHitEdge != m_LastPreviewEdge)
            //    {
            //        if (m_LastPreviewEdge != null)
            //        {
            //            Preview(false);
            //            m_LastPreviewEdge = null;
            //        }
            //        if (newHitEdge != null)
            //        {
            //            m_LastPreviewEdge = newHitEdge;
            //            Preview(true);
            //        }
            //    }

            //}
        }

        private void Preview(bool value)
        {
            //if (multipleSelection)
            //{
            //    var room = DataManager_Room.Instance.GetElement(m_LastPreviewEdge.transform.position);
            //    foreach (var cellData in room.containedCells)
            //    {
            //        foreach (var edge in cellData.edges)
            //        {
            //            if (edge != m_LastPreviewEdge)
            //            {
            //                if (value)
            //                {
            //                    edge.Preview(currentBuildable);
            //                }
            //                else
            //                {
            //                    edge.CancelPreview();
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    if (value)
            //    {
            //        m_LastPreviewEdge.Preview(currentBuildable);
            //    }
            //    else
            //    {
            //        m_LastPreviewEdge.CancelPreview();
            //    }
            //}
        }
    }
}
