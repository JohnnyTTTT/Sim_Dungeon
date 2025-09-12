
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public enum PlaceMode
    {
        Single,
        Area
    }
    public class BuildingPlacerController : MonoBehaviour
    {
        [SerializeField] private DungeonController m_DungeonController;
        [SerializeField] private InputActionReference m_AreaInputReference;

        private  PlaceMode m_PlaceMode;

        private void OnEnable()
        {
            m_PlaceMode = PlaceMode.Single;
            m_AreaInputReference.action.Enable();
            m_AreaInputReference.action.started += OnPressed;
            m_AreaInputReference.action.canceled += OnReleased;
        }

        private void OnDisable()
        {
            m_AreaInputReference.action.started -= OnPressed;
            m_AreaInputReference.action.canceled -= OnReleased;
            m_AreaInputReference.action.Disable();
        }

        private void OnPressed(InputAction.CallbackContext ctx)
        {
            m_PlaceMode = PlaceMode.Area;
        }

        private void OnReleased(InputAction.CallbackContext ctx)
        {
            m_PlaceMode = PlaceMode.Single;
        }
    }
}
