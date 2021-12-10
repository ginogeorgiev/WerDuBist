using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Dialog.Logic
{
    public class SpeakerUIController : MonoBehaviour
    {
        [SerializeField] private Image portrait;
        [SerializeField] private TMP_Text fullName;
        [SerializeField] private TMP_Text dialog;

        private NPCData_SO speaker;
        public NPCData_SO Speaker
        {
            get => speaker;
            set
            {
                speaker = value;
                portrait.sprite = speaker.Portrait;
                fullName.text = speaker.FullName;
            }
        }

        public string Dialog
        {
            set => dialog.text = value;
        }

        public bool HasSpeaker()
        {
            return speaker != null;
        }

        public bool SpeakerIs(NPCData_SO npcData)
        {
            return speaker == npcData;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

