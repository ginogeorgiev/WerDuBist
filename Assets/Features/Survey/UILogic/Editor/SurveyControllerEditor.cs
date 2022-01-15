using UnityEditor;
using UnityEngine;

namespace Features.Survey.UILogic.Editor
    {
        [CustomEditor(typeof(SurveyController))]
        public class SurveyControllerEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                GUI.enabled = Application.isPlaying;

                SurveyController e = target as SurveyController;
            
                if (GUILayout.Button("CalculateResult"))
                    e.OnCalculateResult();
            
                if (GUILayout.Button("ResetSurvey"))
                    e.ResetSurvey();
            }
        }
    }
