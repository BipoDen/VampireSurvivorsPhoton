using UnityEngine.SceneManagement;

namespace _Project.Logic.Services
{
    public class SceneLoader
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}