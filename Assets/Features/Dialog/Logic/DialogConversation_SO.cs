using UnityEngine;

namespace Features.Dialog.Logic
{
    [CreateAssetMenu(fileName = "NewDialogConversation", menuName = "Feature/Dialog/DialogConversation")]
    public class DialogConversation_SO : ScriptableObject
    {
        [SerializeField] private NPCData_SO speakerLeft;
        [SerializeField] private NPCData_SO speakerRight;
        [SerializeField] private DialogQuestion_SO dialogQuestion;
        [SerializeField] private DialogConversation_SO nextDialogConversation;
        [SerializeField] private Line[] lines;

        public NPCData_SO SpeakerLeft => speakerLeft;

        public NPCData_SO SpeakerRight => speakerRight;

        public DialogQuestion_SO DialogQuestion => dialogQuestion;

        public DialogConversation_SO NextDialogConversation => nextDialogConversation;

        public Line[] Lines => lines;
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
