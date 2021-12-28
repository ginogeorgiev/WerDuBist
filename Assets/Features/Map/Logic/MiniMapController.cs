using UnityEngine;
using Features.Quests.Logic;
using DataStructures.Focus;

namespace Features.Map.Logic
{
    public class MiniMapController : MonoBehaviour
    {
        [SerializeField] private GameObject mapUI;

        [SerializeField] private Focus_SO<Quest_SO> questFocus;
        [SerializeField] private GameObject questIcon;
        
        
        public void HideMapUI()
        {
            mapUI.SetActive(false);
        }
        public void ShowMapUI()
        {
            mapUI.SetActive(true);
        }

        // TODO add QuestMarker
        
        // void Update()
        // {
        //     if (questFocus.Get()!=null)
        //     {
        //         questIcon.SetActive(true);
        //         questIcon.transform.position = WorldToMap(questFocus.Get().QuestPosition);
        //     }
        //     else
        //     {
        //         questIcon.SetActive(false);
        //     }
        // }
    }
}
