using System.Collections.Generic;
using DataStructures.Event;
using UnityEngine;
using Features.Quests.Logic;
using DataStructures.Focus;
using UnityEngine.UI;

namespace Features.Map.Logic
{
    public class BigMapController : MonoBehaviour
    {
        [SerializeField] private GameObject mapUI;

        [SerializeField] private Focus_SO<Quest_SO> questFocus;

        [SerializeField] private GameObject questMarker;
        [SerializeField] private Sprite questNew;
        [SerializeField] private Sprite questActive;
        [SerializeField] private Sprite questFocusActive;

        [SerializeField] private GameEvent_SO onHideMiniMap;
        [SerializeField] private GameEvent_SO onShowMiniMap;
        
        [SerializeField] private QuestEvent onDisplayUnlockedQuest;
        [SerializeField] private QuestEvent onDisplayActiveQuest;
        [SerializeField] private QuestEvent onRemoveQuest;


        private readonly Dictionary<int, GameObject> newQuestMarkers = new Dictionary<int, GameObject>();
        private readonly Dictionary<int, GameObject> activeQuestMarkers = new Dictionary<int, GameObject>();
        private GameObject focusMarker;
        

        private void Awake()
        {
            onDisplayUnlockedQuest.RegisterListener(DisplayUnlockedQuest);
            onDisplayActiveQuest.RegisterListener(DisplayActiveQuest);
            onRemoveQuest.RegisterListener(RemoveQuest);
        }

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

        private void DisplayUnlockedQuest(Quest_SO quest)
        {
            var obj= Instantiate(questMarker, WorldToMap(quest.QuestPosition), Quaternion.identity);
            obj.transform.SetParent(mapUI.transform);
            obj.GetComponent<Image>().sprite = questNew;

            newQuestMarkers.Add(quest.QuestID, obj);
        }
        
        private void DisplayActiveQuest(Quest_SO quest)
        {
            var obj = newQuestMarkers[quest.QuestID];
            obj.GetComponent<Image>().sprite = questActive;
            
            newQuestMarkers.Remove(quest.QuestID);
            activeQuestMarkers.Add(quest.QuestID, obj);
        }
        
        public void DisplayActiveFocus()
        {
            if (questFocus.Get() == null) return; 
            
            if (focusMarker!=null)
            {
                focusMarker.GetComponent<Image>().sprite = questActive;
            }
            
            var obj = activeQuestMarkers[questFocus.Get().QuestID];
            obj.GetComponent<Image>().sprite = questFocusActive;
            focusMarker = obj;
        }
        
        private void RemoveQuest(Quest_SO quest)
        {
            Destroy(newQuestMarkers[quest.QuestID]);
            activeQuestMarkers.Remove(quest.QuestID);
        }

        private static Vector3 WorldToMap(Vector3 worldCoordinates)
        {
            return new Vector3(
                440 + (worldCoordinates.x / 30 * 530), 
                245 + worldCoordinates.y / 21 * 369, 
                worldCoordinates.z);
        }

    }
}
