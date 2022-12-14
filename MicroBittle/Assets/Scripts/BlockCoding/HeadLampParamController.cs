using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadLampParamController : ParamController
{
    //Has two params for now:
    //paramInputs[0] : dark max
    [SerializeField]
    public Dropdown compareSelection;
    void Awake()
    {
        // if(mode != 2)
        // {
        /*pinSelection.ClearOptions();
        Dropdown.OptionData newData = new Dropdown.OptionData();
        newData.text = "P1";
        pinSelection.options.Add(newData);*/
        base.Awake();
        obstacleSelection.ClearOptions();
        Dropdown.OptionData newData = new Dropdown.OptionData();
        newData.text = "";
        obstacleSelection.options.Add(newData);
        newData = new Dropdown.OptionData();
        
        // newData = new Dropdown.OptionData();
        newData.text = "Mouse";
        obstacleSelection.options.Add(newData);
        if(ProgramUIMgr.Instance)
        {
            SetObstacleOptions(ProgramUIMgr.Instance.allObstacles);
        }
        // }
    }
    public override void DelegationInit()
    {

        base.DelegationInit();

        if (compareSelection)
        {
            compareSelectionChanged(compareSelection);
            compareSelection.onValueChanged.AddListener(delegate { compareSelectionChanged(compareSelection); });
        }
    }

    void compareSelectionChanged(Dropdown d)
    {
        ParamManager.Instance.compare = d.value > 0;

    }

}
