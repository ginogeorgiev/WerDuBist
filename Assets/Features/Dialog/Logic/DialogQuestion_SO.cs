using DataStructures.Event;
using Features.Quests.Logic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Features.Dialog.Logic
{
    [CreateAssetMenu(fileName = "NewDialogQuestion", menuName = "Feature/Dialog/DialogQuestion")]
    public class DialogQuestion_SO : ScriptableObject
    {
        [TextArea(2, 5)]
        [SerializeField] private  string text;

        [SerializeField] private  Choice[] choices;

        public string Text => text;

        public Choice[] Choices => choices;
    }
    
    [System.Serializable]
    public struct Choice
    {
        [TextArea(2, 5)]
        [SerializeField] private string text;

        [SerializeField] private DialogConversation_SO dialogConversation;

        [SerializeField] private GameEvent_SO choiceEvent;
        [SerializeField] private QuestEvent questEvent;
        [SerializeField] private Quest_SO quest;

        public void OnQuestAccepted()
        {
            questEvent.Raise(quest);
        }

        public void OnChoiceEvent()
        {
            choiceEvent.Raise();
        }

        public QuestEvent QuestEvent => questEvent;
        public GameEvent_SO ChoiceEvent => choiceEvent;

        public string Text => text;

        public DialogConversation_SO DialogConversation => dialogConversation;
    }
}
