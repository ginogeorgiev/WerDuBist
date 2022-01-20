using System.Collections.Generic;
using UnityEngine;
using Features.Quests.Logic;
using DataStructures.Focus;
using DataStructures.Variables;
using Features.Input;
using UnityEngine.UI;

namespace Features.Map.Logic
{
    public class BigMapController : MonoBehaviour
    {
        [SerializeField] private GameObject mapUI;
        [SerializeField] private BoolVariable isGamePaused;

        [SerializeField] private GameObject mapBG;
        [SerializeField] private Sprite mainIsland;
        [SerializeField] private GameObject mapCamera;
        [SerializeField] private Transform mapOverlay;
        [SerializeField] private RectTransform mapBorder;

        [SerializeField] private Focus_SO<Quest_SO> questFocus;

        [SerializeField] private GameObject questMarker_Tutorial;
        [SerializeField] private GameObject questMarker_Main;
        private GameObject questMarker;
        
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
            questMarker = questMarker_Tutorial;
            onDisplayUnlockedQuest.RegisterListener(DisplayUnlockedQuest);
            onDisplayActiveQuest.RegisterListener(DisplayActiveQuest);
            onRemoveQuest.RegisterListener(RemoveQuest);
            
            playerControls = new PlayerControls();
        }

        public void Start()
        {
            playerControls.Player.Map.started += _ => ToggleMapUI();
            mapCamera.transform.localPosition = new Vector3(-521.6f, -207.95f, -1.5f);
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
            if (isGamePaused.Get()) return;
            
            mapUI.SetActive(!mapUI.activeSelf);
        }
        
        public void SwitchIslands()
        {
            Debug.Log("switch");
            mapBG.GetComponent<Image>().sprite = mainIsland;
            
            mapCamera.GetComponent<Camera>().orthographicSize = 55f;
            mapCamera.transform.localPosition = new Vector3(-420, -170, -1.5f);

            mapOverlay.localPosition = new Vector3(-2.2f, 0.2f, 0);
            mapOverlay.localScale = new Vector3(0.6534686f, 0.6534686f, 0.6534686f);

            mapBorder.sizeDelta = new Vector2(885, 618);

            questMarker = questMarker_Main;
        }

        private void DisplayUnlockedQuest(Quest_SO quest)
        {
            var marker = questMarker;
            if (quest.QuestID == 2) marker = questMarker_Main;

            var obj= Instantiate(marker, quest.StartPosition, Quaternion.identity);
            obj.transform.SetParent(mapUI.transform);
            obj.transform.position = new Vector3(quest.StartPosition.x, quest.StartPosition.y, -.4f);
            obj.GetComponent<SpriteRenderer>().sprite = questNew;

            newQuestMarkers.Add(quest.QuestID, obj); 
        }
        
        private void DisplayActiveQuest(Quest_SO quest)
        {
            if (!quest.Visible)
            {
                if (newQuestMarkers.ContainsKey(quest.QuestID))
                {
                    Destroy(newQuestMarkers[quest.QuestID]);
                }
                else return;
            }
            else
            {
                var obj = newQuestMarkers.ContainsKey(quest.QuestID) ? 
                    newQuestMarkers[quest.QuestID] : Instantiate(questMarker, quest.StartPosition, Quaternion.identity);
                
                obj.GetComponent<SpriteRenderer>().sprite = questActive;
                obj.transform.position = new Vector3(quest.EndPosition.x, quest.EndPosition.y, -.5f);
                obj.transform.SetParent(mapUI.transform);
                activeQuestMarkers.Add(quest.QuestID, obj);
            }

            newQuestMarkers.Remove(quest.QuestID);
        }
        
        public void DisplayActiveFocus()
        {
            if (questFocus.Get() == null) return;
            if (!activeQuestMarkers.ContainsKey(questFocus.Get().QuestID)) return;

            if (focusMarker!=null)
            {
                focusMarker.GetComponent<SpriteRenderer>().sprite = questActive;
            }
            
            var obj = activeQuestMarkers[questFocus.Get().QuestID];
            obj.GetComponent<SpriteRenderer>().sprite = questFocusActive;
            obj.transform.SetParent(mapUI.transform);
            focusMarker = obj;
        }
        
        private void RemoveQuest(Quest_SO quest)
        {
            if (activeQuestMarkers.ContainsKey(quest.QuestID))
            {
                Destroy(activeQuestMarkers[quest.QuestID]);
                activeQuestMarkers.Remove(quest.QuestID);
            } 
            else if (newQuestMarkers.ContainsKey(quest.QuestID))
            {
                Destroy(newQuestMarkers[quest.QuestID]);
                newQuestMarkers.Remove(quest.QuestID);
            }
        }
    }
}
