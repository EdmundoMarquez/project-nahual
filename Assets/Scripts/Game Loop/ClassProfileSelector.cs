using UnityEngine;
using ProjectNahual.FPCharacter;
using ProjectNahual.Weapons;
using ProjectNahual.Utils;
using System.Linq;

namespace ProjectNahual.GameLoop
{
    public class ClassProfileSelector : MonoBehaviour
    {
        [SerializeField] private ClassProfile[] profiles;
        [SerializeField] private FirstPersonCharacter characterInstaller;
        [SerializeField] private ClassSelectionMenu view;

        private void Start() => OnActivateSelector();

        public void OnActivateSelector()
        {
            foreach(var profile in profiles)
            {
                profile.weaponBehaviour.gameObject.SetActive(false);
            }

            view.ToggleVisibility(true);
            CursorHandler.FreeCursor();
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
            characterInstaller.Init(profile.weaponBehaviour);
            view.ToggleVisibility(false);

            
        }

    }
}
