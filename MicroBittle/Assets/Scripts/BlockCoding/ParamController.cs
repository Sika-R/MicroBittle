using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum FunctionType
{
    jackhammer,
    headlamp,
    divinggear
}

public class ParamController : MonoBehaviour
{
    
    public int mode = 0;
    [SerializeField]
    public Dropdown pinSelection;
    [SerializeField]
    public Dropdown obstacleSelection;
    [SerializeField]
    public List<InputField> paramInputs = new List<InputField>();
    [SerializeField]
    FunctionType functionType;
    public int pinNum;
    public ParamManager.Obstacle obstacle;
    public List<float> allParams = new List<float>();
    // Start is called before the first frame update
    void Start()
    {
        if(pinSelection)
        {
            pinSelection.value = 0;
            PinValueChanged(pinSelection);
            pinSelection.onValueChanged.AddListener(delegate{PinValueChanged(pinSelection);});
        }
        if(obstacleSelection)
        {
            obstacleSelection.value = 0;
            ObstacleValueChanged(obstacleSelection);
            obstacleSelection.captionText.text = obstacleSelection.options[0].text;
            MG_BlocksEngine2.Block.BE2_DropdownDynamicResize dropdownResize = obstacleSelection.GetComponent<MG_BlocksEngine2.Block.BE2_DropdownDynamicResize>();
            dropdownResize.Resize();
            obstacleSelection.onValueChanged.AddListener(delegate{ObstacleValueChanged(obstacleSelection);});
        }

        for(int i = 0; i < paramInputs.Count; i++)
        {
            allParams.Add(0);
            ParamValueChanged(paramInputs[i]);
            MG_BlocksEngine2.Block.BE2_InputFieldDynamicResize inputResize = paramInputs[i].GetComponent<MG_BlocksEngine2.Block.BE2_InputFieldDynamicResize>();
            inputResize.Resize();
            InputField inputField = paramInputs[i];
            inputField.onValueChanged.AddListener(delegate{ParamValueChanged(inputField);});
        }
        
        if(mode == 0)
        {
            for(int i = 0; i < paramInputs.Count; i++)
            {
                
                paramInputs[i].readOnly = true;
            }
        }
        SaveParams();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PinValueChanged(Dropdown d)
    {
        pinNum = d.value;
        if(ParamManager.Instance)
        {
            ParamManager.Instance.SetPin(pinNum, obstacle);
        }
    }

    void ObstacleValueChanged(Dropdown o)
    {
        string str = o.options[o.value].text;
        if(str == "Rock")
        {
            obstacle = ParamManager.Obstacle.rock;
        }
        else if(str == "Mouse")
        {
            obstacle = ParamManager.Obstacle.mouse;
        }
        else if(str == "Waterfall")
        {
            obstacle = ParamManager.Obstacle.waterfall;
        }
        else if(str == "Spider Web")
        {
            obstacle = ParamManager.Obstacle.spiderweb;
        }
        else if(str == "Wall")
        {
            obstacle = ParamManager.Obstacle.wall;
        }
        if(ParamManager.Instance)
        {
            ParamManager.Instance.SetPin(pinNum, obstacle);
            ParamManager.Instance.SetFunction(functionType, obstacle);
        }
    }

    void ParamValueChanged(InputField i)
    {
        int idx = paramInputs.IndexOf(i);
        try
        {
            allParams[idx] = float.Parse(i.text);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
        
    }

    void SaveParams()
    {
        if(ParamManager.Instance)
        {
            ParamManager.Instance.SetParam(obstacle, allParams);
        }
        else
        {
            Debug.Log("Failed to save.");
        }
    }


}