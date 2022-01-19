using DataStructures.Variables;
using UnityEngine;

namespace Features.Player.Logic
{
    [CreateAssetMenu(fileName = "newPlayerInventory", menuName = "Feature/Player/PlayerInventory")]
    public class PlayerInventory_SO : ScriptableObject
    {
        [SerializeField] private IntVariable wood;
        [SerializeField] private IntVariable stone;
        [SerializeField] private IntVariable appleRed;
        [SerializeField] private IntVariable tube;
        [SerializeField] private IntVariable metalPlate1;
        [SerializeField] private IntVariable metalPlate2;

        public IntVariable Wood => wood;

        public IntVariable Stone => stone;

        public IntVariable AppleRed => appleRed;

        public IntVariable Tube => tube;

        public IntVariable MetalPlate1 => metalPlate1;

        public IntVariable MetalPlate2 => metalPlate2;
    }
}
