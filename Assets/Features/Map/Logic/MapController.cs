using DataStructures.Focus;
using UnityEngine;
using Features.Input;
using Features.Quests.Logic;

namespace Features.Map.Logic
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private GameObject mapUI;
        
        [SerializeField] private Transform player;
        [SerializeField] private RectTransform playerIcon;
        
        [SerializeField] private Focus_SO<Quest_SO> questFocus;
        [SerializeField] private GameObject questIcon;
        
        
        public void ToggleMapUI()
        {
            mapUI.SetActive(!mapUI.activeSelf);
        }

        void Update()
        {
            if (mapUI.activeSelf)
            {
                playerIcon.position = WorldToMap(player.position);
            }

            if (questFocus.Get()!=null)
            {
                questIcon.SetActive(true);
                questIcon.transform.position = WorldToMap(questFocus.Get().QuestPosition);
            }
            else
            {
                questIcon.SetActive(false);
            }
        }

        private Vector3 WorldToMap(Vector3 worldCoordinates)
        {
            return new Vector3(
                432 + (worldCoordinates.x / 30 * 530), 
                243 + worldCoordinates.y / 21 * 369, 
                worldCoordinates.z);
        }

    }
}
