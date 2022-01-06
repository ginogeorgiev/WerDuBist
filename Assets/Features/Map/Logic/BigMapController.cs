using System;
using System.Collections.Generic;
using DataStructures.Event;
using UnityEngine;
using Features.Quests.Logic;
using DataStructures.Focus;
using Features.Input;
using UnityEngine.UI;

namespace Features.Map.Logic
{
    public class BigMapController : MonoBehaviour
    {
        [SerializeField] private GameObject mapUI;

        [SerializeField] private Focus_SO<Quest_SO> questFocus;

        [SerializeField] private GameObject questMarker;
        [SerializeField] private Sprite questActive;
        [SerializeField] private Sprite questFocusActive;
        [SerializeField] private Sprite questNew;
        
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
            
            playerControls = new PlayerControls();
        }

        public void Start()
        {
            playerControls.Player.Map.started += _ => ToggleMapUI();
        }
        
        #region Input related
        
        private PlayerControls playerControls;

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
        
        #endregion

        public void ToggleMapUI()
        {
            mapUI.SetActive(!mapUI.activeSelf);
        }

        private void DisplayUnlockedQuest(Quest_SO quest)
        {
            var obj= Instantiate(questMarker, quest.QuestPosition, Quaternion.identity);
            obj.transform.SetParent(mapUI.transform);
            obj.GetComponent<SpriteRenderer>().sprite = questNew;

            newQuestMarkers.Add(quest.QuestID, obj);
        }
        
        private void DisplayActiveQuest(Quest_SO quest)
        {
            var obj = newQuestMarkers[quest.QuestID];
            obj.GetComponent<SpriteRenderer>().sprite = questActive;
            
            newQuestMarkers.Remove(quest.QuestID);
            activeQuestMarkers.Add(quest.QuestID, obj);
        }
        
        public void DisplayActiveFocus()
        {
            if (questFocus.Get() == null) return; 
            
            if (focusMarker!=null)
            {
                focusMarker.GetComponent<SpriteRenderer>().sprite = questActive;
            }
            
            var obj = activeQuestMarkers[questFocus.Get().QuestID];
            obj.GetComponent<SpriteRenderer>().sprite = questFocusActive;
            focusMarker = obj;
        }
        
        private void RemoveQuest(Quest_SO quest)
        {
            Destroy(newQuestMarkers[quest.QuestID]);
            activeQuestMarkers.Remove(quest.QuestID);
        }
    }
}