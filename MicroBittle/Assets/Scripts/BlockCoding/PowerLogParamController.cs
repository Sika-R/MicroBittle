using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerLogParamController : ParamController
{
    //Has two params for now:
    //paramInputs[0] : slider min
    //paramInputs[1] : slider max
    // Start is called before the first frame update
    [SerializeField]
    GameObject firstInputDataWarningMsg;
    [SerializeField]
    GameObject secondInputDataWarningMsg;

    void Awake()
    {
        // if (mode != 2)
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
        newData.text = "Wall";
        obstacleSelection.options.Add(newData);
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

    override public void DelegationInit()
    {
        base.DelegationInit();
        paramInputs[0].onValueChanged.AddListener(delegate { OnFirstInputFieldChange();  });
        paramInputs[1].onValueChanged.AddListener(delegate { OnSecondInputFieldChange(); });
    }

    void OnFirstInputFieldChange()
    {
        try
        {
            float parsed = float.Parse(paramInputs[0].text);
            if(parsed > 499)
            {
                firstInputDataWarningMsg.SetActive(true);
            }
            else
            {
                firstInputDataWarningMsg.SetActive(false);
            }
        } catch (Exception e)
        {
            firstInputDataWarningMsg.SetActive(false);
        }
        
    }

    void OnSecondInputFieldChange()
    {
        try
        {
            float parsed = float.Parse(paramInputs[1].text);
            if (parsed < 500)
            {
                secondInputDataWarningMsg.SetActive(true);
            }
            else
            {
                secondInputDataWarningMsg.SetActive(false);
            }
        }
        catch (Exception e)
        {
            secondInputDataWarningMsg.SetActive(false);
        }

    }





}
