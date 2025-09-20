using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance
        {
            get
            {
                if (s_Instances == null)
                {
                    s_Instances = FindFirstObjectByType<CameraController>();
                }
                return s_Instances;
            }

        }
        private static CameraController s_Instances;

        public Camera MainCamera;


        [Header("Rotation Settings")]
        [SerializeField] private float m_RotateSpeed = 5f;

        [Header("Pan Settings")]
        [SerializeField] private float m_PlaneHeight = 0f; // 平面高度

        private Vector3 m_PanStartPoint;
        private bool m_IsPanning;
        private UnityEngine.Plane m_PanPlane;

        [Header("Zoom Settings")]
        [SerializeField] private float m_ZoomSpeed = 10f;      // 滚轮速度
        [SerializeField] private float m_ZoomSmoothness = 10f; // Lerp 平滑系数
        [SerializeField] private float m_ZoomAmount = 1f;      // 缩放量倍数

        private Vector3 m_TargetPosition;  // Zoom目标位置
        private float m_ScrollValue = 0f;  // 累积滚轮值

        private void Start()
        {
            m_PanPlane = new UnityEngine.Plane(Vector3.up, new Vector3(0f, m_PlaneHeight, 0f));
            m_TargetPosition = transform.position;
        }

        private void Update()
        {
            HandlePan();
            HandleZoom();
            HandleRotate();
        }

        #region Pan
        private void HandlePan()
        {
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                m_IsPanning = true;
                m_PanStartPoint = GetMousePlaneIntersection(Mouse.current.position.ReadValue());
            }
            else if (Mouse.current.middleButton.wasReleasedThisFrame)
            {
                m_IsPanning = false;
            }

            if (m_IsPanning)
            {
                var currentPoint = GetMousePlaneIntersection(Mouse.current.position.ReadValue());
                if (currentPoint != Vector3.zero)
                {
                    var delta = m_PanStartPoint - currentPoint;
                    transform.position += delta;
                }
            }
        }

        private Vector3 GetMousePlaneIntersection(Vector2 screenPos)
        {
            var ray = Camera.main.ScreenPointToRay(screenPos);
            if (m_PanPlane.Raycast(ray, out var enter))
            {
                return ray.GetPoint(enter);
            }
            return Vector3.zero;
        }
        #endregion

        #region Rotate
        private void HandleRotate()
        {
            if (!Mouse.current.rightButton.isPressed) return;

            var delta = Mouse.current.delta.ReadValue();
            var angle = delta.x * m_RotateSpeed * Time.deltaTime;

            var focusPoint = GetScreenCenterPlanePoint();
            if (focusPoint != Vector3.zero)
            {
                transform.RotateAround(focusPoint, Vector3.up, angle);
            }
            else
            {
                transform.RotateAround(transform.position, Vector3.up, angle);
            }
        }
        #endregion

        #region Zoom
        private void HandleZoom()
        {
            // 累加滚轮输入
            m_ScrollValue += Mouse.current.scroll.ReadValue().y * m_ZoomSpeed;

            // 计算目标位置
            m_TargetPosition = transform.position + transform.forward * m_ScrollValue * m_ZoomAmount;

            // 平滑移动
            transform.position = Vector3.Lerp(transform.position, m_TargetPosition, Time.deltaTime * m_ZoomSmoothness);

            // 清零滚轮值，保证每帧连续响应
            m_ScrollValue = 0f;
        }
        #endregion

        private Vector3 GetScreenCenterPlanePoint()
        {
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            return GetMousePlaneIntersection(screenCenter);
        }
    }
}
