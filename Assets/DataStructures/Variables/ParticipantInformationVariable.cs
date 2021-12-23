using UnityEngine;

namespace DataStructures.Variables
{
    public struct ParticipantInfo
    {
        public string gender;
        [Range(18,99)] public int age;
        [Range(0,20)] public int gamingExperience;
    }
    
    [CreateAssetMenu(fileName = "NewParticipantInformationVariable", menuName = "DataStructures/Variables/Participant Information Variable")]
    public class ParticipantInformationVariable : AbstractVariable<ParticipantInfo>
    {
        public void Set(string gender, int age, int gamingExperience)
        {
            runtimeValue = new ParticipantInfo()
            {
                gender = gender,
                age = age,
                gamingExperience = gamingExperience
            };
            onValueChanged.Raise();
        }

        public void Set(ParticipantInformationVariable value)
        {
            runtimeValue = value.runtimeValue;
            onValueChanged.Raise();
        }
    }
}