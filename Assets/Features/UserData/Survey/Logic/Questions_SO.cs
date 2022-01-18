using System.Collections.Generic;
using Features.Evaluation.Logic;
using UnityEngine;

namespace Features.UserData.Survey.Logic
{
    [CreateAssetMenu(fileName = "NewQuestionSet", menuName = "Feature/Survey/QuestionSet")]
    public class Questions_SO : ScriptableObject
    {
        [SerializeField] private List<Question_SO> items;

        public IEnumerable<Question_SO> Items => items;
    }
}
