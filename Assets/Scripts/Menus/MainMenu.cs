using UnityEngine;
using UnityEngine.UI;
using ProjectNahual.GameLoop;
using ProjectNahual.Utils;

namespace ProjectNahual.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            #if UNITY_WEBGL
            quitButton.gameObject.SetActive(false);
            #endif

            playButton.onClick.AddListener(OnPlayPressed);
            optionsButton.onClick.AddListener(OnOptionsPressed);
            creditsButton.onClick.AddListener(OnCreditsPressed);
            quitButton.onClick.AddListener(OnQuitPressed);

        }

        private void OnPlayPressed()
        {
            if(Game.Instance == null)
            {
                Debug.LogWarning("Game should be instanced from the Preload Scene to load scene. Aborting...");
                return;
            }
            SceneLoader.LoadScene(Game.Instance, "LevelGenerator");
            Game.Instance.StartGame();
        }

        private void OnOptionsPressed()
        {
            
        }

        private void OnCreditsPressed()
        {
            
        }

        private void OnQuitPressed() => Application.Quit();

    }
}
