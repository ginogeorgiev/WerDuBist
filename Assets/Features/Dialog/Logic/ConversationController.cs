using System;
using Features.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Dialog.Logic
{
    [System.Serializable]
    public class QuestionEvent : UnityEvent<Question> {}

    public class ConversationController : MonoBehaviour
    {
        public Conversation conversation;
        public QuestionEvent questionEvent;
        
        private PlayerControls playerControls;

        public GameObject speakerLeft;
        public GameObject speakerRight;

        private SpeakerUI speakerUILeft;
        private SpeakerUI speakerUIRight;

        private int activeLineIndex = 0;
        private bool conversationStarted = false;

        public void ChangeConversation(Conversation nextConversation)
        {
            conversationStarted = false;
            conversation = nextConversation;
            AdvanceLine();
        }
        private void Start()
        {
            speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
            speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

            playerControls.Player.SkipDialog.started += _ => AdvanceLine();
        }
        
        private void AdvanceConversation()
        {
            if (conversation.question != null)
            {
                questionEvent.Invoke(conversation.question);
            }
            else if (conversation.nextConversation != null)
            {
                ChangeConversation(conversation.nextConversation);
            }
            else
            {
                EndConversation();
            }
        }
        private void EndConversation()
        {
            conversation = null;
            conversationStarted = false;
            speakerUILeft.Hide();
            speakerUIRight.Hide();
        }

        private void Initialize()
        {
            conversationStarted = true;
            activeLineIndex = 0;
            speakerUILeft.Speaker = conversation.speakerLeft;
            speakerUIRight.Speaker = conversation.speakerRight;
        }

        private void AdvanceLine()
        {
            if (conversation == null) return;
            if (!conversationStarted) Initialize();

            if (activeLineIndex < conversation.lines.Length)
            {
                DisplayLine();
            }
            else
            {
                AdvanceConversation();
            }
        }

        private void DisplayLine()
        {
            Line line = conversation.lines[activeLineIndex];
            Character character = line.character;

            if (speakerUILeft.SpeakerIs(character))
            {
                SetDialog(speakerUILeft, speakerUIRight, line.text);
            }
            else
            {
                SetDialog(speakerUIRight, speakerUILeft, line.text);
            }

            activeLineIndex += 1;
        }

        

        private void SetDialog(
            SpeakerUI activeSpeakerUI,
            SpeakerUI inactiveSpeakerUI,
            string text)
        {
            activeSpeakerUI.Dialog = text;
            activeSpeakerUI.Show();
            inactiveSpeakerUI.Hide();
        }
        
        private void Awake()
        {
            playerControls = new PlayerControls();
        }
        
        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}
