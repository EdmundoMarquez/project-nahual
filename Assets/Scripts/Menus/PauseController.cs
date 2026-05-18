using UnityEngine;
using ProjectNahual.Input;
using ProjectNahual.Utils;
using ProjectNahual.GameLoop;

namespace ProjectNahual.Menus
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private PauseMenu pauseMenu = null;
        private IMenuInput _menuInput;
        private bool _pauseActive = false;

        private void OnEnable()
        { 
            _menuInput = Registry<IMenuInput>.GetFirst();
            if(_menuInput == null)
            {
                Debug.LogWarning("No IMenuInput class registered!");
                return;
            }

            _menuInput.PausePressed += OnPausePressed;
            _menuInput.BackPressed += OnBackPressed;
            pauseMenu.ExitPause += TogglePause;
        }
        
        private void OnDisable() 
        { 
            if(_menuInput == null) return;
            _menuInput.PausePressed -= OnPausePressed;
            _menuInput.BackPressed -= OnBackPressed;
        }

        private void OnPausePressed() => TogglePause();
        private void OnBackPressed()
        {
            if(_pauseActive)
            {
                TogglePause();
            }
        }

        private void TogglePause()
        {
            _pauseActive = !_pauseActive;
            pauseMenu.ToggleVisibility(_pauseActive);
            
            Game.Instance.Pause(_pauseActive);
        }

    }
}

