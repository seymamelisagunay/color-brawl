using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        public bool Visible => canvas.enabled;
        public Canvas canvas;
        [SerializeField] private Slider loadingBar;
        [SerializeField] private TMP_Text buildIndex;
        private float _targetProgress;
        private float _speed = 0.2f;

        private void Awake()
        {
            Instance = this;
            buildIndex.SetText($"v{Application.version}");
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if (Visible)
            {
                loadingBar.value = Mathf.MoveTowards(loadingBar.value, _targetProgress, Time.deltaTime * 0.5f);
            }
        }

        public void Load(string sceneName, bool autoShowHide = true, float delay = 0,
            Action<AsyncOperation> onLoadScene = null)
        {
            StartCoroutine(SceneChangeProgress(sceneName, autoShowHide, delay, onLoadScene));
        }

        private IEnumerator SceneChangeProgress(string sceneName, bool autoShowHide, float delay,
            Action<AsyncOperation> onLoadScene)
        {
            yield return new WaitForSeconds(delay);
            if (autoShowHide) Show();
            var task = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!task.isDone)
            {
                _targetProgress = Mathf.Max(_targetProgress, task.progress);
                yield return new WaitForEndOfFrame();
            }

            _targetProgress = 1;
            yield return new WaitUntil(() => loadingBar.value >= 1);
            if (autoShowHide) Hide();
            onLoadScene?.Invoke(task);
        }


        public void Show()
        {
            if (Visible) return;
            _targetProgress = 0;
            loadingBar.value = 0;
            canvas.enabled = true;
        }

        public void Hide()
        {
            canvas.enabled = false;
        }

        public void UnLoad(string scene, Action<AsyncOperation> done = null)
        {
            var task = SceneManager.UnloadSceneAsync(scene);
            if (done != null)
                task.completed += done;
        }
    }
}