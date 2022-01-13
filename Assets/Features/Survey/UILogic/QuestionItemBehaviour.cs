using System.Collections.Generic;
using Features.Evaluation.Logic;
using Features.Survey.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Survey.UILogic
{
    public class QuestionItemBehaviour : MonoBehaviour
    {
        [SerializeField] private QuestionItemRuntimeSet questionItemRuntimeSet;
        [SerializeField] private List<Toggle> toggles;
        [SerializeField] private Question_SO question;
        [SerializeField] private TMP_Text questionText;

        public List<Toggle> Toggles => toggles;

        public Question_SO Question
        {
            get => question;
            set => question = value;
        }

        private void OnEnable()
        {
            questionItemRuntimeSet.Add(this);
        }
        
        private void OnDisable()
        {
            questionItemRuntimeSet.Remove(this);
        }
        
        private void OnDestroy()
        {
            questionItemRuntimeSet.Remove(this);
        }

        private void Start()
        {
            questionText.text = question.Question;
        }
    }
}