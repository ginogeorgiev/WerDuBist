using System.Collections;
using DataStructures.Event;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Tutorial.Logic
{
    public class TutorialUIBehaviour : MonoBehaviour
    {
        [SerializeField] private TutorialData_SO tutorialData;
        
        public void OnDelayedActivateAllInfos()
        {
            StartCoroutine(DelayedActivateAllInfos());
        }

        public void OnDelayedDeActivateAllInfos()
        {
            StopAllCoroutines();
            tutorialData.OnDeActivateAllInfos.Raise();
        }

        private IEnumerator DelayedActivateAllInfos()
        {
            yield return new WaitForSeconds(tutorialData.InfoTimer);

            tutorialData.OnActivateAllInfos.Raise();
        }
    }
}
