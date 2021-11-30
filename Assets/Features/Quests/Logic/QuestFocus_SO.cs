using System;
using DataStructures.Focus;
using UnityEngine;

namespace Features.Quests.Logic
{
    [CreateAssetMenu(fileName = "QuestFocus", menuName = "Feature/Quests/QuestFocus")]
    public class QuestFocus_SO : Focus_SO<Quest_SO>
    {
        public void reset()
        {
            focus = null;
        }
    }
}
