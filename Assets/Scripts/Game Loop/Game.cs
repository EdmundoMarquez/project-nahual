using ProjectNahual.GameLoop;
using ProjectNahual.PCG;
using ProjectNahual.Utils;
using Unity.VectorGraphics;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private ClassProfileSelector classProfileSelector = null;
    private LevelGenerator levelGenerator = null;
    public static Game Instance;

    private void Awake()
    {
        if(Instance != null) return;
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void StartGame() => SceneLoader.OnSceneLoaded += LoadLevel;

    private void LoadLevel()
    {
        levelGenerator = Registry<LevelGenerator>.GetFirst();

        if(levelGenerator == null)
        {
            Debug.LogWarning("Level generator couldn't be found in Registry. Aborting...");
        }

        var waitCoroutine = levelGenerator?.GenerateLevel();
        classProfileSelector?.ActivateSelectorByTimer(waitCoroutine);

        SceneLoader.OnSceneLoaded -= LoadLevel;
    }

    public void OnFinishLevel()
    {
        classProfileSelector?.OnFinishLevel();
        levelGenerator?.GenerateLevel();
    }
}
