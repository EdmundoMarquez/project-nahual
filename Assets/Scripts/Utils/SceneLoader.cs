using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectNahual.Utils
{
    public static class SceneLoader
    {

        public static event Action OnSceneLoad;
        public static event Action OnSceneLoaded;
        public static event Action<float> OnSceneLoadUpdate;
        private static Coroutine LoadCoroutine;
        private readonly static WaitForSeconds _waitForSeconds1 = new WaitForSeconds(1f);

        //You should pass the Game singleton as a runner and the sceneName!
        public static void LoadScene(MonoBehaviour gameRunner, string sceneName)
        {
            OnSceneLoad?.Invoke();

            if(LoadCoroutine != null)
            {
                gameRunner.StopCoroutine(LoadCoroutine);
            }
            LoadCoroutine = gameRunner.StartCoroutine(LoadSceneAsync(sceneName));
        }
        
        private static IEnumerator LoadSceneAsync(string sceneName)
        {
            yield return _waitForSeconds1;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            while(!asyncOperation.isDone)
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / .9f);
                OnSceneLoadUpdate?.Invoke(progress);

                if (asyncOperation.progress >= 0.9f)
                {
                    yield return _waitForSeconds1;
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
            OnSceneLoaded?.Invoke();
        }
    }
}

