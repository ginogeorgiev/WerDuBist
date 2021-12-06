using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Dialog.Logic
{
    public class QuestionController : MonoBehaviour
    {
        [SerializeField] private Question question; 
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private Button choiceButton;

        private List<ChoiceController> choiceControllers = new List<ChoiceController>();

        //changes to new conversation on button click
        public void Change(Question _question)
        {
            RemoveChoices();
            question = _question;
            gameObject.SetActive(true);
            Initialize();
        }

        //Hide Conversation on button click
        public void Hide(Conversation conversation)
        {
            RemoveChoices();
            gameObject.SetActive(false);
        }

        //destroys choise buttons
        public void RemoveChoices()
        {
            foreach (ChoiceController c in choiceControllers)
                Destroy(c.gameObject);
            
            choiceControllers.Clear();
        }

        //initalize choicebuttons
        private void Initialize()
        {
            questionText.text = question.text; //changes standard text to question text

            for (int index = 0; index < question.choices.Length; index++)
            {
                ChoiceController c = ChoiceController.AddChoiceButton(choiceButton, question.choices[index], index);
                choiceControllers.Add(c);
            }
            choiceButton.gameObject.SetActive(false);
        }
    }
}
