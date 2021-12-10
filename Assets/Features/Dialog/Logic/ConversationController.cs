using Features.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Dialog.Logic
{
    [System.Serializable]
    public class QuestionEvent : UnityEvent<DialogQuestion_SO> {}

    public class ConversationController : MonoBehaviour
    {
        [SerializeField] private DialogConversation_SO dialogConversation;
        [SerializeField] private QuestionEvent questionEvent;
        
        private PlayerControls playerControls;

        [SerializeField] private GameObject speakerLeft;
        [SerializeField] private GameObject speakerRight;

        private SpeakerUIController speakerUIControllerLeft;
        private SpeakerUIController speakerUIControllerRight;

        private int activeLineIndex = 0;
        private bool conversationStarted = false;

        public void ChangeConversation(DialogConversation_SO nextDialogConversation)
        {
            conversationStarted = false;
            dialogConversation = nextDialogConversation;
            AdvanceLine();
        }
        private void Start()
        {
            speakerUIControllerLeft = speakerLeft.GetComponent<SpeakerUIController>();
            speakerUIControllerRight = speakerRight.GetComponent<SpeakerUIController>();

            playerControls.Player.SkipDialog.started += _ => AdvanceLine();
        }
        
        private void AdvanceConversation()
        {
            if (dialogConversation.DialogQuestion != null)
            {
                questionEvent.Invoke(dialogConversation.DialogQuestion);
            }
            else if (dialogConversation.NextDialogConversation != null)
            {
                ChangeConversation(dialogConversation.NextDialogConversation);
            }
            else
            {
                EndConversation();
            }
        }
        private void EndConversation()
        {
            dialogConversation = null;
            conversationStarted = false;
            speakerUIControllerLeft.Hide();
            speakerUIControllerRight.Hide();
        }

        private void Initialize()
        {
            conversationStarted = true;
            activeLineIndex = 0;
            speakerUIControllerLeft.Speaker = dialogConversation.SpeakerLeft;
            speakerUIControllerRight.Speaker = dialogConversation.SpeakerRight;
        }

        private void AdvanceLine()
        {
            if (dialogConversation == null) return;
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
