using System;
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
        [SerializeField] private GameEvent_SO onDeActivateGameUI;
        
        [SerializeField] private BoolVariable isGameLaunched;
        [SerializeField] private BoolVariable isSurveyLaunched;
        
        [SerializeField] private EvaluationData evaluationData;

        private void Awake()
        {
            isGameLaunched.SetFalse();
            isSurveyLaunched.SetFalse();
        }

        private void Start()
        {
            onDeActivateGameUI.Raise();
        }

        public void OnStartButtonPressed()
        {
            Random rand = new Random();

            if (rand.Next(0,2) == 0)
            {
                onLaunchGame.Raise();
                Debug.Log("onLaunchGame Raised");
                isGameLaunched.SetTrue();
                evaluationData.EvaluationDictionary.Add("Erst Game","-");
            }
            else
            {
                onLaunchSurvey.Raise();
                Debug.Log("onLaunchSurvey Raised");
                isSurveyLaunched.SetTrue();
                evaluationData.EvaluationDictionary.Add("Erst Survey","-");
            }
        }

        public void OnSurveyCompleted()
        {
            if (!isGameLaunched.Get())
            {
                onLaunchGame.Raise();
            }
            else
            {
                onActivateResultScreen.Raise();
            }
        }

        public void OnGameCompleted()
        {
            if (!isSurveyLaunched.Get())
            {
                onLaunchSurvey.Raise();
            }
            else
            {
                onActivateResultScreen.Raise();
            }
        }
    }
}
