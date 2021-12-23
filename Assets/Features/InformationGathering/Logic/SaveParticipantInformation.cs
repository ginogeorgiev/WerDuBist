using DataStructures.Variables;
using TMPro;
using UnityEngine;

namespace Features.InformationGathering.Logic
{
    public class SaveParticipantInformation : MonoBehaviour
    {
        [SerializeField] private ParticipantInformationVariable participantInformationVariable;
        [SerializeField] private TMP_Dropdown genderDropdown;
        [SerializeField] private TMP_InputField ageField;
        [SerializeField] private TMP_InputField gameExperienceField;

        public void SetParticipantInformation()
        {
            
        }
    }
}
