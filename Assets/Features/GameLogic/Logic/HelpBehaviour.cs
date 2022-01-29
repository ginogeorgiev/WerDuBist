using DataStructures.Variables;
using Features.Evaluation.Logic;
using Features.NPCs.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.GameLogic.Logic
{
    public class HelpBehaviour : MonoBehaviour
    {
        [SerializeField] private BoolVariable isPlayerInConversation;
        [SerializeField] private NpcFocus_So npcFocus;
        
        [SerializeField] private Question_SO question;
        
        [SerializeField] private Image helpImage;
        
        private void Start()
        {
            question.AddToInGameRuntimeValue(3f);
        }
        
        public void ToggleHelp()
        {
            if (isPlayerInConversation.Get() || npcFocus.Get() != null) return;
            
            helpImage.gameObject.SetActive(!helpImage.gameObject.activeSelf);
        }
    }
}
