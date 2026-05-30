using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.UI.Gameplay
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Slider _slider;

        public void SetScreenPosition(Transform targetTransform)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
            Vector2 position = new Vector2(
                screenPosition.x - Screen.width / 2, 
                screenPosition.y - Screen.height / 2);
            _rectTransform.anchoredPosition = position;
        }
    }
}