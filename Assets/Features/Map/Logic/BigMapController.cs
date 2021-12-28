using DataStructures.Event;
using UnityEngine;
using Features.Quests.Logic;
using DataStructures.Focus;

namespace Features.Map.Logic
{
    public class BigMapController : MonoBehaviour
    {
        [SerializeField] private GameObject mapUI;

        [SerializeField] private Focus_SO<Quest_SO> questFocus;
        [SerializeField] private GameObject questIcon;

        [SerializeField] private GameEvent_SO onHideMiniMap;
        [SerializeField] private GameEvent_SO onShowMiniMap;
        
        
        public void ToggleMapUI()
        {
            if (mapUI.activeSelf)
            {
                mapUI.SetActive(false);
                onShowMiniMap.Raise();
            }
            else
            {
                mapUI.SetActive(true);
                onHideMiniMap.Raise();
            }
        }

        private void Update()
        {
           if (questFocus.Get()!=null)
           {
               questIcon.SetActive(true);
               questIcon.transform.position =  WorldToMap(questFocus.Get().QuestPosition);
           }
           else
           {
               questIcon.SetActive(false);
           }
        }

        private Vector3 WorldToMap(Vector3 worldCoordinates)
        {
            return new Vector3(
                440 + (worldCoordinates.x / 30 * 530), 
                245 + worldCoordinates.y / 21 * 369, 
                worldCoordinates.z);
        }

    }
}
