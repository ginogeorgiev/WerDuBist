using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewVector3Variable", menuName = "Utils/Variables/Vector3Variable")]
    public class Vector3Variable : AbstractVariable<Vector3>
    {
        public void Add(Vector3 value)
        {
            runtimeValue += value;
            onValueChanged.Raise();
        }

        public void Add(Vector3Variable value)
        {
            runtimeValue += value.runtimeValue;
            onValueChanged.Raise();
        }
    }
}

