using UnityEngine;

namespace Features.Dialog.Logic
{
    [System.Serializable]
    public struct Choice
    {
        [TextArea(2, 5)]
    public string text;

    public Conversation conversation;
    }

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Feature/Dialog/Question")]
    public class Question : ScriptableObject
    {
        [TextArea(2, 5)]
        public string text;

        public Choice[] choices;
    }
}
