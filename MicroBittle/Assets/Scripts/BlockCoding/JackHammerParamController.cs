using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackHammerParamController : ParamController
{
    //Has two params for now:
    //paramInputs[0] : slider min
    //paramInputs[1] : slider max
    // Start is called before the first frame update
    void Awake()
    {
        // if(mode != 2)
        // {
        /*pinSelection.ClearOptions();
        Dropdown.OptionData newData = new Dropdown.OptionData();
        newData.text = "P0";
        pinSelection.options.Add(newData);*/
        base.Awake();
        obstacleSelection.ClearOptions();
            Dropdown.OptionData newData = new Dropdown.OptionData();
        newData.text = "";
        obstacleSelection.options.Add(newData);
        newData = new Dropdown.OptionData();
            newData.text = "Rock";
            obstacleSelection.options.Add(newData);
        if (ProgramUIMgr.Instance)
        {
            SetObstacleOptions(ProgramUIMgr.Instance.allObstacles);
        }
        // }
        /*else if(mode == 2)
        {
            
            for(int i = 0; i < 2; i++)
            {
                Dropdown.OptionData newData = new Dropdown.OptionData();
                newData.text = "P" + i;
                pinSelection.options.Add(newData);
            }
            newData = new Dropdown.OptionData();
            newData.text = "Rock";
            pinSelection.options.Add(newData);
        }*/
    }


}
