using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class ConfirmPanel : MonoBehaviour
    {
        public static ConfirmPanel Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ConfirmPanel>();
                }
                return s_Instance;
            }

        }
        private static ConfirmPanel s_Instance;

        public CanvasGroup canvasGroup;
        public Button confirmButton;
        public Button cancelButton;

        public  Action onConfirm;
        public  Action onCancel;

        public  bool isActive
        {
            get
            {
                return m_IsActive;
            }
            set
            {
                m_IsActive = value;
                canvasGroup.alpha = m_IsActive ? 1f : 0f;
            }

        }
        private  bool m_IsActive;

        private void Start()
        {
            isActive = false;
            confirmButton.onClick.AddListener(OnConfirmClicked);
            cancelButton.onClick.AddListener(OnCancelClicked);
        }
        private void OnConfirmClicked()
        {
            onConfirm?.Invoke();
            isActive = false;
        }

        private void OnCancelClicked()
        {
            onCancel?.Invoke();
            isActive = false;
        }
    }
}
