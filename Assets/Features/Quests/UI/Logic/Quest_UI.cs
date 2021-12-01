using System.Linq;
using Features.Quests.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Quests.UI.Logic
{
    public class Quest_UI : MonoBehaviour
    {
        public QuestFocus_SO focus;
        public GameObject ContentUI;
        public GameObject QuestPrefab;
        public GameObject GoalPrefab;
        public QuestSetActive_SO activeQuests;

        public void DisplayQuest(Quest_SO quest)
        {
            // check if not already displayed
            if (findQuestUI(quest.QuestID) != null)
            {
                return;
            }
            
            // Instantiate Quest UI Prefab
            var questUI= Instantiate(QuestPrefab, new Vector3 (0,0,0), Quaternion.identity);
            questUI.transform.SetParent(ContentUI.transform);

            // write Quest Info
            questUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = quest.QuestName;
            questUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = quest.Description;

            foreach (var goal in quest.Goals)
            {
                // Instantiate new Goal UI Prefab
                var goalUI = Instantiate(GoalPrefab, new Vector3 (0,0,0), Quaternion.identity);
                goalUI.transform.SetParent(questUI.transform);

                // write Goal Info
                var goalText = goalUI.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
                goalText.text = goal.CurrentAmount.Get().ToString();
                goalText.text += "/";
                goalText.text += goal.RequiredAmount.ToString();
                
                var goalImg = goalUI.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
                goalImg.sprite = goal.GoalSprite;
            }
            
            questUI.GetComponent<QuestFocusController>().quest = quest;
        }

        public void UpdateQuests()
        {
            foreach (var quest in activeQuests.Items)
            {
                // find QuestUI with correct questID
                var questUI = findQuestUI(quest.QuestID);

                // update all goal texts
                for (int i = 0; i < quest.Goals.Count(); i++)
                {
                    var goalUI = questUI.GetChild(2 + i).GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
                    goalUI.text = quest.Goals[i].CurrentAmount.Get().ToString();
                    goalUI.text  += "/";
                    goalUI.text  += quest.Goals[i].RequiredAmount.ToString();
                }
            }
        }

        public void RemoveQuest()
        {
            // destroy UI Prefab of completed Quest
            Destroy(findQuestUI(focus.focus.QuestID).gameObject);
        }

        private Transform findQuestUI(string id)
        {
            for (int i = 0; i < ContentUI.transform.childCount; i++)
            {
                var questUI = ContentUI.transform.GetChild(i);
                if (questUI.GetComponent<QuestFocusController>().quest.QuestID == id)
                {
                    return questUI;
                }
            }

            return null;
        }
    }
}
