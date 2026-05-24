using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _createGameButton;
        [SerializeField] private Button _joinGameButton;
        [SerializeField] private Button _exitGameButton;
        
        public Button.ButtonClickedEvent OnCreateGameButtonClicked => _createGameButton.onClick;
        public Button.ButtonClickedEvent OnJoinGameButtonClicked => _joinGameButton.onClick;


        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);
    }
}