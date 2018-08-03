namespace Brinx.SO
{
    [System.Serializable]
    public class Vector2Reference
    {
        /// <summary>Whether to use an editor-assigned constant or a ScriptableObject variable</summary>
        public bool UseVariable = false;
        /// <summary>Constant value. Primarily used for testing and debugging </summary>
        public UnityEngine.Vector2 ConstantValue;
        /// <summary>Linked ScriptableObject variable</summary>
        public Vector2Variable Variable;

        /// <summary>Both Get and Set affect ConstantValue or Variable.Value, depending on UseVariable</summary>
        public UnityEngine.Vector2 Value
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

        public static implicit operator UnityEngine.Vector2(Vector2Reference reference)
        {
            return reference.Value;
        }
    }
}