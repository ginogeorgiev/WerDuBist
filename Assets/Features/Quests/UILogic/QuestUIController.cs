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
            GameObject questUI= Instantiate(questPrefab, new Vector3 (0,0,0), Quaternion.identity);
            questUI.transform.SetParent(contentUI.transform);

            // write Quest Info
            questUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = quest.QuestTitle;
            questUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = quest.QuestDescription;

            foreach (var goal in quest.GoalList)
            {
                // Instantiate new Goal UI Prefab
                GameObject goalUI = Instantiate(goalPrefab, new Vector3 (0,0,0), Quaternion.identity);
                goalUI.transform.SetParent(questUI.transform);

                // write Goal Info
                TMP_Text goalText = goalUI.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
                goalText.text = goal.Type==Goal.GoalType.collect ? goal.CurrentAmount.Get().ToString() : "0";
                goalText.text += "/";
                goalText.text += goal.RequiredAmount.ToString();
                
                Image goalImg = goalUI.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
                goalImg.sprite = goal.GoalSprite;
            }
            
            // connect correct Quest_SO to UI Element
            questUI.GetComponent<QuestFocusController>().Quest = quest;
            
            // if only active quest, set it as focus
            if (focus.Get()==null)
            {
                focus.Set(quest);
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
            var questUI = GetQuestUI(focus.Get().QuestID);

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
                else// if type talk 
                {
                     goalUI.text = goal.Completed ? "1/1" : "0/1";
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
            
            questUI.transform.GetChild(1).GetChild(0).gameObject.SetActive(boo);
            
            for (var i = 0; i < quest.GoalList.Count(); i++)
            {
                questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(boo);
                questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(boo);
            }
        }

        private void RemoveQuest(Quest_SO quest)
        {
            if (!quest.Visible)
            {
                activeQuests.Items.Remove(quest);
                return;
            }

            // destroy UI Prefab of completed Quest
            Destroy(GetQuestUI(quest.QuestID).gameObject);
            
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
            
            foreach (var rec in uiLayoutElements)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rec);
            }
        }
        
    }
}
