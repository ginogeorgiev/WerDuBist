using UnityEngine;
using UnityEngine.UI;

namespace Features.GameLogic.Logic
{
    public class TransitionBehavior : MonoBehaviour
    {
        private const float StartAlpha = 0;
        private const float TransitionAlpha = 1;
        
        [SerializeField] private Image image;

        [SerializeField] private TransitionData transitionData;

        public void OnStartTransition()
        {
            gameObject.SetActive(true);
            LeanTween.alpha(image.rectTransform, TransitionAlpha, transitionData.FadeInTime)
                .setEase(transitionData.FadeInEaseType)
                .setOnComplete(OnTransitionStartCompleted);
        }

        private void OnTransitionStartCompleted()
        {
            transitionData.OnStartCompleted.Raise();
        }

        public void OnEndTransition()
        {
            LeanTween.alpha(image.rectTransform, StartAlpha, transitionData.FadeOutTime)
                .setEase(transitionData.FadeOutEaseType)
                .setOnComplete(OnTransitionEndCompleted)
                .setOnComplete(Deactivate);
        }

        private void OnTransitionEndCompleted()
        {
            transitionData.OnEndCompleted.Raise();
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
