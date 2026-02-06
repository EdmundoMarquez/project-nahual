using UnityEngine;
using UnityEngine.UI;
using ProjectNahual.Utils;

namespace ProjectNahual.Menus
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider progressBar = null;
        private CanvasGroup _canvasGroup;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if(_canvasGroup == null)
            {
                Debug.LogWarning("Canvas group is not set in the loading screen. So it will not show up.");
                return;
            }

            SceneLoader.OnSceneLoad += OnSceneLoad;
            SceneLoader.OnSceneLoadUpdate += OnSceneLoadUpdate;
            SceneLoader.OnSceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneLoader.OnSceneLoad -= OnSceneLoad;
            SceneLoader.OnSceneLoadUpdate -= OnSceneLoadUpdate;
            SceneLoader.OnSceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoad() => ToggleVisibility(true);

        private void OnSceneLoaded() => ToggleVisibility(false);

        private void OnSceneLoadUpdate(float progress)
        {
            // Debug.Log("Update progress: " + progress * 100f + "%");
            progressBar.value = progress;
        }

        public void ToggleVisibility(bool toggle)
        {
            _canvasGroup.alpha = toggle ? 1 : 0;
            _canvasGroup.interactable = toggle;
            _canvasGroup.blocksRaycasts = toggle;
            progressBar.value = 0;
        }
    }
}


