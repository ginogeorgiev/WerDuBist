using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Dialog.Logic
{
    public class QuestionController : MonoBehaviour
    {
        [SerializeField] private DialogQuestion_SO dialogQuestion; 
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private Button choiceButton;

        private List<ChoiceController> choiceControllers = new List<ChoiceController>();

        //changes to new conversation on button click
        public void Change(DialogQuestion_SO dialogQuestion)
        {
            RemoveChoices();
            this.dialogQuestion = dialogQuestion;
            gameObject.SetActive(true);
            Initialize();
        }

        //Hide Conversation on button click
        public void Hide(DialogConversation_SO dialogConversation)
        {
            RemoveChoices();
            gameObject.SetActive(false);
        }

        //destroys choise buttons
        private void RemoveChoices()
        {
            foreach (ChoiceController c in choiceControllers)
                Destroy(c.gameObject);
            
            choiceControllers.Clear();
        }

        //initalize choicebuttons
        private void Initialize()
        {
            questionText.text = dialogQuestion.Text; //changes standard text to question text

            for (int index = 0; index < dialogQuestion.Choices.Length; index++)
            {
                ChoiceController c = ChoiceController.AddChoiceButton(choiceButton, dialogQuestion.Choices[index], index);
                choiceControllers.Add(c);
            }
            choiceButton.gameObject.SetActive(false);
        }
    }
}
