using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewFloatVariable", menuName = "DataStructures/Variables/Float Variable")]
    public class FloatVariable : AbstractVariable<float>
    {
        public void Add(float value)
        {
            runtimeValue += value;
            onValueChanged.Raise();
        }

        public void Add(FloatVariable value)
        {
            runtimeValue += value.runtimeValue;
            onValueChanged.Raise();
        }
    }
}
