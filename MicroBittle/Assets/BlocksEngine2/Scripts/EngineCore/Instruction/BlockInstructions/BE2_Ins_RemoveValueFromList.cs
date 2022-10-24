using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;

// v2.9 - new block
public class BE2_Ins_RemoveValueFromList : BE2_InstructionBase, I_BE2_Instruction
{
    protected override void OnStart()
    {
        _variablesManager = BE2_VariablesListManager.instance;
        _dropdown = GetSectionInputs(0)[1].Transform.GetComponent<Dropdown>();
        _dropdown.onValueChanged.AddListener(delegate { _lastValue = _dropdown.options[_dropdown.value].text; });

        BE2_MainEventsManager.Instance.StartListening(BE2EventTypes.OnAnyVariableAddedOrRemoved, PopulateDropdown);

        PopulateDropdown();
    }

    void OnDisable()
    {
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypes.OnAnyVariableAddedOrRemoved, PopulateDropdown);
    }

    void PopulateDropdown()
    {
        _dropdown.ClearOptions();
        foreach (KeyValuePair<string, List<string>> variable in _variablesManager.lists)
        {
            _dropdown.options.Add(new Dropdown.OptionData(variable.Key));
        }
        _dropdown.RefreshShownValue();
        _dropdown.value = _dropdown.options.FindIndex(option => option.text == _lastValue);
    }

    string _lastValue;
    Dropdown _dropdown;
    BE2_VariablesListManager _variablesManager;

    public new void Function()
    {
        _variablesManager.RemoveListItem(Section0Inputs[1].StringValue, Section0Inputs[0].StringValue);

        ExecuteNextInstruction();
    }
}