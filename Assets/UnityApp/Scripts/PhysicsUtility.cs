using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public static class PhysicsUtility
    {
        public static bool MouseRaycastHit(LayerMask layerMask, out RaycastHit hit)
        {
            var mousePos = Mouse.current.position.ReadValue();
            var ray = CameraController.Instance.MainCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                return true;
            }
            return false;
        }
    }
}
