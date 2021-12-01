using System.Collections;
using System.Collections.Generic;
using DataStructures.Event;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.GameLogic.Logic
{
    public class GameSceneLoader : MonoBehaviour
    {
        [Header("Loading Screen")]
        public GameObject loadingScreen;
        public Image fillAmount;
        public TextMeshProUGUI loadingPercentAmount;
        
        private float totalSceneProgress;
        private int totalScenesLoaded;
        
        private readonly List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
        
        [SerializeField] private GameEvent_SO onLoadCompleted;

        private void Awake()
        {
            loadingScreen.gameObject.SetActive(true);

            if (!SceneManager.GetSceneByBuildIndex(1).isLoaded);
            {
                scenesToLoad.Add(SceneManager.LoadSceneAsync("Game_Scene", LoadSceneMode.Additive));
            }

            StartCoroutine(GetSceneLoadProgress());
        }

        private void UpdateLoadingScreenProgress(AsyncOperation async)
        {
            totalScenesLoaded++;
                
            totalSceneProgress = (float)totalScenesLoaded / (scenesToLoad.Count + 1);
                
            fillAmount.fillAmount = totalSceneProgress;
            loadingPercentAmount.text = $"{(int)(totalSceneProgress * 100)} %";
        }
        
        private IEnumerator GetSceneLoadProgress()
        {
            //Loading scenes
            totalSceneProgress = 0;
            totalScenesLoaded = 0;
            
            foreach (AsyncOperation loadingScene in scenesToLoad)
            {
                loadingScene.completed += UpdateLoadingScreenProgress;
            }
            
            yield return new WaitUntil(() => totalScenesLoaded == scenesToLoad.Count);

            onLoadCompleted.Raise();
            
            //reset the loading screen
            loadingScreen.gameObject.SetActive(false);
            fillAmount.fillAmount = 0;
            loadingPercentAmount.text = "0 %";
            
            SceneManager.UnloadSceneAsync(0);
        }
    }
}
