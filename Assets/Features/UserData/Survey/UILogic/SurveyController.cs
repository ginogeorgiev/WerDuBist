using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Event;
using DataStructures.Variables;
using Features.Evaluation.Logic;
using Features.UserData.Survey.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UserData.Survey.UILogic
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
        [Tooltip("Ref")] [SerializeField] private ScrollRect scrollView;
        [Tooltip("Ref")] [SerializeField] private float scrollTime = 0.4f;
        [Tooltip("Ref")] [SerializeField] private float scrollValue = 0.021f;

        [Header("For calculating the Result")]
        [SerializeField] private bool autoUpdateResult = true;
        [SerializeField] private GameEvent_SO onSurveyCompleted;
        [SerializeField] private GameEvent_SO onSendUserData;
        [Tooltip("SO")] [SerializeField] private QuestionItemRuntimeSet questionItemRuntimeSet;
        
        [Header("The 5 Aspects")]
        [Tooltip("SO")] [SerializeField] private IntVariable extraversion;
        [Tooltip("SO")] [SerializeField] private IntVariable agreeableness;
        [Tooltip("SO")] [SerializeField] private IntVariable conscientiousness;
        [Tooltip("SO")] [SerializeField] private IntVariable neuroticism;
        [Tooltip("SO")] [SerializeField] private IntVariable openness;
        
        [Header("ResultItem References")]
        [Tooltip("Ref")] [SerializeField] private Button submitResult_Button;
        [Tooltip("Ref")] [SerializeField] private GameObject info_Text;
        
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
            
            submitResult_Button.interactable = false;
            info_Text.SetActive(true);

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

            // Adjust scroll position after selecting a answer
            StartCoroutine(MoveScrollView());

            OnValidateResult();
        }

        private IEnumerator MoveScrollView()
        {
            float startTime = Time.time;
            Vector2 newPos = new Vector2(0, scrollView.normalizedPosition.y - scrollValue);
            while (Time.time < startTime + scrollTime)
            {
                scrollView.normalizedPosition = Vector2.Lerp(scrollView.normalizedPosition, newPos, (Time.time - startTime)/scrollTime);
                yield return null;
            }

            scrollView.normalizedPosition = newPos;
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
        }
        
        private void OnValidateResult()
        {
            foreach (QuestionItemBehaviour question in questionItemRuntimeSet.GetItems())
            {
                bool atLeastOneToggleIsOn = false;
                foreach (Toggle toggle in question.Toggles.Where(toggle => toggle.isOn))
                {
                    atLeastOneToggleIsOn = true;
                }

                if (atLeastOneToggleIsOn) continue;
                
                submitResult_Button.interactable = false;
                info_Text.SetActive(true);
                return;
            }
            
            submitResult_Button.interactable = true;
            info_Text.SetActive(false);
        }

        public void OnSubmitSurveyResult()
        {
            onSendUserData.Raise();

            onSurveyCompleted.Raise();
        }
    }
}
