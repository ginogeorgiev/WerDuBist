using UnityEngine;

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
        [SerializeField] private  string text;

        [SerializeField] private  DialogConversation_SO dialogConversation;

        public string Text => text;

        public DialogConversation_SO DialogConversation => dialogConversation;
    }
}
