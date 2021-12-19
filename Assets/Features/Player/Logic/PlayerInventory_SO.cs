using DataStructures.Variables;
using UnityEngine;

namespace Features.Player.Logic
{
    [CreateAssetMenu(fileName = "newPlayerInventory", menuName = "Feature/Player/PlayerInventory")]
    public class PlayerInventory_SO : ScriptableObject
    {
        [SerializeField] private IntVariable wood;
        [SerializeField] private IntVariable stone;
        [SerializeField] private IntVariable starfish;

        public IntVariable Wood => wood;

        public IntVariable Stone => stone;

        public IntVariable Starfish => starfish;
    }
}
