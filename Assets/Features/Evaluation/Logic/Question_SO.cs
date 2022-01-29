using DataStructures.Variables;
using UnityEngine;

namespace Features.Evaluation.Logic
{
    [CreateAssetMenu(fileName = "NewQuestion", menuName = "Feature/Survey/Question")]
    public class Question_SO : ScriptableObject
    {
        [SerializeField] private string question;
        [SerializeField] private IntVariable gameAspectValue;
        [SerializeField] private IntVariable surveyAspectValue;
        [Tooltip("false means - (negative) and true means + (positive)")]
        [SerializeField] private bool key;

        [SerializeField] private EvaluationData evaluationData;
        
        public float IngameRuntimeValue { get; set; }

        private void OnEnable()
        {
            IngameRuntimeValue = 0;
        }

        public string Question => question;

        public IntVariable GameAspectValue => gameAspectValue;
        public IntVariable SurveyAspectValue => surveyAspectValue;

        public bool Key => key;

        public void AddToInGameRuntimeValue(float value)
        {
            IngameRuntimeValue += value;
            IngameRuntimeValue = Mathf.Clamp(IngameRuntimeValue, 0, 3);
            evaluationData.Add(GameAspectValue.name + "_" + Question, (Mathf.Round(IngameRuntimeValue)).ToString());
            // Debug.Log(GameAspectValue.name + " " + value + " ergibt " + IngameRuntimeValue);
        }
    }
}
