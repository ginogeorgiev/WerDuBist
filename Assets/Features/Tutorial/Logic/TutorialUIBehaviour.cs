using System.Collections;
using UnityEngine;

namespace Features.Tutorial.Logic
{
    public class TutorialUIBehaviour : MonoBehaviour
    {
        [SerializeField] private TutorialData_SO tutorialData;

        public void OnDelayedActivateAllInfos()
        {
            if (!tutorialData.IsPlayerInConversation.Get())
            {
                StartCoroutine(DelayedActivateAllInfos());
            }
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
