using Features.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Dialog.Logic
{
    [System.Serializable]
    public class QuestionEvent : UnityEvent<DialogQuestion_SO> {}

    public class ConversationController : MonoBehaviour
    {
        [SerializeField] private ConversationFocus_SO dialogConversationFocus;
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
            //dirty but for MVP ok
            dialogConversationFocus.Set(dialogConversationFocus.Get().NextDialogConversation);
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
            if (dialogConversationFocus.Get().DialogQuestion != null)
            {
                questionEvent.Invoke(dialogConversationFocus.Get().DialogQuestion);
            }
            else if (dialogConversationFocus.Get().NextDialogConversation != null)
            {
                ChangeConversation(dialogConversationFocus.Get().NextDialogConversation);
            }
            else
            {
                EndConversation();
            }
        }
        private void EndConversation()
        {
            dialogConversationFocus.Set(null);
            conversationStarted = false;
            speakerUIControllerLeft.Hide();
            speakerUIControllerRight.Hide();
        }

        private void Initialize()
        {
            conversationStarted = true;
            activeLineIndex = 0;
            speakerUIControllerLeft.Speaker = dialogConversationFocus.Get().SpeakerLeft;
            speakerUIControllerRight.Speaker = dialogConversationFocus.Get().SpeakerRight;
        }

        private void AdvanceLine()
        {
            if (dialogConversationFocus.Get() == null) return;
            //TODO set PlayerState
            if (!conversationStarted) Initialize();

            if (activeLineIndex < dialogConversationFocus.Get().Lines.Length)
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
            Line line = dialogConversationFocus.Get().Lines[activeLineIndex];
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
