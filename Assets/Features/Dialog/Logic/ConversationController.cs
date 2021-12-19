using DataStructures.Event;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Dialog.Logic
{
    [System.Serializable]
    public class QuestionEvent : UnityEvent<DialogQuestion_SO> {}

    public class ConversationController : MonoBehaviour
    {
        [SerializeField] private GameEvent_SO onCheckForNextConversationPart;
        [SerializeField] private NpcFocus_So npcFocus;
        [SerializeField] private DialogConversation_SO dialogConversation;
        [SerializeField] private BoolVariable isPlayerInConversation;
        [SerializeField] private QuestionEvent questionEvent;

        [SerializeField] private GameObject speakerLeft;
        [SerializeField] private GameObject speakerRight;

        private SpeakerUIController speakerUIControllerLeft;
        private SpeakerUIController speakerUIControllerRight;

        private int activeLineIndex;
        private bool conversationStarted;

        public void OnNpcFocusChanged()
        {
            if (npcFocus.Get() != null)
            {
                dialogConversation = npcFocus.Get().ActiveConversation;
            }
        }
        private void Start()
        {
            speakerUIControllerLeft = speakerLeft.GetComponent<SpeakerUIController>();
            speakerUIControllerRight = speakerRight.GetComponent<SpeakerUIController>();
        }
        
        private void AdvanceConversation()
        {
            if (dialogConversation.DialogQuestion != null)
            {
                isPlayerInConversation.SetFalse();
                questionEvent.Invoke(dialogConversation.DialogQuestion);
                EndConversation();
            }
            else if (dialogConversation.NextDialogConversationStep != null)
            {
                ChangeConversation(dialogConversation.NextDialogConversationStep);
            }
            else
            {
                EndConversation();
                isPlayerInConversation.SetFalse();
            }
        }

        public void ChangeConversation(DialogConversation_SO nextDialogConversation)
        {
            conversationStarted = false;
            
            if (nextDialogConversation == null) return;
            dialogConversation = nextDialogConversation;
            AdvanceLine();
        }
        
        private void EndConversation()
        {
            dialogConversation = null;
            conversationStarted = false;
            speakerUIControllerLeft.Hide();
            speakerUIControllerRight.Hide();
            onCheckForNextConversationPart.Raise();
        }

        private void Initialize()
        {
            onCheckForNextConversationPart.Raise();
            conversationStarted = true;
            activeLineIndex = 0;
            speakerUIControllerLeft.Speaker = dialogConversation.SpeakerLeft;
            speakerUIControllerRight.Speaker = dialogConversation.SpeakerRight;
        }

        public void AdvanceLine()
        {
            if (dialogConversation == null) return;
            isPlayerInConversation.SetTrue();
            
            if (!conversationStarted) Initialize();

            if (activeLineIndex < dialogConversation.Lines.Length)
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
            Line line = dialogConversation.Lines[activeLineIndex];
            NPCData_SO npcDataSo = line.NpcData;

            if (speakerUIControllerLeft.SpeakerIs(npcDataSo))
            {
                SetDialog(speakerUIControllerLeft, speakerUIControllerRight, line.Text);
            }
            else
            {
                SetDialog(speakerUIControllerRight, speakerUIControllerLeft, line.Text);
            }

            activeLineIndex += 1;
        }

        private void SetDialog(
            SpeakerUIController activeSpeakerUIController,
            SpeakerUIController inactiveSpeakerUIController,
            string text)
        {
            activeSpeakerUIController.Dialog = text;
            activeSpeakerUIController.Show();
            inactiveSpeakerUIController.Hide();
        }
    }
}
