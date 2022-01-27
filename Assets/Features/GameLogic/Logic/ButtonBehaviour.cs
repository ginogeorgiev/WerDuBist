using DataStructures.Variables;
using Features.NPCs.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.GameLogic.Logic
{
    public class ButtonBehaviour : MonoBehaviour
    {
        [SerializeField] private BoolVariable isPlayerInConversation;
        [SerializeField] private NpcFocus_So npcFocus;
        
        [SerializeField] private Button mapButton;
        [SerializeField] private Button helpButton;
        [SerializeField] private Button pauseButton;

        public void ToggleButtonIntractability()
        {
            if (isPlayerInConversation.Get() || npcFocus.Get() != null)
            {
                mapButton.interactable = false;
                helpButton.interactable = false;
                pauseButton.interactable = false;
            }
            else
            {
                mapButton.interactable = true;
                helpButton.interactable = true;
                pauseButton.interactable = true;
            }
        }
    }
}
