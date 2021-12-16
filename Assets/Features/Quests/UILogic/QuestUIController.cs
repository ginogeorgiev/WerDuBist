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
            var questUI= Instantiate(questPrefab, new Vector3 (0,0,0), Quaternion.identity);
            questUI.transform.SetParent(contentUI.transform);

            // write Quest Info
            questUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = quest.questTitle;
            questUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = quest.questDescription;

            foreach (var goal in quest.goalList)
            {
                // Instantiate new Goal UI Prefab
                var goalUI = Instantiate(goalPrefab, new Vector3 (0,0,0), Quaternion.identity);
                goalUI.transform.SetParent(questUI.transform);

                // write Goal Info
                var goalText = goalUI.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
                goalText.text = goal.currentAmount.Get().ToString();
                goalText.text += "/";
                goalText.text += goal.requiredAmount.ToString();
                
                var goalImg = goalUI.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
                goalImg.sprite = goal.goalSprite;
            }
            
            // connect correct Quest_SO to UI Element
            questUI.GetComponent<QuestFocusController>().quest = quest;
            
            // if only active quest, set it as focus
            if (activeQuests.items.Count() == 1)
            {
                focus.focus = quest;
                Debug.Log("Focus on: " + quest.questID);
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
            // find QuestUI with correct questID
            var questUI = GetQuestUI(focus.focus.questID);

            // update all goal texts
            for (var i = 0; i < focus.focus.goalList.Count(); i++)
            {
                var goalUI = questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
                goalUI.text = focus.focus.goalList[i].currentAmount.Get().ToString();
                goalUI.text  += "/";
                goalUI.text  += focus.focus.goalList[i].requiredAmount.ToString();
            }
            
            
            RebuildLayout();
        }
        
        public void UpdateQuestsFocus()
        {
            foreach (var quest in activeQuests.items)
            {
                // hide all Quest info
                SetQuestUI(quest, false);
            }  
            
            // display only info of the Focus Quest
            SetQuestUI(focus.focus, true);
            
            UpdateQuests();
        }

        private void SetQuestUI(Quest_SO quest, bool boo)
        {
             //sets all Quest Info active/inactive i.e. collapses unfocused Quests
             
             var questUI = GetQuestUI(quest.questID);
            
            questUI.transform.GetChild(1).GetChild(0).gameObject.SetActive(boo);
            
            for (var i = 0; i < quest.goalList.Count(); i++)
            {
                questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(boo);
                questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(boo);
            }
        }
        
        public void RemoveQuest()
        {
            // destroy UI Prefab of completed Quest
            Destroy(GetQuestUI(focus.focus.questID).gameObject);
            
            // remove from active quests
            activeQuests.items.Remove(focus.focus);

            // if any more active quests, focus on the first one
            if (activeQuests.items.Any())
            { 
                focus.focus = activeQuests.items[0];
                Debug.Log("Focus on: " + activeQuests.items[0].questID);
                UpdateQuestsFocus();
            }

            RebuildLayout();
        }

        private Transform GetQuestUI(int id)
        {
            // gets the UI Elements of the given Quest
            
            for (var i = 0; i < contentUI.transform.childCount; i++)
            {
                var questUI = contentUI.transform.GetChild(i);
                if (questUI.GetComponent<QuestFocusController>().quest.questID == id)
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
