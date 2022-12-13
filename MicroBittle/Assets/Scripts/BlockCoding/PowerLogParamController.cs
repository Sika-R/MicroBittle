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
        inputDataWarningMsg.GetComponent<Text>().text = "Try a whole number between 0 and 1000!";
        try
        {
            float parsed = float.Parse(i.text);
            if (parsed < 0 || parsed > 1000)
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
                    if (parsed < 0 || parsed > 1000)
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
            if(one - zero > 0 && one - zero < 300)
            {
                inputDataWarningMsg.SetActive(true);
                paramInputs[0].image.color = Color.red;
                paramInputs[1].image.color = Color.red;
                inputDataWarningMsg.GetComponent<Text>().text = "Try numbers with a difference greater than 300!";
            }
            else if(one - zero <= 0)
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

    /*override public void DelegationInit()
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

    }*/





}
