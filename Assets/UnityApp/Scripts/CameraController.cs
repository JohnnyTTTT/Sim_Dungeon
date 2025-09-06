using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float m_PanSpeed = 0.5f;
        [SerializeField] private float m_ZoomSpeed = 10f;
        [SerializeField] private float m_MinDistance = 5f;
        [SerializeField] private float m_MaxDistance = 50f;
        [SerializeField] private float m_PanSensitivity = 0.01f;
        [SerializeField] private LayerMask m_PanPlaneLayer;

        private Vector3 m_PanStartPoint;
        private Vector2 m_LastMousePosition;
        private bool m_IsPanning;

        private void Update()
        {
            HandlePan();
            HandleZoom();
        }

        private void HandlePan()
        {
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                m_IsPanning = true;
                m_PanStartPoint = GetMousePlaneIntersection();
            }
            else if (Mouse.current.middleButton.wasReleasedThisFrame)
            {
                m_IsPanning = false;
            }

            if (m_IsPanning)
            {
                Vector3 currentPoint = GetMousePlaneIntersection();
                if (currentPoint != Vector3.zero)
                {
                    Vector3 delta = m_PanStartPoint - currentPoint;
                    transform.position += delta;
                }
            }
        }

        private Vector3 GetMousePlaneIntersection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_PanPlaneLayer))
            {
                return hit.point;
            }
            return Vector3.zero;
        }


        private void HandleZoom()
        {
            var scroll = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                var direction = transform.forward.normalized;

                var targetPosition = transform.position + direction * (scroll * m_ZoomSpeed * Time.deltaTime);

                transform.position = targetPosition;
            }
        }
    }

}

