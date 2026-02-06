using UnityEngine;
using UnityEngine.UI;
using ProjectNahual.GameLoop;
using ProjectNahual.Utils;

namespace ProjectNahual.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button quitButton;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            #if UNITY_WEBGL
            quitButton.gameObject.SetActive(false);
            #endif

            _canvasGroup = GetComponent<CanvasGroup>();
            // ToggleVisibility(false);

            continueButton.onClick.AddListener(OnContinuePressed);
            optionsButton.onClick.AddListener(OnOptionsPressed);
            menuButton.onClick.AddListener(OnMenuPressed);
            quitButton.onClick.AddListener(OnQuitPressed);

        }

        private void OnContinuePressed()
        {
            ToggleVisibility(false);
        }

        private void OnOptionsPressed()
        {
            
        }

        private void OnMenuPressed()
        {
            SceneLoader.LoadScene(Game.Instance, "MenuScene");
        }

        private void OnQuitPressed() => Application.Quit();

        public void ToggleVisibility(bool toggle)
        {
            _canvasGroup.alpha = toggle ? 1 : 0;
            _canvasGroup.interactable = toggle;
            _canvasGroup.blocksRaycasts = toggle;
        }

    }
}
