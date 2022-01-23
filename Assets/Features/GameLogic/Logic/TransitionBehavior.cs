using DataStructures.Event;
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
                .setOnComplete(transitionData.OnStartCompleted.Raise);
        }

        public void OnEndTransition()
        {
            LeanTween.alpha(image.rectTransform, StartAlpha, transitionData.FadeOutTime)
                .setEase(transitionData.FadeOutEaseType)
                .setOnComplete(transitionData.OnEndCompleted.Raise)
                .setOnComplete(Deactivate);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
