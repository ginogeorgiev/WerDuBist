using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Dialog.Logic
{
    public class QuestionController : MonoBehaviour
    {
        public Question question; //change to private later
        public TMP_Text questionText;
        public Button choiceButton;

        private List<ChoiceController> choiceControllers = new List<ChoiceController>();

        public void Change(Question _question)
        {
            RemoveChoices();
            question = _question;
            gameObject.SetActive(true);
            Initialize();
        }

        public void Hide(Conversation conversation)
        {
            RemoveChoices();
            gameObject.SetActive(false);
        }

        public void RemoveChoices()
        {
            foreach (ChoiceController c in choiceControllers)
                Destroy(c.gameObject);
            
            choiceControllers.Clear();
        }

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
