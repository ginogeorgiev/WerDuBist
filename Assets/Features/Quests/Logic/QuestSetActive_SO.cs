using System.Collections.Generic;
using UnityEngine;

namespace Features.Quests.Logic
{
   [CreateAssetMenu(fileName = "NewQuestSetActive", menuName = "Feature/Quests/QuestSetActive")]
   public class QuestSetActive_SO : ScriptableObject
   {
       // class holds all Quests which are currently active
       [SerializeField] private List<Quest_SO> activeQuests;
       public List<Quest_SO> items => activeQuests;
   }                                     
}