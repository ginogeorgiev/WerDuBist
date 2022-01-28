// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// Edited by: Gino Georgiev
// ----------------------------------------------------------------------------

using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewFloatVariable", menuName = "DataStructures/Variables/Float Variable")]
    public class FloatVariable : AbstractVariable<float>
    {
        public void Add(float value)
        {
            runtimeValue += value;
            if(onValueChanged != null) onValueChanged.Raise();
        }

        public void Add(FloatVariable value)
        {
            runtimeValue += value.runtimeValue;
            if(onValueChanged != null) onValueChanged.Raise();
        }
    }
}
