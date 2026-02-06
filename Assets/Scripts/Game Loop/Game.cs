using ProjectNahual.GameLoop;
using ProjectNahual.PCG;
using ProjectNahual.Utils;
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

        DontDestroyOnLoad(gameObject);
    }

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
