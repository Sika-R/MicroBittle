using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
        newData.text = "";
        obstacleSelection.options.Add(newData);
        newData = new Dropdown.OptionData();
            newData.text = "Spider Web";
            obstacleSelection.options.Add(newData);
        if (ProgramUIMgr.Instance)
        {
            SetObstacleOptions(ProgramUIMgr.Instance.allObstacles);
        }
        // }
    }

    public override void ParamValueChanged(InputField i)
    {
        int idx = paramInputs.IndexOf(i);
        inputDataWarningMsg.GetComponent<Text>().text = "Try a whole number between 100 and 500!";
        try
        {
            float parsed = float.Parse(i.text);
            if (parsed < 100 || parsed > 500)
            {
                i.image.color = Color.red;
            }
            else
            {
                allParams[idx] = parsed;
                i.image.color = Color.white;
            }
            foreach (InputField input in paramInputs)
            {
                try
                {
                    parsed = float.Parse(input.text);
                    if (parsed < 100 || parsed > 500)
                    {
                        inputDataWarningMsg.SetActive(true);
                        return;
                    }
                }
                catch (Exception e)
                {
                    inputDataWarningMsg.SetActive(true);
                    return;
                }
            }
            
            inputDataWarningMsg.SetActive(false);

        }
        catch (Exception e)
        {
            i.image.color = Color.red;
            Debug.Log(e);
        }
    }

}
