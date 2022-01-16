using UnityEngine;

namespace Features.CharacterCreator.Logic
{
    [CreateAssetMenu(fileName = "newPlayerAccessory", menuName = "Feature/CharacterCreator/PlayerAccessory")]
    public class PlayerAccessory_SO : ScriptableObject
    {
        [SerializeField] private Sprite playerAccessory;
        [SerializeField] private int alpha;

        public Sprite PlayerAccessory
        {
            get => playerAccessory;
            set => playerAccessory = value;
        }

        public int Alpha
        {
            get => alpha;
            set => alpha = value;
        }
    }
}
