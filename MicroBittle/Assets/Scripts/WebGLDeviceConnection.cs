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
    Accelerometer = 5
};

[System.Serializable]
public class MovementEvent : UnityEvent<MovementDirections>
{
}
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
    public UnityEvent pressAEvent;
    public UnityEvent pressBEvent;
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

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            OpenPort();
            text.text += "Try\n";
        }

        if(Input.GetMouseButton(1))
        {
            SendLine("Hello!");
        }
    }

    public void ReadLine(string str)
    {
        text.text += str;
        ParseLine(str);
        StartCoroutine(text.gameObject.GetComponent<DebugLogController>().ScrollBarBottom());
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
            }

        }
        catch (FormatException e)
        {
            Debug.Log("Can not parse instruction: " + str);
        }
         
    }


}
