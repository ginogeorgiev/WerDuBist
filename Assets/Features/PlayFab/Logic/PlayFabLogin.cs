using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Features.PlayFab.Logic
{
    public class PlayFabLogin : MonoBehaviour
    {
        public void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
                /*
                Please change the titleId below to your own titleId from PlayFab Game Manager.
                If you have already set the value in the Editor Extensions, this can be skipped.
                */
                PlayFabSettings.staticSettings.TitleId = "5BE79";
            }

            string id = (Random.Range(1000, 1000000)).ToString();
            var request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        { }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

        public void SetUserData(Dictionary<string,string> dataToSave) {
            PlayFabClientAPI.UpdateUserData(
                new UpdateUserDataRequest() {
                    Data = dataToSave
                },
                result => {},
                error =>
                {
                    Debug.Log("Error");
                }
            );
        }
    }
}