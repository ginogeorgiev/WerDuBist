using UnityEditor;
using UnityEngine;

namespace Features.Evaluation.Logic.Editor
{
    [CustomEditor(typeof(EvaluationData))]
    public class EvaluationDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            EvaluationData e = target as EvaluationData;
            
            if (GUILayout.Button("GiveEvaDicContent"))
                e.GiveEvaDicContent();
                
            if (GUILayout.Button("GiveEvaDicLength"))
                e.GiveEvaDicLength();
                
            if (GUILayout.Button("LogUsaData"))
                e.LogUserData();
        }
    }
}