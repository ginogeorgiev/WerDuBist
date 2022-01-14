using System;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CharacterCreator.Logic
{
    public class CharacterCreator : MonoBehaviour
    {
        [Header("Objects to change")]
        [SerializeField] private SpriteRenderer[] spriteRenderers;
        [SerializeField] private Image[] images;

        [Header("Color")] [SerializeField] private ColorVariable playerSkinColor;

        public void OnCharacterCreatorSubmit()
        {
            if(spriteRenderers == null || images == null) return;

            var skinColor = playerSkinColor.Get();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = skinColor;
            }
            foreach (var image in images)
            {
                image.color = skinColor;
            }
        }

    }
}
