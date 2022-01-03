using DataStructures.Event;
using UnityEngine;

namespace Features.Tutorial.Logic
{
    [CreateAssetMenu(fileName = "newTutorialData", menuName = "Feature/Tutorial/TutorialData")]
    public class TutorialData_SO : ScriptableObject
    {
        
        [SerializeField] private GameEvent_SO onActivateAllInfos;
        [SerializeField] private GameEvent_SO onDeActivateAllInfos;
        
        [SerializeField] private GameEvent_SO onDelayedActivateAllInfos;
        [SerializeField] private GameEvent_SO onDelayedDeActivateAllInfos;
        
        [SerializeField] private float infoTimer;

        [SerializeField] private GameEvent_SO onActivateInteractInfo;
        [SerializeField] private GameEvent_SO onDeActivateInteractInfo;
        
        public GameEvent_SO OnActivateAllInfos => onActivateAllInfos;
        public GameEvent_SO OnDeActivateAllInfos => onDeActivateAllInfos;

        public GameEvent_SO OnDelayedActivateAllInfos => onDelayedActivateAllInfos;

        public GameEvent_SO OnDelayedDeActivateAllInfos => onDelayedDeActivateAllInfos;

        public float InfoTimer => infoTimer;

        public GameEvent_SO OnActivateInteractInfo => onActivateInteractInfo;

        public GameEvent_SO OnDeActivateInteractInfo => onDeActivateInteractInfo;
    }
}