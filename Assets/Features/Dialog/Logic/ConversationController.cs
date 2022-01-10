using DataStructures.Variables;
using Features.Input;
using Features.NPCs.Logic;
using Features.Tutorial.Logic;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Dialog.Logic
{
    [System.Serializable]
    public class QuestionEvent : UnityEvent<DialogQuestion_SO> {}

    public class ConversationController : MonoBehaviour
    {
        [SerializeField] private NpcFocus_So npcFocus;
        [SerializeField] private TutorialData_SO tutorialData;
        [SerializeField] private DialogConversation_SO dialogConversation;
        [SerializeField] private BoolVariable isPlayerInConversation;
        [SerializeField] private QuestionEvent questionEvent;

        [SerializeField] private GameObject questionUI;
        
        [SerializeField] private GameObject speakerLeft;
        [SerializeField] private GameObject speakerRight;

        private SpeakerUIController speakerUIControllerLeft;
        private SpeakerUIController speakerUIControllerRight;

        private int activeLineIndex;
        private bool conversationStarted;

        public void OnNpcFocusChanged()
        {
            dialogConversation = npcFocus.Get() != null ? npcFocus.Get().ActiveConversation : null;
        }
        private void Start()
        {
            speakerUIControllerLeft = speakerLeft.GetComponent<SpeakerUIController>();
            speakerUIControllerRight = speakerRight.GetComponent<SpeakerUIController>();
            
            playerControls.Player.Interact.started += _ => AdvanceLine();
        }
        
        #region Input related
        
        private PlayerControls playerControls;
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
        
        #endregion
        
        private void AdvanceConversation()
        {
            if (dialogConversation.DialogQuestion != null)
            {
                questionEvent.Invoke(dialogConversation.DialogQuestion);
                EndConversation();
            }
            else
            {
                isPlayerInConversation.SetFalse();
                
                EndConversation();
                
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
            
            
            npcFocus.Get().OnCheckForNextConversationPart();
        }

        private void Initialize()
        {
            conversationStarted = true;
            activeLineIndex = 0;
            speakerUIControllerLeft.Speaker = dialogConversation.SpeakerLeft;
            speakerUIControllerRight.Speaker = dialogConversation.SpeakerRight;

            if (npcFocus.Get().GetActiveConversationElement.Quest != null)
            {
                npcFocus.Get().OnCheckForNextConversationPart();
            }
        }

        private void AdvanceLine()
        {
            if (questionUI.activeSelf) return;
            
            if (dialogConversation == null) return;
            
            isPlayerInConversation.SetTrue();
            tutorialData.OnDeActivateInteractInfo.Raise();
            
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
