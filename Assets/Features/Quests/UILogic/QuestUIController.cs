using System;
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
        [SerializeField] private QuestSet_SO activeQuests;
        [SerializeField] private QuestEvent onDisplayQuest;
        [SerializeField] private QuestEvent onRemoveQuest;

        private void Start()
        {
            onDisplayQuest.RegisterListener(DisplayQuest);
            onRemoveQuest.RegisterListener(RemoveQuest);
        }

        private void DisplayQuest(Quest_SO quest)
        {
            if (!quest.Visible)
            {
                return;
            }
            // Instantiate Quest UI Prefab
            var questUI= Instantiate(questPrefab, new Vector3 (0,0,0), Quaternion.identity);
            questUI.transform.SetParent(contentUI.transform);

            // write Quest Info
            questUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = quest.QuestTitle;
            questUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = quest.QuestDescription;

            foreach (var goal in quest.GoalList)
            {
                // Instantiate new Goal UI Prefab
                var goalUI = Instantiate(goalPrefab, new Vector3 (0,0,0), Quaternion.identity);
                goalUI.transform.SetParent(questUI.transform);

                // write Goal Info
                var goalText = goalUI.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
                
                switch (goal.Type)
                {
                    case Goal.GoalType.collect:
                        goalText.text = goal.CurrentAmount.Get().ToString();
                        break;
                    case Goal.GoalType.talk:
                        goalText.text = "0";
                        break;
                    case Goal.GoalType.quest:
                        goalText.text = goal.OtherQuests.Count(qu => qu.IsCompleted).ToString();
                        break;
                }
                goalText.text += "/";
                goalText.text += goal.RequiredAmount.ToString();
                
                var goalImg = goalUI.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
                goalImg.sprite = goal.GoalSprite;
            }
            
            // connect correct Quest_SO to UI Element
            questUI.GetComponent<QuestFocusController>().Quest = quest;
            
            // if other in Focus already, collapse current focus 
            if (focus.Get() != null)
            {
                 SetQuestUI(focus.Get(), false);
            }
            // set newly accepted Quest as new Focus
            focus.Set(quest);


            RebuildLayout();
        }
        
        public void UpdateQuests()
        {
            if (focus.Get() == null) return;
            
            // find QuestUI with correct questID
            var questUI = GetQuestUI(focus.Get().QuestID);
            
            if (questUI==null) return;
            

            // update all goal texts
            for (var i = 0; i < focus.Get().GoalList.Count(); i++)
            {
                var goalUI = questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
                var goal = focus.Get().GoalList[i];

                if (goal.Type==Goal.GoalType.collect)
                {
                    goalUI.text = goal.CurrentAmount.Get().ToString();
                    goalUI.text  += "/";
                    goalUI.text  += goal.RequiredAmount.ToString() ;
                }
                else if (goal.Type==Goal.GoalType.collect)
                {
                     goalUI.text = goal.Completed ? "1/1" : "0/1";
                }
                else if (goal.Type==Goal.GoalType.quest)
                {
                    var q = goal.OtherQuests.FindAll(quest => quest.IsCompleted).Count();
                    goalUI.text = q.ToString();
                    goalUI.text  += "/";
                    goalUI.text  += goal.RequiredAmount.ToString() ;
                }
            }
            
            RebuildLayout();
        }
        
        public void UpdateQuestsFocus()
        {
            if (focus.Get() == null) return;
            
            foreach (var quest in activeQuests.Items.Where(quest => quest.Visible))
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
             
            var questUI = GetQuestUI(quest.QuestID);
            if (questUI==null) return;
            
            questUI.transform.GetChild(1).GetChild(0).gameObject.SetActive(boo);
            
            for (var i = 0; i < quest.GoalList.Count(); i++)
            {
                questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(boo);
                questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(boo);
            }
        }

        private void RemoveQuest(Quest_SO quest)
        {
            if (!activeQuests.Items.Contains(quest)) return;

            if (!quest.Visible)
            {
                activeQuests.Items.Remove(quest);
                return;
            }

            var questUI = GetQuestUI(quest.QuestID);
            if (questUI==null) return;
            
            // destroy UI Prefab of completed Quest
            Destroy(questUI.gameObject);
            
            // remove from active quests
            activeQuests.Items.Remove(quest);

            // if completed quest was in Focus
            if (focus.Get() == quest)
            {
                // if any more active quests, focus on the first visible one
                if (activeQuests.Items.Any(q => q.Visible))
                {
                    foreach (var questsItem in activeQuests.Items.Where(questsItem => questsItem.Visible))
                    {
                        focus.Set(questsItem);
                        UpdateQuestsFocus();
                        break;
                    }
                }else
                {
                    focus.Set(null);
                }
            }
            
            RebuildLayout();
        }

        private Transform GetQuestUI(int id)
        {
            // gets the UI Elements of the given Quest
            
            for (var i = 0; i < contentUI.transform.childCount; i++)
            {
                var questUI = contentUI.transform.GetChild(i);
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
            
            foreach (var rec in uiLayoutElements)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rec);
            }
        }
        
    }
}
