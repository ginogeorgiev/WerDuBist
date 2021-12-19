using System.Collections.Generic;
using DataStructures.Variables;
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
        [SerializeField] private BoolVariable isPlayerInConversation;


        private readonly List<ChoiceController> choiceControllers = new List<ChoiceController>();

        //changes to new conversation on button click
        public void Change(DialogQuestion_SO dialogQuestion)
        {
            RemoveChoices();
            this.dialogQuestion = dialogQuestion;
            gameObject.SetActive(true);
            isPlayerInConversation.SetTrue();
            Initialize();
        }

        //Hide Conversation on button click
        public void Hide(DialogConversation_SO dialogConversation)
        {
            RemoveChoices();
            gameObject.SetActive(false);
            isPlayerInConversation.SetFalse();
        }

        //destroys choices buttons
        private void RemoveChoices()
        {
            foreach (ChoiceController c in choiceControllers)
                Destroy(c.gameObject);
            
            choiceControllers.Clear();
        }

        //initialize choiceButtons
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
