using UnityEngine;
using ProjectNahual.Utils;

namespace ProjectNahual.GameLoop
{
    public class Preload : MonoBehaviour
    {
        private void Start()
        {
            SceneLoader.LoadScene(Game.Instance, "MenuScene");
        }
    }

}

