using DataStructures.Event;
using Features.Evaluation.Logic;
using Features.Quests.Logic;
using UnityEngine;

namespace Features.Dialog.Logic
{
    [CreateAssetMenu(fileName = "NewDialogQuestion", menuName = "Feature/Dialog/DialogQuestion")]
    public class DialogQuestion_SO : ScriptableObject
    {
        [Header("Hier kommt eine Liste von Antwortmöglichkeiten rein")]
        [SerializeField] private  Choice[] choices;

        public Choice[] Choices => choices;
    }
    
    [System.Serializable]
    public struct Choice
    {
        [TextArea(2, 5)]
        [SerializeField] private string text;

        [Header("Dieses Feld nur ausfüllen, wenn nach der Auswahl einer Option eine ", order = 0)]
        [Space(-10, order = 1)]
        [Header("Survey-Frage beantwortet wurde. Wert ist der, der dazugerechnet wird.", order = 2)]
        [SerializeField] private Question_SO ingameQuestion;
        [SerializeField] [Range(0,3)] private float value;
        [Space (10, order = 3)]

        [Header("Dieses Feld nur ausfüllen, wenn nach der Auswahl einer ", order = 4)]
        [Space (-10, order = 5)]
        [Header("Option eine weitere Conversation folgen soll.", order = 6)]
        [SerializeField] private DialogConversation_SO dialogConversation;

        [Space (10, order = 7)]
        [Header("Dieses Feld ausfüllen, wenn nach der Auswahl einer Option ", order = 8)]
        [Space (-10, order = 9)]
        [Header("ein besonderes Event starten soll (zb. Teleport Player).", order = 10)]
        [SerializeField] private GameEvent_SO choiceEvent;

        [Space (10, order = 11)]
        [Header("Wenn diese Dialog Option dafür sorgen würde, ", order = 12)]
        [Space (-10, order = 13)]
        [Header("dass eine Quest angenommern werden soll, muss diese hier eingefügt werden.", order = 14)]
        [SerializeField] private Quest_SO quest;

        private QuestEvent questEvent;
        private EvaluationData evaluationData;

        public void OnQuestAccepted()
        {
            questEvent.Raise(quest);
        }

        public void OnChoiceEvent()
        {
            choiceEvent.Raise();
        }

        public void OnAddToEvaluation()
        {
            ingameQuestion.IngameRuntimeValue += value;
            evaluationData.Add(ingameQuestion.IngameAspectValue.name + "_" + ingameQuestion.Question, Mathf.Min(ingameQuestion.IngameRuntimeValue, 3).ToString());
        }

        public Question_SO IngameQuestion => ingameQuestion;

        public EvaluationData EvaluationData
        {
            get => evaluationData;
            set => evaluationData = value;
        }

        public QuestEvent QuestEvent
        {
            get => questEvent;
            set => questEvent = value;
        }

        public Quest_SO Quest => quest;
        public GameEvent_SO ChoiceEvent => choiceEvent;

        public string Text => text;

        public DialogConversation_SO DialogConversation => dialogConversation;
    }
}
