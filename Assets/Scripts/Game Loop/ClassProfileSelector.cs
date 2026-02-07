using UnityEngine;
using ProjectNahual.FPCharacter;
using ProjectNahual.Weapons;
using ProjectNahual.Utils;
using System.Linq;
using System.Collections;

namespace ProjectNahual.GameLoop
{
    public class ClassProfileSelector : MonoBehaviour
    {
        [SerializeField] private ClassProfile[] profiles;
        [SerializeField] private ClassSelectionMenu view;
        [SerializeField] private IPlayerCharacter playerCharacter;

        // private void Start() => ActivateSelector();
        private Coroutine SelectorCoroutine;

        private void OnEnable() => Game.GamePaused += OnGamePaused; 
        private void OnDisable() => Game.GamePaused -= OnGamePaused; 

        private void OnGamePaused(bool state) => playerCharacter.SetState(!state);

        public void OnFinishLevel()
        {
            playerCharacter.Reset();
            ActivateSelector();
        }

        public void ActivateSelector()
        {
            // foreach(var profile in profiles)
            // {
            //     profile.weaponBehaviour.gameObject.SetActive(false);
            // }
            playerCharacter = Registry<IPlayerCharacter>.GetFirst();

            view.ToggleVisibility(true);
            CursorHandler.FreeCursor();
        }

        public void ActivateSelectorByTimer(Coroutine waitCoroutine)
        {
            if(SelectorCoroutine != null)
                StopCoroutine(SelectorCoroutine);
            SelectorCoroutine = StartCoroutine(ActivateSelectorTimer(waitCoroutine));
        }

        IEnumerator ActivateSelectorTimer(Coroutine waitCoroutine)
        {
            yield return waitCoroutine;
            ActivateSelector();
        }


        public void SelectProfile(string profileId)
        {
            var profile = profiles.FirstOrDefault(x => x.classId == profileId);
            if(profile == null)
            {
                Debug.LogWarning("Profile id is invalid. Please check your id configuration.");
                return;
            }

            profile.weaponBehaviour.gameObject.SetActive(true);
            playerCharacter.Init(profile.weaponBehaviour);
            view.ToggleVisibility(false);
        }

    }
}
