using UnityEngine;

namespace Features.Dialog.Logic
{
    [CreateAssetMenu(fileName = "NewDialogConversation", menuName = "Feature/Dialog/DialogConversation")]
    public class DialogConversation_SO : ScriptableObject
    {
        [SerializeField] private NPCData_SO speakerLeft;
        [SerializeField] private NPCData_SO speakerRight;
        [SerializeField] private Line[] lines;
        [SerializeField] private DialogQuestion_SO dialogQuestion;
        [SerializeField] private DialogConversation_SO nextDialogConversationStep;

        public NPCData_SO SpeakerLeft => speakerLeft;

        public NPCData_SO SpeakerRight => speakerRight;

        public Line[] Lines => lines;

        public DialogQuestion_SO DialogQuestion => dialogQuestion;

        public DialogConversation_SO NextDialogConversationStep => nextDialogConversationStep;
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
