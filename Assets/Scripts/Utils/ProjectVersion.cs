using UnityEngine;
using TMPro;
namespace ProjectNahual.Utils
{
    public class ProjectVersion : MonoBehaviour
    {
        private TMP_Text versionText;
        void Awake()
        {
            versionText = GetComponent<TMP_Text>();
            versionText.SetText("V. " + Application.version);
        }
    }
}