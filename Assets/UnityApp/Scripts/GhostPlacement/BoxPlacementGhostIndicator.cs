using UnityEngine;

namespace Johnny.SimDungeon
{
    public class BoxPlacementGhostIndicator : MonoBehaviour
    {
        [SerializeField] private Transform m_Indicator;
        [SerializeField] private LineRenderer m_LineRenderer;
        [SerializeField] private Vector3 m_Offset = new Vector3(0f,2f,0f);
        [SerializeField] private Material m_ValidIndicatorMaterial;
        [SerializeField] private Material m_ValidLineRendererMaterial;
        [SerializeField] private Material m_InvalidIndicatorMaterial;
        [SerializeField] private Material m_InvalidLineRendererrMaterial;

        public void Set(bool active, bool lineActive, Vector3 position, Vector3 lineTarget)
        {
            m_Indicator.position = position;
            m_Indicator.gameObject.SetActive(active);
            m_LineRenderer.SetPosition(0, position + m_Offset);
            m_LineRenderer.SetPosition(1, lineTarget + m_Offset);
            m_LineRenderer.gameObject.SetActive(lineActive);
        }
    }
}
