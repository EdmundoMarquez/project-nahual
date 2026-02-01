using ProjectNahual.GameLoop;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private ClassProfileSelector classProfileSelector;
    public static Game Instance;

    private void Awake()
    {
        if(Instance != null) return;
        Instance = this;
    }

    public void GenerateLevel()
    {
        classProfileSelector.OnActivateSelector();
    }
}
