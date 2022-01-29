using System.Collections.Generic;
using DataStructures.Event;
using UnityEngine;

namespace Features.Evaluation.Logic
{
    [CreateAssetMenu(fileName = "newEvaluationDictionary", menuName = "Feature/Evaluation/EvaluationDictionary")]
    public class EvaluationData : ScriptableObject
    {
        public Dictionary<string, string> EvaluationDictionary { get; private set; }

        [SerializeField] private GameEvent_SO evaluationDictionaryChanged;
        private void OnEnable()
        {
            EvaluationDictionary = new Dictionary<string, string>();
        }

        public void Add(string key, string value)
        {
             if (value == "" + -1) return;
             
            if (EvaluationDictionary.ContainsKey(key))
            {
                EvaluationDictionary[key] = value;
                if (evaluationDictionaryChanged != null) evaluationDictionaryChanged.Raise();
            }
            else
            {
                EvaluationDictionary.Add(key, value);
                if (evaluationDictionaryChanged != null) evaluationDictionaryChanged.Raise();
            }
        }
        
        public void GiveEvaDicLength()
        {
            Debug.Log(EvaluationDictionary.Count);
        }

        public void GiveEvaDicContent()
        {
            foreach (KeyValuePair<string, string> pair in EvaluationDictionary)
            {
                Debug.Log(pair.Key + " : " + pair.Value);
            }
        }
    }
}
