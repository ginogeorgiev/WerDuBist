using System;
using System.Collections;
using System.Collections.Generic;
using Features.Evaluation.Logic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Features.PlayFab.Logic
{
    public class PlayFabLogin : MonoBehaviour
    {
        [SerializeField] private EvaluationData evalData;
        [SerializeField] private int throttleTime = 10;
        [SerializeField] private bool disableSendData = false;
        [SerializeField] private bool disableCharacterCreation = false;
        
        private bool coroutineRunning;

        public void Start()
        {
            if (disableCharacterCreation) return;
            
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
                /*
                Please change the titleId below to your own titleId from PlayFab Game Manager.
                If you have already set the value in the Editor Extensions, this can be skipped.
                */
                PlayFabSettings.staticSettings.TitleId = "5BE79";
            }

            string id = Guid.NewGuid().ToString();
            var request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        { }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

        public void OnEvaluationDictionaryChanged()
        {
            if (!coroutineRunning && !disableSendData)
            {
                StartCoroutine(RequestThrottle());
            }
        }
        
        public void SetUserData() {
            if(evalData == null || evalData.EvaluationDictionary == null || disableSendData) { return; }
            
            PlayFabClientAPI.UpdateUserData(
                new UpdateUserDataRequest() {
                    Data = evalData.EvaluationDictionary
                },
                result =>
                {
                    Debug.Log("Sending data.");
                },
                error =>
                {
                    Debug.LogError("Send User Data Error");
                    Debug.LogError(error.GenerateErrorReport());
                }
            );
        }
        
        private IEnumerator RequestThrottle()
        {
            coroutineRunning = true;
            yield return new WaitForSeconds(throttleTime);
            SetUserData();
            coroutineRunning = false;
        }
    }
}