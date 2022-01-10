using Features.NPCs.Logic;
using UnityEngine;

namespace Features.Dialog.Logic
{
    [CreateAssetMenu(fileName = "NewDialogConversation", menuName = "Feature/Dialog/DialogConversation")]
    public class DialogConversation_SO : ScriptableObject
    {
        [Header("Hier kommen die NPCs rein, die sprechen. ", order = 0)]
        [Space (-10, order = 1)]
        [Header("(Den Kreis rechts dafür benutzen)", order = 2)]
        [SerializeField] private NPCData_SO speakerLeft;
        [SerializeField] private NPCData_SO speakerRight;
        [Header("Hier die Texte rein, die nacheinander Folgen sollen in einer Conversation", order = 3)]
        [Space (-10, order = 4)]
        
        [Header("(Mit dem Plus können mehrere hinzugefügt werden", order = 5)]
        [SerializeField] private Line[] lines;
        
        [Header("Nur wenn nach der Conversation einen Frage mit ", order = 7)]
        [Space (-10, order = 8)]
        [Header("Antwortmögllichkeiten folgen soll muss hier was rein", order = 9)]
        [SerializeField] private DialogQuestion_SO dialogQuestion;
        
        public NPCData_SO SpeakerLeft => speakerLeft;

        public NPCData_SO SpeakerRight => speakerRight;

        public Line[] Lines => lines;

        public DialogQuestion_SO DialogQuestion => dialogQuestion;
    }
    
    [System.Serializable]
    public struct Line
    {
        [SerializeField] private NPCData_SO npcData;

        [TextArea(2, 5)] [SerializeField] private string text;

        public NPCData_SO NpcData => npcData;

        public string Text => text;
    }
}
