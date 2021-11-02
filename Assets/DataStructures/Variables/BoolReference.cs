using System;

namespace DataStructures.Variables
{
    [Serializable]
    public class BoolReference : AbstractReference<bool>
    {
        public BoolReference(bool value) : base(value)
        {
        }
    }
}
