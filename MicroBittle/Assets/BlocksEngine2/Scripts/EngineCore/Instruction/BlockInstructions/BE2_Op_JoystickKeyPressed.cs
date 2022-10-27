using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Block;

public class BE2_Op_JoystickKeyPressed : BE2_InstructionBase, I_BE2_Instruction
{
    Dropdown _dropdown;
    BE2_VirtualJoystick _virtualJoystick;

    //protected override void OnAwake()
    //{
    //
    //}

    protected override void OnStart()
    {
        _dropdown = Section0Inputs[0].Transform.GetComponent<Dropdown>();
        _virtualJoystick = BE2_VirtualJoystick.instance;
    }

    public new string Operation()
    {
        if (_virtualJoystick.keys[_dropdown.value].isPressed)
        {
            return "1";
        }
        else
        {
            return "0";
        }
    }
}
