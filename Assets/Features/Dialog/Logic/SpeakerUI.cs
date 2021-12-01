using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Dialog.Logic
{
    public class SpeakerUI : MonoBehaviour
    {
        [SerializeField] private Image portrait;
        [SerializeField] private TMP_Text fullName;
        [SerializeField] private TMP_Text dialog;

        private Character speaker;
        public Character Speaker
        {
            get { return speaker; }
            set
            {
                speaker = value;
                portrait.sprite = speaker.portrait;
                fullName.text = speaker.fullName;
            }
        }

        public string Dialog
        {
            set { dialog.text = value; }
        }

        public bool HasSpeaker()
        {
            return speaker != null;
        }

        public bool SpeakerIs(Character character)
        {
            return speaker == character;
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

