using UnityEngine;

namespace Johnny.SimDungeon
{
    public class BoxPlacementGhostIndicator : MonoBehaviour
    {
        public Transform indicator;
        public LineRenderer lineRenderer;
        public Vector3 offset = new Vector3(0f,2f,0f);

        public void Set(bool active, bool lineActive, Vector3 position, Vector3 lineTarget)
        {
            indicator.position = position;
            indicator.gameObject.SetActive(active);
            lineRenderer.SetPosition(0, position + offset);
            lineRenderer.SetPosition(1, lineTarget + offset);
            lineRenderer.gameObject.SetActive(lineActive);


        }
    }
}
