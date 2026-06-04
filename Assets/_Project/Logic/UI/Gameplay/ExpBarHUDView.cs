using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.UI.Gameplay
{
    public class ExpBarHUDView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        
        public void SetExpValue(float currentExp, float maxExp)
        {
            _slider.value = currentExp / maxExp;
        }
    }
}