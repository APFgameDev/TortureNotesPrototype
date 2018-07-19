using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Utilities.NS_Organizing;

//example if we want to also use KeyCode buttons as input for Axis
//define AxisPair
//using AxisPair = NS_Utilities.NS_Organizing.Pair<UnityEngine.KeyCode, UnityEngine.KeyCode>;

namespace NS_Managers.NS_Input
{
    public class KeyBoardMouseInputDevice : GameInputDevice<KeyCode, string>
    {
        //example if we want to also use KeyCode buttons as input for Axis
        //Dictionary<GamePlayAxis, Pair<KeyCode,KeyCode>[]> m_gamePlayAxisButtons;

        public KeyBoardMouseInputDevice()
        {
            //SetupDefaultButtons for now
            SetUpGamePlayInputsOnButton(GamePlayButtons.Jump, new KeyCode[] { KeyCode.Space, KeyCode.LeftShift });
            SetUpGamePlayInputsOnButton(GamePlayButtons.Shoot, new KeyCode[] { KeyCode.Mouse0 });
            SetUpGamePlayInputsOnButton(GamePlayButtons.Escape, new KeyCode[] { KeyCode.Escape });

            SetUpGamePlayInputsOnAxis(GamePlayAxis.FwdMove, new string[] { "Vertical" });
            SetUpGamePlayInputsOnAxis(GamePlayAxis.SideMove, new string[] { "Horizontal" });
            SetUpGamePlayInputsOnAxis(GamePlayAxis.LookX, new string[] { "Mouse X" });
            SetUpGamePlayInputsOnAxis(GamePlayAxis.LookY, new string[] { "Mouse Y" });

            //example if we want to also use KeyCode buttons as input for Axis
            //m_gamePlayAxisButtons.Add(GamePlayAxis.FwdMove, new AxisPair[] { new AxisPair(KeyCode.UpArrow, KeyCode.DownArrow) });
            //m_gamePlayAxisButtons.Add(GamePlayAxis.FwdMove, new AxisPair[] { new AxisPair(KeyCode.RightArrow, KeyCode.LeftArrow) });
        }

        protected override bool CheckButtonInput(KeyCode input)
        {
            return Input.GetKey(input);
        }

        protected override float GetAxisValue(string input)
        {
            return Input.GetAxis(input);
        }

        //example if we want to also use KeyCode buttons as input for Axis
        //protected override void ProcessAxis()
        //{
        //    base.ProcessAxis();

        //    foreach (var gameAxis in m_gamePlayAxisButtons)
        //        for (int i = 0; i < gameAxis.Value.Length; i++)
        //            AddValueToAxis(gameAxis.Key, GetButtonAxisValue(gameAxis.Value[i]));
        //}

        //example if we want to also use KeyCode buttons as input for Axis
        //float GetButtonAxisValue(AxisPair axisPair)
        //{
        //    float axisValue = 0f;

        //    if (Input.GetKey(axisPair.m_A))
        //        axisValue += 1f;
        //    if (Input.GetKey(axisPair.m_B))
        //        axisValue -= 1f;

        //    return axisValue;
        //}
    }
}
