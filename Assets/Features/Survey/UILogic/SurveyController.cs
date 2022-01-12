using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Variables;
using Features.Evaluation.Logic;
using Features.Survey.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Survey.UILogic
{
    public class SurveyController : MonoBehaviour
    {
        [Header("For UI QuestItem generation")]
        [Tooltip("SO")] [SerializeField] private Questions_SO questionsData;
        [Tooltip("SO")] [SerializeField] private EvaluationData evaluationData;

        private IEnumerable<Question_SO> questions;
        [SerializeField] private bool randomizeQuestions;
        [Tooltip("Prefab")] [SerializeField] private GameObject questionItem;
        [Tooltip("Ref")] [SerializeField] private RectTransform content;

        [Header("For calculating the Result")]
        [SerializeField] private bool autoUpdateResult = true;
        [Tooltip("SO")] [SerializeField] private QuestionItemRuntimeSet questionItemRuntimeSet;

        [Header("The 5 Aspects")]
        [Tooltip("SO")] [SerializeField] private IntVariable extraversion;
        [Tooltip("SO")] [SerializeField] private IntVariable agreeableness;
        [Tooltip("SO")] [SerializeField] private IntVariable conscientiousness;
        [Tooltip("SO")] [SerializeField] private IntVariable neuroticism;
        [Tooltip("SO")] [SerializeField] private IntVariable openness;

        [Header("The 5 Results for each Aspects")]
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text extraversionText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text agreeablenessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text conscientiousnessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text neuroticismText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text opennessText;

        [Header("The 5 Results for each Aspects in percent")]
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text extraversionPercentage;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text agreeablenessPercentage;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text conscientiousnessPercentage;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text neuroticismPercentage;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text opennessPercentage;

        private void Awake()
        {
            questionItemRuntimeSet.Restore();
            
            // Cache questionList and randomize if necessary
            questions = randomizeQuestions ? questionsData.Items.OrderBy(x => Guid.NewGuid()) : questionsData.Items;

            // Fill Scroll-View with Question Items and assign questions to those Items
            foreach (Question_SO question in questions)
            {
                GameObject uiQuestionItem = Instantiate(questionItem, content);
                uiQuestionItem.GetComponent<QuestionItemBehaviour>().Question = question;
            }

            // Move the ResultItem to the last position of the Scroll-View
            content.GetChild(0).SetAsLastSibling();
        }

        public void OnAutoCalculateResult()
        {
            if (!autoUpdateResult) return;
            OnCalculateResult();
        }

        public void OnCalculateResult()
        {
            // Restore values of each Aspect
            extraversion.Restore();
            agreeableness.Restore();
            conscientiousness.Restore();
            neuroticism.Restore();
            openness.Restore();

            // Iterate over answers to sum up a result 
            foreach (QuestionItemBehaviour question in questionItemRuntimeSet.GetItems())
            {
                foreach (Toggle toggle in question.Toggles)
                {
                    int value = -1;
                    // for positive keyed questions
                    if (question.Question.Key)
                    {
                        switch (toggle.gameObject.name)
                        {
                            case "Agree" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(5);
                                value = 5;
                                break;
                            case "PartlyAgree" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(4);
                                value = 4;
                                break;
                            case "Neither" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(3);
                                value = 3;
                                break;
                            case "PartlyDisagree" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(2);
                                value = 2;
                                break;
                            case "Disagree" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(1);
                                value = 1;
                                break;
                        }
                    }
                    // for negative keyed questions
                    else
                    {
                        switch (toggle.gameObject.name)
                        {
                            case "Disagree" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(5);
                                value = 5;
                                break;
                            case "PartlyDisagree" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(4);
                                value = 4;
                                break;
                            case "Neither" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(3);
                                value = 3;
                                break;
                            case "PartlyAgree" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(2);
                                value = 2;
                                break;
                            case "Agree" when toggle.isOn:
                                question.Question.SurveyAspectValue.Add(1);
                                value = 1;
                                break;
                        }
                    }
                    evaluationData.Add(question.Question.SurveyAspectValue.name + "_" + question.Question.Question, value.ToString());
                }
            }

            UpdateResultUI();
        }

        private void UpdateResultUI()
        {
            // Update Values
            extraversionText.text = extraversion.Get().ToString();
            agreeablenessText.text = agreeableness.Get().ToString();
            conscientiousnessText.text = conscientiousness.Get().ToString();
            neuroticismText.text = neuroticism.Get().ToString();
            opennessText.text = openness.Get().ToString();

            // Update Percentages
            extraversionPercentage.text = extraversion.Get() * 2.5f + "%";
            agreeablenessPercentage.text = agreeableness.Get() * 2.5f + "%";
            conscientiousnessPercentage.text = conscientiousness.Get() * 2.5f + "%";
            neuroticismPercentage.text = neuroticism.Get() * 2.5f + "%";
            opennessPercentage.text = openness.Get() * 2.5f + "%";
        }

        public void ResetSurvey()
        {
            // Restore values of each Aspect
            extraversion.Restore();
            agreeableness.Restore();
            conscientiousness.Restore();
            neuroticism.Restore();
            openness.Restore();

            // Iterate over answers to reset toggles
            foreach (Toggle toggle in questionItemRuntimeSet.GetItems().SelectMany(question => question.Toggles))
            {
                toggle.isOn = false;
            }
            
            UpdateResultUI();
        }
    }
}
