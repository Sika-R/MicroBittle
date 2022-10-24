﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Environment;

public class BE2_Ins_PlaySound : BE2_InstructionBase, I_BE2_Instruction
{
    Dropdown _dropdown;

    //protected override void OnAwake()
    //{
    //    
    //}

    void PopulateDropdown()
    {
        _dropdown.ClearOptions();
        foreach (AudioClip clip in BE2_AudioManager.instance.audiosArray)
        {
            _dropdown.options.Add(new Dropdown.OptionData(clip.name));
        }
        _dropdown.value = 1;
        _dropdown.RefreshShownValue();
        _dropdown.value = 0;
    }

    protected override void OnStart()
    {
        _dropdown = GetSectionInputs(0)[0].Transform.GetComponent<Dropdown>();
        PopulateDropdown();
    }

    I_BE2_BlockSectionHeaderInput _input0;
    string _value;

    //protected override void OnPlay()
    //{
    //    
    //}

    public new void Function()
    {
        _input0 = Section0Inputs[0];
        _value = _input0.StringValue;

        int idx = _dropdown.options.FindIndex(option => option.text == _value);

        BE2_AudioManager.instance.PlaySound(idx);
        ExecuteNextInstruction();
    }
}
