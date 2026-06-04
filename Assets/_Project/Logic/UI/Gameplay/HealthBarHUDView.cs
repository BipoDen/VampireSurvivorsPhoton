using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.UI.Gameplay
{
    public class HealthBarHUDView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        
        public void SetHealthValue(float currentHealth, float maxHealth)
        {
            _slider.value = currentHealth / maxHealth;
        }
    }
}