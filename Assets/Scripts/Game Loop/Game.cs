using System;
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

    public static event Action GameStarted;
    public static event Action<bool> GamePaused;

    private void Awake()
    {
        if(Instance != null) return;
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        if (Game.Instance == null)
        {
            Debug.LogWarning("Game should be instanced from the Preload Scene to load scene. Aborting...");
            return;
        }
        SceneLoader.LoadScene(Game.Instance, "LevelGenerator");
        SceneLoader.OnSceneLoaded += LoadLevel;
    }

    private void LoadLevel()
    {
        levelGenerator = Registry<LevelGenerator>.GetFirst();

        if(levelGenerator == null)
        {
            Debug.LogWarning("Level generator couldn't be found in Registry. Aborting...");
            return;
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

    public void Pause(bool state) => GamePaused?.Invoke(state);
}
