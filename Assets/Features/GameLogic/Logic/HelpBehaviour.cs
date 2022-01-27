using DataStructures.Variables;
using Features.NPCs.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.GameLogic.Logic
{
    public class HelpBehaviour : MonoBehaviour
    {
        [SerializeField] private BoolVariable isPlayerInConversation;
        [SerializeField] private NpcFocus_So npcFocus;
        
        [SerializeField] private Image helpImage;
        
        public void ToggleHelp()
        {
            if (isPlayerInConversation.Get() || npcFocus.Get() != null) return;
            
            helpImage.gameObject.SetActive(!helpImage.gameObject.activeSelf);
        }
    }
}
