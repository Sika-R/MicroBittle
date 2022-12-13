using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    public override void ParamValueChanged(InputField i)
    {
        int idx = paramInputs.IndexOf(i);
        inputDataWarningMsg.GetComponent<Text>().text = "Try a whole number between 200 and 700!";
        try
        {
            float parsed = float.Parse(i.text);
            if (parsed < 200 || parsed > 700)
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
                    if (parsed < 200 || parsed > 700)
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
            float zero = float.Parse(paramInputs[0].text);
            float one = float.Parse(paramInputs[1].text);
            if (one - zero > 0 && one - zero < 200)
            {
                inputDataWarningMsg.SetActive(true);
                paramInputs[0].image.color = Color.red;
                paramInputs[1].image.color = Color.red;
                inputDataWarningMsg.GetComponent<Text>().text = "Try numbers with a difference greater than 200!";
            }
            else if (one - zero <= 0)
            {
                inputDataWarningMsg.SetActive(true);
                paramInputs[0].image.color = Color.red;
                paramInputs[1].image.color = Color.red;
                inputDataWarningMsg.GetComponent<Text>().text = "Try making the second number greater than the first one!";
            }
            else
            {
                paramInputs[0].image.color = Color.white;
                paramInputs[1].image.color = Color.white;
                inputDataWarningMsg.SetActive(false);
            }

        }
        catch (Exception e)
        {
            i.image.color = Color.red;
            Debug.Log(e);
        }
    }


}
