using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public sealed class SceneLoader : MonoBehaviour
    {
        public bool IsLoading { get; private set; }

        public Coroutine LoadAsync(string sceneName, Action onComplete = null)
        {
            return StartCoroutine(LoadRoutine(sceneName, onComplete));
        }

        IEnumerator LoadRoutine(string sceneName, Action onComplete)
        {
            if (IsLoading)
            {
                Debug.LogWarning($"[SceneLoader] Already loading; ignored request for '{sceneName}'.");
                yield break;
            }

            IsLoading = true;
            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (op == null)
            {
                Debug.LogError($"[SceneLoader] Scene '{sceneName}' not found in build settings.");
                IsLoading = false;
                yield break;
            }

            while (!op.isDone) yield return null;
            IsLoading = false;
            onComplete?.Invoke();
        }
    }
}
