using System.Collections;
using DataStructures.Variables;
using Features.Evaluation.Logic;
using Features.UserData.Survey.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.GameLogic.Logic
{
    public class ResultScreenBehaviour : MonoBehaviour
    {
        [SerializeField][Range(0,5)] private float fillTime = 3f;
        [SerializeField][Range(0,5)] private float waitForTransitionTime = 3f;

        [SerializeField] private Questions_SO questions;
        
        [Header("The 5 Survey Aspects")]
        [Tooltip("SO")] [SerializeField] private IntVariable surveyOpenness;
        [Tooltip("SO")] [SerializeField] private IntVariable surveyConscientiousness;
        [Tooltip("SO")] [SerializeField] private IntVariable surveyExtraversion;
        [Tooltip("SO")] [SerializeField] private IntVariable surveyAgreeableness;
        [Tooltip("SO")] [SerializeField] private IntVariable surveyNeuroticism;

        [Header("The 5 Results for each Survey Aspects")]
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text surveyOpennessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text surveyConscientiousnessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text surveyExtraversionText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text surveyAgreeablenessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text surveyNeuroticismText;

        [Header("The 5 Results for each Survey Aspects")]
        [Tooltip("GameObjectRef")] [SerializeField] private Slider surveyOpennessBar;
        [Tooltip("GameObjectRef")] [SerializeField] private Slider surveyConscientiousnessBar;
        [Tooltip("GameObjectRef")] [SerializeField] private Slider surveyExtraversionBar;
        [Tooltip("GameObjectRef")] [SerializeField] private Slider surveyAgreeablenessBar;
        [Tooltip("GameObjectRef")] [SerializeField] private Slider surveyNeuroticismBar;
        
        [Header("The 5 Game Aspects")]
        [Tooltip("SO")] [SerializeField] private IntVariable gameOpenness;
        [Tooltip("SO")] [SerializeField] private IntVariable gameConscientiousness;
        [Tooltip("SO")] [SerializeField] private IntVariable gameExtraversion;
        [Tooltip("SO")] [SerializeField] private IntVariable gameAgreeableness;
        [Tooltip("SO")] [SerializeField] private IntVariable gameNeuroticism;

        [Header("The 5 Results for each Game Aspects")]
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text gameOpennessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text gameConscientiousnessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text gameExtraversionText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text gameAgreeablenessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text gameNeuroticismText;

        [Header("The 5 Results for each Game Aspects")]
        [Tooltip("GameObjectRef")] [SerializeField] private Slider gameOpennessBar;
        [Tooltip("GameObjectRef")] [SerializeField] private Slider gameConscientiousnessBar;
        [Tooltip("GameObjectRef")] [SerializeField] private Slider gameExtraversionBar;
        [Tooltip("GameObjectRef")] [SerializeField] private Slider gameAgreeablenessBar;
        [Tooltip("GameObjectRef")] [SerializeField] private Slider gameNeuroticismBar;

        public void OnOpenRepoURL()
        {
            Application.OpenURL("https://github.com/ginogeorgiev/WerDuBist");
        }

        private void OnEnable()
        {
            // Apply runtime values of each answered question by the player to their respective IntVariables (Aspects)
            foreach (Question_SO item in questions.Items)
            {
                if (item.GameAspectValue.Equals(gameOpenness))
                {
                    item.GameAspectValue.Add((int)item.IngameRuntimeValue);
                }
                if (item.GameAspectValue.Equals(gameConscientiousness))
                {
                    item.GameAspectValue.Add((int)item.IngameRuntimeValue);
                }
                if (item.GameAspectValue.Equals(gameExtraversion))
                {
                    item.GameAspectValue.Add((int)item.IngameRuntimeValue);
                }
                if (item.GameAspectValue.Equals(gameAgreeableness))
                {
                    item.GameAspectValue.Add((int)item.IngameRuntimeValue);
                }
                if (item.GameAspectValue.Equals(gameNeuroticism))
                {
                    item.GameAspectValue.Add((int)item.IngameRuntimeValue);
                }
            }
            
            StartCoroutine(UpdateResultUI());
        }
        
        private IEnumerator UpdateResultUI()
        {
            // reset values, because default is 1
            surveyOpennessBar.value = 0;
            surveyConscientiousnessBar.value = 0;
            surveyExtraversionBar.value = 0;
            surveyAgreeablenessBar.value = 0;
            surveyNeuroticismBar.value = 0;
            
            gameOpennessBar.value = 0;
            gameConscientiousnessBar.value = 0;
            gameExtraversionBar.value = 0;
            gameAgreeablenessBar.value = 0;
            gameNeuroticismBar.value = 0;
            
            yield return new WaitForSeconds(waitForTransitionTime);
            
            // Lerp values for Survey and update UI accordingly
            StartCoroutine(LerpBarAndValue(surveyOpenness.Get(), surveyOpennessBar, surveyOpennessText));
            StartCoroutine(LerpBarAndValue(surveyConscientiousness.Get(), surveyConscientiousnessBar, surveyConscientiousnessText));
            StartCoroutine(LerpBarAndValue(surveyExtraversion.Get(), surveyExtraversionBar, surveyExtraversionText));
            StartCoroutine(LerpBarAndValue(surveyAgreeableness.Get(), surveyAgreeablenessBar, surveyAgreeablenessText));
            StartCoroutine(LerpBarAndValue(surveyNeuroticism.Get(), surveyNeuroticismBar, surveyNeuroticismText));
            
            // Lerp values for Game and update UI accordingly
            StartCoroutine(LerpBarAndValue(gameOpenness.Get(), gameOpennessBar, gameOpennessText));
            StartCoroutine(LerpBarAndValue(gameConscientiousness.Get(), gameConscientiousnessBar, gameConscientiousnessText));
            StartCoroutine(LerpBarAndValue(gameExtraversion.Get(), gameExtraversionBar, gameExtraversionText));
            StartCoroutine(LerpBarAndValue(gameAgreeableness.Get(), gameAgreeablenessBar, gameAgreeablenessText));
            StartCoroutine(LerpBarAndValue(gameNeuroticism.Get(), gameNeuroticismBar, gameNeuroticismText));
        }
        
        private IEnumerator LerpBarAndValue(float targetValue, Slider bar, TMP_Text text)
        {
            float startTime = Time.time;
            while (Time.time < startTime + fillTime)
            {
                bar.value = Mathf.Lerp(0, NormalizeForBar(targetValue), (Time.time - startTime)/fillTime);
                text.text = (int)Mathf.Lerp(0, NormalizeForText(targetValue), (Time.time - startTime)/fillTime) + " %";
                yield return null;
            }
            
            bar.value = NormalizeForBar(targetValue);
            text.text = (int)NormalizeForText(targetValue) + " %";
        }

        private static float NormalizeForBar(float value)
        {
            // Hard coded for a max value of 50
            value *= 0.02f;
            return value;
        }

        private static float NormalizeForText(float value)
        {
            // Hard coded for a max value of 50
            value *= 2f;
            return value;
        }
    }
}
