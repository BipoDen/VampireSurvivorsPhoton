using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.UI.MainMenu
{
    public class JoinSessionView : MonoBehaviour
    {
        [SerializeField] private Button _joinSessionButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TMP_InputField sessionIDField;

        public Button.ButtonClickedEvent OnJoinSessionButtonClicked => _joinSessionButton.onClick;
        public Button.ButtonClickedEvent OnBackButtonClicked => _backButton.onClick;
        public string SessionID => sessionIDField.text;
        
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}