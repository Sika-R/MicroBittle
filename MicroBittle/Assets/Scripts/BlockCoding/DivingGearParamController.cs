using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivingGearParamController : ParamController
{
    //Has nine params for now:
    //paramInputs[0] : wetsuit min
    //paramInputs[1] : wetsuit max
    //paramInputs[2] : in how many seconds
    //paramInputs[3] : diving vest min
    //paramInputs[4] : diving vest  max
    //paramInputs[5] : in how many seconds
    //paramInputs[6] : oxygen tank min
    //paramInputs[7] : oxygen tank max
    //paramInputs[8] : in how many seconds

    void Awake()
    {
        if(mode != 2)
        {
            pinSelection.ClearOptions();
            Dropdown.OptionData newData = new Dropdown.OptionData();
            newData.text = "P2";
            pinSelection.options.Add(newData);

            obstacleSelection.ClearOptions();
            newData = new Dropdown.OptionData();
            newData.text = "Waterfall";
            obstacleSelection.options.Add(newData);
        }
    }
}
