using System;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CharacterCreator.UILogic
{
    public class CharacterControllerUIController : MonoBehaviour
    {
        [SerializeField] private ColorVariable playerSkinColor;
        [SerializeField] private Image player;

        public void ChangeRed(float red)
        {
            Color currentColor = playerSkinColor.Get();
            playerSkinColor.Set(new Color(red/255,currentColor.g,currentColor.b));
        }
        
        public void ChangeGreen(float green)
        {
            Color currentColor = playerSkinColor.Get();
            playerSkinColor.Set(new Color(currentColor.r,green/255,currentColor.b));
        }
        
        public void ChangeBlue(float blue)
        {
            Color currentColor = playerSkinColor.Get();
            playerSkinColor.Set(new Color(currentColor.r,currentColor.g,blue/255));
        }

        public void UpdatePlayerPortrait()
        {
            player.color = playerSkinColor.Get();
        }
    }
}
