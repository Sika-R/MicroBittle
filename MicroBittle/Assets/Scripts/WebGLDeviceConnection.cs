using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.Events;


public enum MicrobitEventType
{
    Connected = 0,
    Disconnected = 1,
    ButtonAPressed = 2,
    ButtonBPressed = 3,
    P0 = 4,
    P1 = 5,
    P2 = 6
};

[System.Serializable]
public class MovementEvent : UnityEvent<MovementDirections>{}
[System.Serializable]
public class InputEvent : UnityEvent<float, ObstacleType>{}
[System.Serializable]
public class FloatEvent : UnityEvent<float>{}
public class WebGLDeviceConnection : MonoBehaviour
{
    private static WebGLDeviceConnection _instance;
    public static WebGLDeviceConnection Instance { get { return _instance; } }
    [DllImport("__Internal")]
    private static extern void OpenPort();
    //[DllImport("__Internal")]
    //private static extern void ReadLine();
    [DllImport("__Internal")]
    private static extern void SendLine(string str);

    public MovementEvent movementEvent = new MovementEvent();
    public UnityEvent pressAEvent = new UnityEvent();
    public UnityEvent pressBEvent;
    public InputEvent sliderEvent;
    public FloatEvent sliderValueEvent;
    public FloatEvent waterValueEvent;
    public FloatEvent lightValueEvent;
    bool isParsing = false;
    [SerializeField]
    Text text;
    bool temp = false;

    StringBuilder inputBuffer = new StringBuilder("");
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(OutfitMgr.Instance)
        {
            pressAEvent.AddListener(() => OutfitMgr.Instance.ChangeOutfit(true));
            pressBEvent.AddListener(() => OutfitMgr.Instance.ChangeOutfit(false));
        }
        if(ObstacleMgr.Instance)
        {
            sliderEvent.AddListener(ObstacleMgr.Instance.getInput);
        }
        if(programUI.Instance)
        {
            sliderValueEvent.AddListener(programUI.Instance.sliderforJackhamer);
            waterValueEvent.AddListener(programUI.Instance.sliderforDivingGear);
            lightValueEvent.AddListener(programUI.Instance.sliderforHeadLamp);
        }
        // pressAEvent.AddListener(() => ObstacleMgr.Instance.getInput(1, ObstacleType.ButtonA));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(temp)
            {
                ParseLine("41024");
                temp = false;
            }
            else
            {
                ParseLine("40");
                temp = true;
            }
            
            // OpenPort();
            if(text)
            {
                text.text += "Try\n";
            }
            
        }
        if(Input.GetMouseButtonDown(2))
        {
            if(temp)
            {
                ParseLine("51024");
                temp = false;
            }
            else
            {
                ParseLine("50");
                temp = true;
            }
            // pressAEvent.Invoke();
        }


        if(Input.GetMouseButtonDown(0))
        {
            if(temp)
            {
                ParseLine("61024");
                temp = false;
            }
            else
            {
                ParseLine("60");
                temp = true;
            }
            // pressAEvent.Invoke();
        }

    }

    /*public void ReadLine(string str)
    {
        // if(isParsing) return;
        text.text += str.Length + ": ";
        text.text += str;
        if(str == "") return;
        ParseLine(str);
        StartCoroutine(text.gameObject.GetComponent<DebugLogController>().ScrollBarBottom());
    }*/

    public void ReadLine(string str)
    {
        inputBuffer.Append(str);
        string all = inputBuffer.ToString();
        // text.text = all;
        int idx = all.IndexOf("\n");
        
        if(idx != -1)
        {
            string input = all.Substring(0, idx + 1);
            inputBuffer.Remove(0, idx + 1);
            ParseLine(input);
        }
        if(text)
        {
            text.text = inputBuffer.ToString();
        }
        
    }

    private void ParseLine(string str)
    {   
        try
        {
            int messageType = Int32.Parse(str.Substring(0, 1));
            MicrobitEventType type = (MicrobitEventType)messageType;
            switch(type)
            {
                case MicrobitEventType.ButtonAPressed:
                    text.text += "Text: " + str + " APressed\n";
                    pressAEvent.Invoke();
                    break;
                case MicrobitEventType.ButtonBPressed:
                    pressBEvent.Invoke();
                    break;
                /*case MicrobitEventType.Accelerometer:
                    int movementDirection;
                    if(Int32.TryParse(str.Substring(1, 1), out movementDirection))
                    {
                        if(movementDirection != 0)
                        {
                            movementEvent.Invoke((MovementDirections)movementDirection);
                        }
                    }
                    else
                    {
                        Debug.Log("Can not parse direction: " + str);
                    }
                    
                    break;*/
                case MicrobitEventType.P0:
                    float sliderValue = float.Parse(str.Substring(1));
                    // sliderValue /= 20;
                    text.text += "Slider: " + sliderValue + " \n";
                    sliderEvent.Invoke(sliderValue, ObstacleType.Slider);
                    sliderValueEvent.Invoke(sliderValue);
                    // sliderValueEvent.Invoke(Mathf.Floor(sliderValue / 50));
                    break;
                case MicrobitEventType.P1:
                    float waterLvl = float.Parse(str.Substring(1));
                    waterLvl = 1024 - waterLvl;
                    // waterLvl = (1000 - waterLvl) / 7; 
                    text.text += "Water: " + waterLvl + " \n";
                    sliderEvent.Invoke(waterLvl, ObstacleType.Humid);
                    waterValueEvent.Invoke(waterLvl);
                    // waterValueEvent.Invoke(Mathf.Floor(waterLvl / 30));
                    break;

                case MicrobitEventType.P2:
                    float lightLvl = float.Parse(str.Substring(1));
                    lightLvl = 1024 - lightLvl;
                    text.text += "Light: " + lightLvl + " \n";
                    sliderEvent.Invoke(lightLvl, ObstacleType.Light);
                    lightValueEvent.Invoke(lightLvl);
                    break;
            }

        }
        catch (FormatException e)
        {
            Debug.Log("Can not parse instruction: " + str);
        }
        finally
        {
            isParsing = false;
        }
         
    }

    /*public static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase)
    {
        int len = value.Length;
        int max = (sb.Length - len) + 1;
        var v1 = (ignoreCase)
            ? value.ToLower() : value;
        var func1 = (ignoreCase)
            ? new Func<char, char, bool>((x, y) => char.ToLower(x) == y)
            : new Func<char, char, bool>((x, y) => x == y);
        for (int i1 = startIndex; i1 < max; ++i1)
            if (func1(sb[i1], v1[0]))
            {
                int i2 = 1;
                while ((i2 < len) && func1(sb[i1 + i2], v1[i2]))
                    ++i2;
                if (i2 == len)
                    return i1;
            }
        return -1;
    }*/

}
