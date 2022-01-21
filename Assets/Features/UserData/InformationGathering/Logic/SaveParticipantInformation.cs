using System.Collections;
using DataStructures.Event;
using DataStructures.Variables;
using Features.Evaluation.Logic;
using TMPro;
using UnityEngine;

namespace Features.UserData.InformationGathering.Logic
{
    public class SaveParticipantInformation : MonoBehaviour
    {
        [SerializeField] private EvaluationData evaluationData;
            
        [SerializeField] private ParticipantInformationVariable participantInformationVariable;
        [SerializeField] private TMP_Text errorMessage;
        [SerializeField] private TMP_Dropdown genderDropdown;
        [SerializeField] private TMP_InputField ageField;
        [SerializeField] private TMP_Dropdown gameExperienceDropdown;

        [Header("UIs")] 
        [SerializeField] private GameEvent_SO activateInfoGathering;
        [SerializeField] private GameEvent_SO activateSurvey;
        
        // Tracks if the error Coroutine (there no other) is running, to prevent the code to Start another
        private bool coroutineRunning = false;

        public void SetParticipantInformation()
        {
            if (genderDropdown == null || ageField == null || gameExperienceDropdown == null) { return; }
            if (activateInfoGathering == null || activateSurvey == null) { return; }

            // Trigger the error message upon no values, invalid values are just being corrected
            if (ageField.text == "" || (!string.IsNullOrEmpty(ageField.text) && int.Parse(ageField.text) < 18))
            {
                if (!coroutineRunning)
                {
                    StartCoroutine(ShowErrorMessage());
                }
                
                return;
            }

            // Save the age and game experience to show the clamped values in the information gathering window
            var age = 0;
            participantInformationVariable.Set(
                genderDropdown.options[genderDropdown.value].text,
                // keep the values for age and game experience in a certain range
                age = Mathf.Clamp(int.Parse(ageField.text),18,99),
                gameExperienceDropdown.options[gameExperienceDropdown.value].text
            );

            // Visual representation of the clamping
            ageField.text = age.ToString();
            
            evaluationData.Add("Alter", age.ToString());
            evaluationData.Add("Geschlecht", genderDropdown.options[genderDropdown.value].text);
            evaluationData.Add("Spielerfahrung", gameExperienceDropdown.options[gameExperienceDropdown.value].text);
            
            activateSurvey.Raise();
            activateInfoGathering.Raise();
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

        public void LimitAgeInput(string inputText)
        {
            var age = 0;
            if (!string.IsNullOrEmpty(inputText))
            {
                age = int.Parse(inputText);
            }
            
            if (age > 99)
            {
                age = 99;
            }

            ageField.text = age == 0 ? ageField.text: age.ToString();
        }
    }
}
