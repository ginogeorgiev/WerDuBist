using System.Collections.Generic;
using System.Linq;
using DataStructures.Event;
using UnityEngine;

namespace Features.Evaluation.Logic
{
    [CreateAssetMenu(fileName = "newEvaluationDictionary", menuName = "Feature/Evaluation/EvaluationDictionary")]
    public class EvaluationData : ScriptableObject
    {
        
        [SerializeField] private string key = "UserData";
        [SerializeField] private string value = " is Empty";
        public Dictionary<string, string> EvaluationDictionary { get; private set; }
        public Dictionary<string, string> UserDataDictionary { get; private set; }

        [SerializeField] private GameEvent_SO evaluationDictionaryChanged;
        private void OnEnable()
        {
            EvaluationDictionary = new Dictionary<string, string>();
            UserDataDictionary = new Dictionary<string, string>();
        }

        public void Add(string key, string value, bool send = true)
        {
             if (value == "" + -1) return;
             
            if (EvaluationDictionary.ContainsKey(key))
            {
                EvaluationDictionary[key] = value;
                if (send) GenerateUserDataDictionary();
            }
            else
            {
                EvaluationDictionary.Add(key, value);
                if (send) GenerateUserDataDictionary();
            }
        }

        public void GenerateUserDataDictionary()
        {
            if (EvaluationDictionary.Count == 0) return;

            value = "";
            
            foreach (KeyValuePair<string, string> item in EvaluationDictionary)
            {
                value += item.Key + "," + item.Value + ",";
            }
            
            value = value.Remove(value.Length - 1);
            
            Debug.Log(key + " : " + value);
            UserDataDictionary[key] = value;
            
            if (evaluationDictionaryChanged != null) evaluationDictionaryChanged.Raise();
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
