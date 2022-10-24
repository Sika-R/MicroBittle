using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Block;

public class BE2_Ins_WhenKeyPressed : BE2_InstructionBase, I_BE2_Instruction
{
    Dropdown _dropdown;

    //protected override void OnAwake()
    //{
    //     
    //}

    protected override void OnStart()
    {
        _dropdown = Section0Inputs[0].Transform.GetComponent<Dropdown>();

        PopulateDropdown();
        _dropdown.value = _dropdown.options.FindIndex(option => option.text == "A");
        ParseKeyCode();
        _dropdown.onValueChanged.AddListener(delegate { ParseKeyCode(); });
    }

    void PopulateDropdown()
    {
        _dropdown.ClearOptions();
        string[] keys = System.Enum.GetNames(typeof(KeyCode));
        foreach (string key in keys)
        {
            _dropdown.options.Add(new Dropdown.OptionData(key));
        }
        _dropdown.RefreshShownValue();
    }

    void Update()
    {
        if (Input.GetKey(_key))
        {
            BlocksStack.IsActive = true;
        }
    }

    KeyCode _key;
    void ParseKeyCode()
    {
        KeyCode key = KeyCode.A;
        try
        {
            key = (KeyCode)System.Enum.Parse(typeof(KeyCode), Section0Inputs[0].StringValue);
        }
        catch { }
        _key = key;
    }

    public new void Function()
    {
        ExecuteSection(0);
    }

    public new string Operation()
    {
        if (Input.GetKey(_key))
            return "1";
        else
            return "0";
    }
}
