using UnityEngine;

public class GenerateLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            Game.Instance.GenerateLevel();
        }
    }
}
