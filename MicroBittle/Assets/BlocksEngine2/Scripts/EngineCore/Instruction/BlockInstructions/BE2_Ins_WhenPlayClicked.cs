﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Block;

public class BE2_Ins_WhenPlayClicked : BE2_InstructionBase, I_BE2_Instruction
{
    protected override void OnButtonPlay()
    {
        BlocksStack.IsActive = true;
    }

    //protected override void OnAwake()
    //{
    //
    //}

    //protected override void OnStart()
    //{
    //
    //}

    public new void Function()
    {
        ExecuteSection(0);
    }
}
