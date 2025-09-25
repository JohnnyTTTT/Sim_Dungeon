using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public class InputManager : MonoBehaviour
    {
        private const string KEYBOARD = "Keyboard";
        private const string FLOOD_FILL = "Flood Fill";

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

        private void Start()
        {
            var map = m_InputActionsAsset.FindActionMap(KEYBOARD);
            floodFillAction = map.FindAction(FLOOD_FILL);
            floodFillAction.Enable();
        }

        private void Update()
        {
            if (PhysicsUtility.MouseRaycastHit(m_MouseCheckLayer, out var hit))
            {
                var point = hit.point;
                point.y = 0;
                hitLargeCell = ElementManager_LargeCell.Instance.GetElement(point);
                hitSmallCell = ElementManager_SmallCell.Instance.GetElement(point);
            }
        }
    }
}
