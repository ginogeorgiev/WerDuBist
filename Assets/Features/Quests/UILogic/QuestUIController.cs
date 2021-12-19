using System.Collections.Generic;
using System.Linq;
using Features.Quests.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Quests.UILogic
{
    public class QuestUIController : MonoBehaviour
    {
        [SerializeField] private QuestFocus_SO focus;
        [SerializeField] private List<RectTransform> uiLayoutElements;
        [SerializeField] private GameObject contentUI;
        [SerializeField] private GameObject questPrefab;
        [SerializeField] private GameObject goalPrefab;
        [SerializeField] private QuestSetActive_SO activeQuests;
        [SerializeField] private QuestEvent onDisplayQuest;

        private void Start()
        {
            onDisplayQuest.RegisterListener(DisplayQuest);
        }

        private void DisplayQuest(Quest_SO quest)
        {
            // Instantiate Quest UI Prefab
            GameObject questUI= Instantiate(questPrefab, new Vector3 (0,0,0), Quaternion.identity);
            questUI.transform.SetParent(contentUI.transform);

            // write Quest Info
            questUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = quest.QuestTitle;
            questUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = quest.QuestDescription;

            foreach (Goal goal in quest.GoalList)
            {
                // Instantiate new Goal UI Prefab
                GameObject goalUI = Instantiate(goalPrefab, new Vector3 (0,0,0), Quaternion.identity);
                goalUI.transform.SetParent(questUI.transform);

                // write Goal Info
                TMP_Text goalText = goalUI.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
                goalText.text = goal.CurrentAmount.Get().ToString();
                goalText.text += "/";
                goalText.text += goal.RequiredAmount.ToString();
                
                Image goalImg = goalUI.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
                goalImg.sprite = goal.GoalSprite;
            }
            
            // connect correct Quest_SO to UI Element
            questUI.GetComponent<QuestFocusController>().Quest = quest;
            
            // if only active quest, set it as focus
            if (activeQuests.Items.Count() == 1)
            {
                focus.Set(quest);
                Debug.Log("Focus on: " + quest.QuestID);
            }
            // else collapse UI
            else
            {
                SetQuestUI(quest, false);
            }

            RebuildLayout();
        }
        
        public void UpdateQuests()
        {
            if (focus.Get() == null) return;
            
            // find QuestUI with correct questID
            Transform questUI = GetQuestUI(focus.Get().QuestID);

            // update all goal texts
            for (var i = 0; i < focus.Get().GoalList.Count(); i++)
            {
                TMP_Text goalUI = questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
                goalUI.text = focus.Get().GoalList[i].CurrentAmount.Get().ToString();
                goalUI.text  += "/";
                goalUI.text  += focus.Get().GoalList[i].RequiredAmount.ToString();
            }
            
            
            RebuildLayout();
        }
        
        public void UpdateQuestsFocus()
        {
            if (focus.Get() == null) return;
            
            foreach (Quest_SO quest in activeQuests.Items)
            {
                // hide all Quest info
                SetQuestUI(quest, false);
            }  
            
            // display only info of the Focus Quest
            SetQuestUI(focus.Get(), true);
            
            UpdateQuests();
        }

        private void SetQuestUI(Quest_SO quest, bool boo)
        {
             //sets all Quest Info active/inactive i.e. collapses unfocused Quests
             
             Transform questUI = GetQuestUI(quest.QuestID);
            
            questUI.transform.GetChild(1).GetChild(0).gameObject.SetActive(boo);
            
            for (var i = 0; i < quest.GoalList.Count(); i++)
            {
                questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(boo);
                questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(boo);
            }
        }
        
        public void RemoveQuest()
        {
            // destroy UI Prefab of completed Quest
            Destroy(GetQuestUI(focus.Get().QuestID).gameObject);
            
            // remove from active quests
            activeQuests.Items.Remove(focus.Get());

            // if any more active quests, focus on the first one
            if (activeQuests.Items.Any())
            { 
                focus.Set(activeQuests.Items[0]);
                Debug.Log("Focus on: " + activeQuests.Items[0].QuestID);
                UpdateQuestsFocus();
            }

            RebuildLayout();
        }

        private Transform GetQuestUI(int id)
        {
            // gets the UI Elements of the given Quest
            
            for (var i = 0; i < contentUI.transform.childCount; i++)
            {
                Transform questUI = contentUI.transform.GetChild(i);
                if (questUI.GetComponent<QuestFocusController>().Quest.QuestID == id)
                {
                    return questUI;
                }
            }

            return null;
        }

        private void RebuildLayout()
        {
            // updates the Layout Elements; else they update themselves to late causing visual glitches
            
            foreach (RectTransform rec in uiLayoutElements)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rec);
            }
        }
    }
}
