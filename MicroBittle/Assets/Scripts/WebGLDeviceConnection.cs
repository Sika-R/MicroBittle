using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;
using UnityEngine.Events;


public enum MicrobitEventType
{
    Connected = 0,
    Disconnected = 1,
    ButtonAPressed = 2,
    ButtonBPressed = 3,
    LightLvl = 4,
    Accelerometer = 5,
    Slider = 6,
    Humid = 7,
};

[System.Serializable]
public class MovementEvent : UnityEvent<MovementDirections>{}
[System.Serializable]
public class InputEvent : UnityEvent<float, ObstacleType>{}
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
    bool isParsing = false;
    [SerializeField]
    Text text;

    
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
        pressAEvent.AddListener(() => OutfitMgr.Instance.ChangeOutfit(true));
        pressBEvent.AddListener(() => OutfitMgr.Instance.ChangeOutfit(false));
        sliderEvent.AddListener(ObstacleMgr.Instance.getInput);
        // pressAEvent.AddListener(() => ObstacleMgr.Instance.getInput(1, ObstacleType.ButtonA));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            OpenPort();
            text.text += "Try\n";
        }
        if(Input.GetMouseButtonDown(2))
        {
            pressAEvent.Invoke();
        }

    }

    public void ReadLine(string str)
    {
        if(isParsing) return;
        text.text += str;
        ParseLine(str);
        StartCoroutine(text.gameObject.GetComponent<DebugLogController>().ScrollBarBottom());
    }

    private void ParseLine(string str)
    {   
        if(str.Length < 2) return;
        isParsing = true;
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
                case MicrobitEventType.Accelerometer:
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
                    
                    break;
                case MicrobitEventType.Slider:
                    float sliderValue = float.Parse(str.Substring(1));
                    sliderEvent.Invoke(sliderValue, ObstacleType.Slider);
                    break;
                case MicrobitEventType.Humid:
                    float waterLvl = float.Parse(str.Substring(1));
                    sliderEvent.Invoke(waterLvl, ObstacleType.Humid);
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

}
