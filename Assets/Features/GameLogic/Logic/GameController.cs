using System.Collections;
using DataStructures.Event;
using DataStructures.Variables;
using Features.Evaluation.Logic;
using UnityEngine;
using Random = System.Random;

namespace Features.GameLogic.Logic
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameEvent_SO onLaunchGame;
        [SerializeField] private GameEvent_SO onLaunchSurvey;
        [SerializeField] private GameEvent_SO onActivateResultScreen;
        
        [SerializeField] private GameEvent_SO onDeActivateStartScreen;
        
        [SerializeField] private GameEvent_SO onDeActivateGameUI;
        [SerializeField] private GameEvent_SO onDeActivateInfoGatheringUI;
        [SerializeField] private GameEvent_SO onDeActivateSurveyUI;
        
        [SerializeField] private BoolVariable isGameLaunched;
        [SerializeField] private BoolVariable isSurveyLaunched;
        
        [SerializeField] private TransitionData transitionData;
        [SerializeField] private float transitionTime = 1f;
        
        [SerializeField] private EvaluationData evaluationData;

        private bool coroutineIsRunning;

        private void Awake()
        {
            isGameLaunched.SetFalse();
            isSurveyLaunched.SetFalse();
        }

        private void Start()
        {
            onDeActivateGameUI.Raise();
            onDeActivateInfoGatheringUI.Raise();
            onDeActivateSurveyUI.Raise();
        }

        public void OnStartButtonPressed()
        {
            Random rand = new Random();

            if (rand.Next(0,2) == 0)
            {
                if (!coroutineIsRunning)
                {
                    isGameLaunched.SetTrue();
                    evaluationData.EvaluationDictionary.Add("Erst Game","-");
                    StartCoroutine(TriggerTransition(onLaunchGame));
                }
            }
            else
            {
                if (!coroutineIsRunning)
                {
                    isSurveyLaunched.SetTrue();
                    evaluationData.EvaluationDictionary.Add("Erst Survey","-");
                    StartCoroutine(TriggerTransition(onLaunchSurvey));
                }
            }
        }

        public void OnSurveyCompleted()
        {
            if (!isGameLaunched.Get())
            {
                StartCoroutine(TriggerTransition(onLaunchGame));
            }
            else
            {
                if (!coroutineIsRunning)
                {
                    StartCoroutine(TriggerTransition(onActivateResultScreen));
                }
            }
        }

        public void OnGameCompleted()
        {
            if (!isSurveyLaunched.Get())
            {
                StartCoroutine(TriggerTransition(onLaunchSurvey));
            }
            else
            {
                if (!coroutineIsRunning)
                {
                    StartCoroutine(TriggerTransition(onActivateResultScreen));
                }
            }
        }

        private IEnumerator TriggerTransition(GameEvent_SO gameEvent)
        {
            coroutineIsRunning = true;
            transitionData.OnStart.Raise();
            yield return new WaitForSeconds(transitionData.FadeInTime);
            
            onDeActivateStartScreen.Raise();
            gameEvent.Raise();
            
            yield return new WaitForSeconds(transitionTime);
            transitionData.OnEnd.Raise();
            coroutineIsRunning = false;
        }
    }
}
