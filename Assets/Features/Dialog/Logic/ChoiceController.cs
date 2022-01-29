using Features.Quests.Logic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.Dialog.Logic
{
    [System.Serializable]
    public class ConversationChangeEvent : UnityEvent<DialogConversation_SO> {}
    
    public class ChoiceController : MonoBehaviour
    {
        [SerializeField] private Choice choice;
        [SerializeField] private ConversationChangeEvent conversationChangeEvent;
        [SerializeField] [Range(-100, -10)] private int serializedButtonSpacing = -100;

        [SerializeField] private QuestEvent serializedQuestEvent;

        private static int buttonSpacing;
        private static QuestEvent questEvent;
        
        private void Awake()
        {
            buttonSpacing = serializedButtonSpacing;
            questEvent = serializedQuestEvent;
        }

        private void Start()
        {
            if (conversationChangeEvent == null) conversationChangeEvent = new ConversationChangeEvent();
            GetComponent<Button>().GetComponentInChildren<Text>().text = choice.Text;
        }

        //Adds a gameObject (choice button) for every choice there is
        public static ChoiceController AddChoiceButton(Button choiceButtonTemplate, Choice choice, int index)
        {
            Button button = Instantiate(choiceButtonTemplate);
            
            button.transform.SetParent(choiceButtonTemplate.transform.parent);
            button.transform.localScale= Vector3.one;
            button.transform.localPosition = new Vector3(0, index * buttonSpacing, 0);
            button.name = "Choice " + (index + 1);
            button.gameObject.SetActive(true);

            if (choice.Quest != null)
            {
                choice.QuestEvent = questEvent;
                button.onClick.AddListener(choice.OnQuestAccepted);
            }

            if (choice.ChoiceEvents.Count != 0)
            {
                button.onClick.AddListener(choice.OnChoiceEvent);
            }

            if (choice.IngameQuestion != null)
            {
                button.onClick.AddListener(choice.OnAddToEvaluation);
            }

            ChoiceController choiceController = button.GetComponent<ChoiceController>();
            choiceController.choice = choice;
            return choiceController;
        }

        public void MakeChoice()
        {
            conversationChangeEvent.Invoke(choice.DialogConversation);
        }
    }
}

