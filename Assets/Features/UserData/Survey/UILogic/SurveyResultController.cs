using DataStructures.Variables;
using TMPro;
using UnityEngine;

namespace Features.UserData.Survey.UILogic
{
    public class SurveyResultController : MonoBehaviour
    {
        [Header("The 5 Aspects")]
        [Tooltip("SO")] [SerializeField] private IntVariable extraversion;
        [Tooltip("SO")] [SerializeField] private IntVariable agreeableness;
        [Tooltip("SO")] [SerializeField] private IntVariable conscientiousness;
        [Tooltip("SO")] [SerializeField] private IntVariable neuroticism;
        [Tooltip("SO")] [SerializeField] private IntVariable openness;

        [Header("The 5 Results for each Aspects")]
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text extraversionText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text agreeablenessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text conscientiousnessText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text neuroticismText;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text opennessText;

        [Header("The 5 Results for each Aspects in percent")]
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text extraversionPercentage;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text agreeablenessPercentage;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text conscientiousnessPercentage;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text neuroticismPercentage;
        [Tooltip("GameObjectRef")] [SerializeField] private TMP_Text opennessPercentage;

        private void OnShowResult()
        {
            
        }

        private void UpdateResultUI()
        {
            // Update Values
            extraversionText.text = extraversion.Get().ToString();
            agreeablenessText.text = agreeableness.Get().ToString();
            conscientiousnessText.text = conscientiousness.Get().ToString();
            neuroticismText.text = neuroticism.Get().ToString();
            opennessText.text = openness.Get().ToString();

            // Update Percentages
            extraversionPercentage.text = extraversion.Get() * 2.5f + "%";
            agreeablenessPercentage.text = agreeableness.Get() * 2.5f + "%";
            conscientiousnessPercentage.text = conscientiousness.Get() * 2.5f + "%";
            neuroticismPercentage.text = neuroticism.Get() * 2.5f + "%";
            opennessPercentage.text = openness.Get() * 2.5f + "%";
        }
    }
}