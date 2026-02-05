using log4net.Core;
using ProjectNahual.GameLoop;
using ProjectNahual.PCG;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private ClassProfileSelector classProfileSelector = null;
    [SerializeField] private LevelGenerator levelGenerator = null;
    public static Game Instance;

    private void Awake()
    {
        if(Instance != null) return;
        Instance = this;
    }

    private void Start() => StartGame(); //TODO: Move this function call to Main Menu

    public void StartGame()
    {
        var waitCoroutine = levelGenerator?.GenerateLevel();
        classProfileSelector?.ActivateSelectorByTimer(waitCoroutine);
    }

    public void OnFinishLevel()
    {
        classProfileSelector?.OnFinishLevel();
        levelGenerator?.GenerateLevel();
    }
}
