using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewDoubleVariable", menuName = "Utils/Variables/DoubleVariable")]
    public class DoubleVariable : AbstractVariable<double>
    {
        public void Add(double value)
        {
            runtimeValue += value;
            onValueChanged.Raise();
        }

        public void Add(DoubleVariable value)
        {
            runtimeValue += value.runtimeValue;
            onValueChanged.Raise();
        }
    }
}
