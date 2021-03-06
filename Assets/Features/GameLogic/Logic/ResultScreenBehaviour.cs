using System.Collections;
using DataStructures.Event;
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

        [SerializeField] private Image postCardImage;
        [SerializeField] private Sprite postCardStay;
        [SerializeField] private Sprite postCardLeave;

        [SerializeField] private float surveyBarMultiplier = 0.025f;
        [SerializeField] private float surveyTextMultiplier = 2.5f;
        [SerializeField] private float gameBarMultiplier = 1f / 8f;
        [SerializeField] private float gameTextMultiplier = 100f / 8f;

        [SerializeField] private Questions_SO questions;
        [SerializeField] private EvaluationData evaluationData;
        
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

        public void OnPlayerStays()
        {
            // Set correct Sprite for EndScreen according to player decision
            if (postCardStay != null)
            {
                postCardImage.sprite = postCardStay;
            }
        }

        public void OnPlayerLeaves()
        {
            // Set correct Sprite for EndScreen according to player decision
            if (postCardLeave != null)
            {
                postCardImage.sprite = postCardLeave;
            }
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

            surveyOpenness.Add(-10);
            surveyConscientiousness.Add(-10);
            surveyExtraversion.Add(-10);
            surveyAgreeableness.Add(-10);
            surveyNeuroticism.Add(-10);
            
            gameOpenness.Add(-4);
            gameConscientiousness.Add(-4);
            gameExtraversion.Add(-4);
            gameAgreeableness.Add(-4);
            gameNeuroticism.Add(-4);
            
            // Lerp values for Survey and update UI accordingly
            StartCoroutine(LerpBarAndValue(surveyOpenness, surveyOpennessBar, surveyOpennessText, surveyBarMultiplier, surveyTextMultiplier));
            StartCoroutine(LerpBarAndValue(surveyConscientiousness, surveyConscientiousnessBar, surveyConscientiousnessText, surveyBarMultiplier, surveyTextMultiplier));
            StartCoroutine(LerpBarAndValue(surveyExtraversion, surveyExtraversionBar, surveyExtraversionText, surveyBarMultiplier, surveyTextMultiplier));
            StartCoroutine(LerpBarAndValue(surveyAgreeableness, surveyAgreeablenessBar, surveyAgreeablenessText, surveyBarMultiplier, surveyTextMultiplier));
            StartCoroutine(LerpBarAndValue(surveyNeuroticism, surveyNeuroticismBar, surveyNeuroticismText, surveyBarMultiplier, surveyTextMultiplier));
            
            // Lerp values for Game and update UI accordingly
            StartCoroutine(LerpBarAndValue(gameOpenness, gameOpennessBar, gameOpennessText, gameBarMultiplier, gameTextMultiplier));
            StartCoroutine(LerpBarAndValue(gameConscientiousness, gameConscientiousnessBar, gameConscientiousnessText, gameBarMultiplier, gameTextMultiplier));
            StartCoroutine(LerpBarAndValue(gameExtraversion, gameExtraversionBar, gameExtraversionText, gameBarMultiplier, gameTextMultiplier));
            StartCoroutine(LerpBarAndValue(gameAgreeableness, gameAgreeablenessBar, gameAgreeablenessText, gameBarMultiplier, gameTextMultiplier));
            StartCoroutine(LerpBarAndValue(gameNeuroticism, gameNeuroticismBar, gameNeuroticismText, gameBarMultiplier, gameTextMultiplier));
            
            yield return new WaitForSeconds(fillTime + 1f);
            
            evaluationData.GenerateUserDataDictionary();
        }
        
        private IEnumerator LerpBarAndValue(IntVariable targetValue, Slider bar, TMP_Text text, float barMultiplier, float textMultiplier)
        {
            targetValue.Set(Mathf.Clamp(targetValue.Get(), 0, 100));
            float startTime = Time.time;
            while (Time.time < startTime + fillTime)
            {
                bar.value = Mathf.Lerp(0, Normalize(targetValue.Get(), barMultiplier), (Time.time - startTime)/fillTime);
                text.text = (int)Mathf.Lerp(0, Normalize(targetValue.Get(), textMultiplier), (Time.time - startTime)/fillTime) + " %";
                yield return null;
            }
            
            bar.value = Normalize(targetValue.Get(), barMultiplier);
            text.text = (int)Normalize(targetValue.Get(), textMultiplier) + " %";
            
            evaluationData.Add(targetValue.name, ((int)Normalize(targetValue.Get(), textMultiplier)).ToString(), false);
        }

        private static float Normalize(float value, float multiplier)
        {
            value *= multiplier;
            if (multiplier < 1f)
            {
                return Mathf.Clamp(value,0,1);
            }
            else
            {
                return Mathf.Clamp(value,0,100);
            }
        }
    }
}
