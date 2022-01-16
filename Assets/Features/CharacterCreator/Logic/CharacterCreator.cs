using System;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CharacterCreator.Logic
{
    public class CharacterCreator : MonoBehaviour
    {
        [Header("Objects to color")]
        [SerializeField] private SpriteRenderer[] spriteRenderers;
        [SerializeField] private Image[] images;

        [Header("Accessory")] 
        [SerializeField] private SpriteRenderer[] accessoryRenderers;
        [SerializeField] private Image[] accessoryImages;

        [Header("Color")] 
        [SerializeField] private ColorVariable playerSkinColor;

        [Header("Player Accessory")] 
        [SerializeField] private PlayerAccessory_SO playerAccessory;

        public void OnCharacterCreatorSubmit()
        {
            if(spriteRenderers == null || images == null || accessoryRenderers == null) return;

            var skinColor = playerSkinColor.Get();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = skinColor;
            }
            foreach (var image in images)
            {
                image.color = skinColor;
            }

            foreach (var accessoryRenderer in accessoryRenderers)
            {
                accessoryRenderer.sprite = playerAccessory.PlayerAccessory;
                accessoryRenderer.color = new Color(255, 255, 255, playerAccessory.Alpha);
            }
            
            foreach (var accessoryImage in accessoryImages)
            {
                accessoryImage.sprite = playerAccessory.PlayerAccessory;
                accessoryImage.color = new Color(255, 255, 255, playerAccessory.Alpha);
            }
        }

    }
}
