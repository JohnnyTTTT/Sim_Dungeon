using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class IconButton : MonoBehaviour
    {
        [SerializeField] private Image m_NormalImage;
        [SerializeField] private Image[] m_Icons;
        [SerializeField] private Color m_SelectColor;
        public void SetIcon(Sprite sprite)
        {
            foreach (var item in m_Icons)
            {
                item.sprite = sprite;
            }
        }
    }
}
