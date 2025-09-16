using UnityEngine;

namespace Johnny.SimDungeon
{
    public class WallTest : MonoBehaviour
    {
        [Header("隐藏角度阈值")]
        public float angleThreshold = 60f; // 相机方向与墙法线夹角小于这个值就隐藏

        private Transform mainCam;
        public GameObject model;

        private void Start()
        {
            if (Camera.main != null)
                mainCam = Camera.main.transform;
            else
                Debug.LogError("场景中没有主相机！");
        }

        private void Update()
        {
            if (mainCam == null) return;

            UpdateVisibilityBasedOnCamera();
        }

        private void UpdateVisibilityBasedOnCamera()
        {
            Vector3 toCam = mainCam.position - transform.position;
            toCam.y = 0f; // 忽略Y
            toCam.Normalize();

            // 墙的正向（forward）
            Vector3 wallForward = transform.forward;
            wallForward.y = 0f;
            wallForward.Normalize();

            // Dot Product判断相机是否在墙前方
            float dot = Vector3.Dot(toCam, wallForward);

            // 相机在墙前方且超过阈值才隐藏
            bool shouldHide = dot > angleThreshold;


            model.SetActive(!shouldHide);
        }
    }
}
