using System;
using System.Collections;
using DataStructures.Variables;
using TMPro;
using UnityEngine;

namespace Features.InformationGathering.Logic
{
    public class SaveParticipantInformation : MonoBehaviour
    {
        [SerializeField] private ParticipantInformationVariable participantInformationVariable;
        [SerializeField] private TMP_Text errorMessage;
        [SerializeField] private TMP_Dropdown genderDropdown;
        [SerializeField] private TMP_InputField ageField;
        [SerializeField] private TMP_InputField gameExperienceField;

        [Header("UIs")] 
        [SerializeField] private GameObject infoGatheringUI;
        [SerializeField] private GameObject surveyUI;
        
        // Tracks if the error Coroutine (there no other) is running, to prevent the code to Start another
        private bool coroutineRunning = false;

        public void SetParticipantInformation()
        {
            if (genderDropdown == null || ageField == null || gameExperienceField == null) { return; }
            if (infoGatheringUI == null || surveyUI == null) { return; }

            // Trigger the error message upon no values, invalid values are just being corrected
            if (ageField.text == "" || gameExperienceField.text == "")
            {
                if (!coroutineRunning)
                {
                    StartCoroutine(ShowErrorMessage());
                }
                
                return;
            }

            // Save the age and game experience to show the clamped values in the information gathering window
            var age = 0;
            var gameExp = 0;
            participantInformationVariable.Set(
                genderDropdown.options[genderDropdown.value].text,
                // keep the values for age and game experience in a certain range
                age = Mathf.Clamp(int.Parse(ageField.text),18,99),
                gameExp= Mathf.Clamp(int.Parse(gameExperienceField.text),0,30)
            );

            // Visual representation of the clamping
            ageField.text = age.ToString();
            gameExperienceField.text = gameExp.ToString();
            
            surveyUI.SetActive(true);
            infoGatheringUI.SetActive(false);
        }

        /// <summary>
        /// Function enables the ErrorMessages of the information gathering window,
        /// which demands the user to insert valid values.
        /// </summary>
        IEnumerator ShowErrorMessage()
        {
            coroutineRunning = true;
            errorMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            errorMessage.gameObject.SetActive(false);
            coroutineRunning = false;
        }
    }
}
