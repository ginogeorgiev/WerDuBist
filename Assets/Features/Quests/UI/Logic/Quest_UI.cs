using System.Collections.Generic;
using System.Linq;
using DataStructures.Focus;
using Features.Quests.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Quests.UI.Logic
{
    public class Quest_UI : MonoBehaviour
    {
        public Focus_SO<Quest_SO> QuestFocus;
        public GameObject uiContent;
        public GameObject QuestPrefab;
        public GameObject GoalPrefab;
        public QuestSetActive_SO activeQuests;
        private List<TextSlots> textSlots = new List<TextSlots>();

        public void DisplayQuest(Quest_SO quest)
        {
            // check if that Quest is already displayed
            if (textSlots.Any(slot => slot.QuestID == quest.QuestID))
            {
                return;
            }

            // Instantiate Quest UI Prefab
            var qObj= Instantiate(QuestPrefab, new Vector3 (0,0,0), Quaternion.identity);
            qObj.transform.SetParent(uiContent.transform);
            
            // save the Text Slots from within the Prefab, to access them easier
            var q = new TextSlots
            (
                qObj,
                quest.QuestID,
                qObj.transform.GetChild (0).gameObject.transform.GetChild(0).GetComponent<TMP_Text>(),
                qObj.transform.GetChild (1).gameObject.transform.GetChild(0).GetComponent<TMP_Text>()
            );
            textSlots.Add(q);
            
            // write Quest Info
            q.QuestName.text = quest.QuestName;
            q.Descriptions.text = quest.Description;

            foreach (var goal in quest.Goals)
            {
                // Instantiate new Goal UI Prefab
                var gObj = Instantiate(GoalPrefab, new Vector3 (0,0,0), Quaternion.identity);
                gObj.transform.SetParent(qObj.transform);
                
                // save the Text/Image Slots from within the Prefab
                var g = new TextSlots.G
                (
                    gObj.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<TMP_Text>(), 
                    gObj.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>()
                );
                q.Goals.Add(g);
                
                // write Goal Info
                g.Text.text = goal.CurrentAmount.Get().ToString();
                g.Text.text += "/";
                g.Text.text += goal.RequiredAmount.ToString();
                 
                g.Image.sprite = goal.GoalSprite;
            }
        }

        public void UpdateQuests()
        {
            foreach (var quest in activeQuests.Items)
            {
                // get Goal Slots of correct questID
                var g = textSlots.Where(slot => slot.QuestID == quest.QuestID).ToList()[0].Goals;

                // update all goal texts
                for (int i = 0; i < quest.Goals.Count(); i++)
                {
                    g[i].Text.text = quest.Goals[i].CurrentAmount.Get().ToString();
                    g[i].Text.text  += "/";
                    g[i].Text.text  += quest.Goals[i].RequiredAmount.ToString();
                }
            }
        }
        
        public void RemoveQuest(Quest_SO quest)
        {
            // get Goal Slots of correct questID
            // var g = textSlots.(slot => slot.QuestID == quest.QuestID);
            var obj = textSlots.First(slot => slot.QuestID == quest.QuestID).QuestObj;
            Destroy(obj);
        }

        
        public void setFocus(Quest_SO quest)
        {
            QuestFocus.focus = quest;
        }
    }
}

public class TextSlots
{
    public GameObject QuestObj;
    public string QuestID;
    public TMP_Text QuestName;
    public TMP_Text Descriptions;
    public List<G> Goals;

    public class G
    {
        public TMP_Text Text;
        public Image Image;

        public G(TMP_Text text,  Image image)
        {
            Text = text;
            Image = image;
        }
    }

    public TextSlots(GameObject obj, string id, TMP_Text name, TMP_Text desc )
    {
        QuestObj = obj;
        QuestID = id;
        QuestName = name;
        Descriptions = desc;
        Goals = new List<G>();
    }
}
