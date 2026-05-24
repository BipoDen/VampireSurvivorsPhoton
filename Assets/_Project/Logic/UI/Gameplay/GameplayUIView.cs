using TMPro;
using UnityEngine;

namespace _Project.Logic.UI.Gameplay
{
    public class GameplayUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _sessionID;

        public void SetSessionID(string sessionID)
        {
            _sessionID.text = sessionID;
        }
    }
}