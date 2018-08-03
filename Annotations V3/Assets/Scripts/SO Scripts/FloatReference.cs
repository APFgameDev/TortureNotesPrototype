namespace Brinx.SO
{
    [System.Serializable]
    public class FloatReference
    {
        /// <summary>Whether to use an editor-assigned constant or a ScriptableObject variable</summary>
        public bool UseVariable = false;
        /// <summary>Constant value. Primarily used for testing and debugging </summary>
        public float ConstantValue;
        /// <summary>Linked ScriptableObject variable</summary>
        public FloatVariable Variable;

        /// <summary>Both Get and Set affect ConstantValue or Variable.Value, depending on UseVariable</summary>
        public float Value
        {
            get
            {
                if (!UseVariable || (!UnityEngine.Application.isPlaying && Variable == null))
                    return ConstantValue;
                else
                    return Variable.Value;
            }

            set
            {
                if (!UseVariable || (!UnityEngine.Application.isPlaying && Variable == null))
                    ConstantValue = value;
                else
                    Variable.Value = value;
            }
        }

        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }

}
