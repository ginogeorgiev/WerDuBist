using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewIntVariable", menuName = "DataStructures/Variables/IntVariable")]
    public class IntVariable : AbstractVariable<int>
    {
        public void Add(int value)
        {
            runtimeValue += value;
            if(onValueChanged != null) onValueChanged.Raise();
        }

        public void Add(IntVariable value)
        {
            runtimeValue += value.runtimeValue;
            if(onValueChanged != null) onValueChanged.Raise();
        }
    }
}
