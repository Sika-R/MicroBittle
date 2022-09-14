using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleLEDController : MonoBehaviour
{
    public SerialController serialController;
    [SerializeField]
    Text text;
    int cnt;
    [SerializeField]
    Slider slider;
    int lightLvl = 0;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Cnt: 0";
        if(!serialController)
        {
            serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            serialController.SendSerialMessage("u");
            serialController.SendSerialMessage("u");
        }

        ReceiveFeedback();
    }

    void ReceiveFeedback()
    {
        string message = serialController.ReadSerialMessage();
        if (message == null)
            return;
        Debug.Log(int.Parse(message.ToString()));
        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else if (message.ToString().Substring(0, 2) == "hi")
        {
            cnt++;
            text.text = "Cnt: " + cnt;
        }
        else if(int.Parse(message.ToString()) != 0)
        {
            lightLvl = int.Parse(message.ToString());
            slider.value = (float)lightLvl / 255.0f;
        }
        else
            Debug.Log("Message arrived: " + message);
    }
}
