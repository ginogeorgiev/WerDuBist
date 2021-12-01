using UnityEngine;

namespace Features.Dialog.Logic
{
    [System.Serializable]
    public struct Line 
    {
    public Character character;
    
    [TextArea(2, 5)]
    public string text;
    }

[CreateAssetMenu(fileName = "NewConversation", menuName = "Feature/Dialog/Conversation")]
    public class Conversation : ScriptableObject
    {
        public Character speakerLeft;
        public Character speakerRight;
        public Question question;
        public Conversation nextConversation;
        public Line[] lines;
    }
}

