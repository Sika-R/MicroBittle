using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VacuumCleanerParamController : ParamController
{
    //Has two params for now:
    //paramInputs[0] : start max

    void Awake()
    {
        // if (mode != 2)
        // {
        //  pinSelection.ClearOptions();
        // Dropdown.OptionData newData = new Dropdown.OptionData();
        // newData.text = "P1";
        // pinSelection.options.Add(newData);
        base.Awake();
        obstacleSelection.ClearOptions();
            Dropdown.OptionData newData = new Dropdown.OptionData();
            newData.text = "Spider Web";
            obstacleSelection.options.Add(newData);
        // }
    }

}
