using DataStructures.Variables;
using UnityEngine;

namespace Features.Survey.Logic
{
    [CreateAssetMenu(fileName = "NewQuestion", menuName = "Feature/Survey/Question")]
    public class Question_SO : ScriptableObject
    {
        [SerializeField] private string question;
        [SerializeField] private IntVariable aspect;
        [Tooltip("false means - (negative) and true means + (positive)")]
        [SerializeField] private bool key;

        public string Question => question;

        public IntVariable Aspect => aspect;

        public bool Key => key;
    }
}
