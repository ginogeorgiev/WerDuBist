using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Quests.Logic
{
    public class Quest_UI : MonoBehaviour
    {
        public Quest_SO quest;

        public TMP_Text nameText;
        public TMP_Text descriptionText;
        public TMP_Text goal1;
        public TMP_Text goal2;
        
        public Sprite img;
        // public TMP_Text goal2;

        // Start is called before the first frame update
        void Start()
        {
            goal1.text = "";
            // goal2.text = "";
            nameText.text = quest.QuestName;
            descriptionText.text = quest.Description;
            for (int i = 0; i < quest.Goals.Count(); i++)
            {
                goal1.text += quest.Goals.ElementAt(i).CurrentAmount.Get().ToString();
                goal1.text += "/";
                goal1.text += quest.Goals.ElementAt(i).RequiredAmount.ToString();
                goal1.text += "\n";
                
                var newObj = new GameObject(); //Create the GameObject
                                               var newImage = newObj.AddComponent<Image>(); //Add the Image Component script
                                               newImage.sprite = img; //Set the Sprite of the Image Component on the new GameObject
                                               newObj.GetComponent<RectTransform>().SetParent(goal1.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
                                               newObj.SetActive(true);
                                               newObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-55,0,0);
                                               newObj.GetComponent<RectTransform>().sizeDelta = new Vector2(40,40);
            }

            

            // x -55; y0
            // w/h 40

        }

        public void UpdateQuests()
        {
            goal1.text = "";
            for (int i = 0; i < quest.Goals.Count(); i++)
            {
                goal1.text += quest.Goals.ElementAt(i).CurrentAmount.Get().ToString();
                goal1.text += "/";
                goal1.text += quest.Goals.ElementAt(i).RequiredAmount.ToString();
                goal1.text += "\n";
            }
        }
    }
}
