using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.Logic.EntryPoint
{
    public class BootstrapEntryPoint : IInitializable
    {
        public void Initialize()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}