using Annotation.SO;
using Annotation.SO.UnityEvents;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Input Event Container", menuName = "SO Variables/Input Event Container", order = 10)]
public class InputEventContainer : ScriptableObject
{
    [SerializeField] private FloatVariable m_HeldPercentage;
    [SerializeField] private FloatReference m_DeadZone;
    [SerializeField] private BoolVariable m_IsBeingHeld;
    
    [SerializeField] private UnityEventVoidSO m_Pressed;
    [SerializeField] private UnityEventVoidSO m_Held;
    [SerializeField] private UnityEventVoidSO m_Released;
   
    public void CalculateInputData(float currentInputValue)
    {     
        //Check to see if the previous Trigger value is less then the deadzone and if the current value is greater then the deadzone. If this statement is true then the Trigger is being grabbed for the first time this frame
        if (m_HeldPercentage.Value < m_DeadZone.Value && currentInputValue >= m_DeadZone.Value)
        {
            m_IsBeingHeld.Value = true;
            m_Pressed.UnityEvent.Invoke();            
        }

        //if it wasn't first held on this frame then just check to see if the previous value was greater then the deadzone, but the current value isn't. If this statement is true then the Trigger is no longer being held
        if (m_HeldPercentage.Value >= m_DeadZone.Value && currentInputValue < m_DeadZone.Value)
        {
            m_IsBeingHeld.Value = false;
            m_Released.UnityEvent.Invoke();
        }

        //if the current input value
        if (currentInputValue >= m_DeadZone.Value)
        {
            m_Held.UnityEvent.Invoke();
        }

        m_HeldPercentage.Value = currentInputValue;
    }
}
