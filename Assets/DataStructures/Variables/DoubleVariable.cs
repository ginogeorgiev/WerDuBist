using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewDoubleVariable", menuName = "DataStructures/Variables/DoubleVariable")]
    public class DoubleVariable : AbstractVariable<double>
    {
        public void Add(double value)
        {
            runtimeValue += value;
            if(onValueChanged != null) onValueChanged.Raise();
        }

        public void Add(DoubleVariable value)
        {
            runtimeValue += value.runtimeValue;
            if(onValueChanged != null) onValueChanged.Raise();
        }
    }
}
