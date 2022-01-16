using System;
using DataStructures.Variables;
using Features.CharacterCreator.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CharacterCreator.UILogic
{
    public class CharacterControllerUIController : MonoBehaviour
    {
        [SerializeField] private ColorVariable playerSkinColor;
        [SerializeField] private PlayerAccessory_SO playerAccessory_SO;
        [SerializeField] private Image player, playerAccessory, accessoryPicker;
        [SerializeField] private Sprite[] accessories;

        private int accessoryIndex;

        private void Awake()
        {
            accessoryIndex = 0;
            if(accessories.Length>0)
            {
                accessoryPicker.sprite = accessories[accessoryIndex];
                accessoryPicker.color = accessories[accessoryIndex] == null ? new Color(255, 255, 255, 0) : new Color(255, 255, 255, 255);
            }
        }

        public void IncreaseAccessory()
        {
            accessoryIndex = accessoryIndex == accessories.Length - 1 ? 0 : accessoryIndex + 1;
            SetAccessory();
        }
        
        public void DecreaseAccessory()
        {
            accessoryIndex = accessoryIndex <= 0 ? accessories.Length-1 : accessoryIndex - 1;
            SetAccessory();
        }

        private void SetAccessory()
        {
            var accessoryAtIndex = accessories[accessoryIndex];
            accessoryPicker.sprite = accessoryAtIndex;
            accessoryPicker.color = accessoryAtIndex == null ? new Color(255, 255, 255, 0) : new Color(255, 255, 255, 255);
            playerAccessory.sprite = accessoryAtIndex;
            playerAccessory.color = accessoryAtIndex == null ? new Color(255, 255, 255, 0) : new Color(255, 255, 255, 255);
            
            playerAccessory_SO.PlayerAccessory = accessoryAtIndex;
            playerAccessory_SO.Alpha = accessoryAtIndex == null ? 0 : 255;
        }
        
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
