using Features.Input;
using UnityEngine;

namespace Features.Dialog.Logic
{
    public class DialogDisplay : MonoBehaviour
    {
        public Conversation conversation;
        
        private PlayerControls playerControls;

        public GameObject speakerLeft;
        public GameObject speakerRight;

        private SpeakerUI speakerUILeft;
        private SpeakerUI speakerUIRight;

        private int activeLineIndex = 0;
        
        private void Start()
        {
            speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
            speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

            speakerUILeft.Speaker = conversation.speakerLeft;
            speakerUIRight.Speaker = conversation.speakerRight;
            
            playerControls.Player.SkipDialog.started += _ => AdvanceConversation();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void AdvanceConversation()
        {
            Debug.Log("hi");
            if (activeLineIndex < conversation.lines.Length)
            {
                DisplayLine();
                activeLineIndex += 1;
            }
            else
            {
                speakerUILeft.Hide();
                speakerUIRight.Hide();
                activeLineIndex = 0;
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
