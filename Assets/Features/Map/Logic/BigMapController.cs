using System.Collections.Generic;
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

        [SerializeField] private GameObject newQuest;
        [SerializeField] private GameObject quest_Active;
        [SerializeField] private GameObject quest_Ready;
        [SerializeField] private GameObject questFocus_Active;
        [SerializeField] private GameObject questFocus_Ready;

        [SerializeField] private GameEvent_SO onHideMiniMap;
        [SerializeField] private GameEvent_SO onShowMiniMap;
        
        [SerializeField] private QuestEvent onDisplayUnlockedQuest;
        [SerializeField] private QuestEvent onDisplayActiveQuest;
        [SerializeField] private QuestEvent onRemoveQuest;


        private Dictionary<int, GameObject> newQuestMarkers = new Dictionary<int, GameObject>();
        private Dictionary<int, GameObject> activeQuestMarkers = new Dictionary<int, GameObject>();

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

        private void Start()
        {
            onDisplayUnlockedQuest.RegisterListener(DisplayUnlockedQuest);
            onDisplayActiveQuest.RegisterListener(DisplayActiveQuest);
           
            
            // if (questFocus.Get()!=null)
            // {
            //     questFocus_Active.SetActive(true);
            //     questFocus_Active.transform.position =  WorldToMap(questFocus.Get().QuestPosition);
            // }
            // else
            // {
            //     questFocus_Active.SetActive(false);
            // }
        }

        private void DisplayUnlockedQuest(Quest_SO quest)
        {
            
            var obj= Instantiate(newQuest, WorldToMap(quest.QuestPosition), Quaternion.identity);
            obj.transform.SetParent(mapUI.transform);

            newQuestMarkers.Add(quest.QuestID, obj);
        }
        
        private void DisplayActiveQuest(Quest_SO quest)
        {
            Destroy(newQuestMarkers[quest.QuestID]);
            newQuestMarkers.Remove(quest.QuestID);
            
            // var obj= Instantiate(newQuest, WorldToMap(quest.QuestPosition), Quaternion.identity);
            // obj.transform.SetParent(mapUI.transform);
            //
            activeQuestMarkers.Add(quest.QuestID, null);
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
