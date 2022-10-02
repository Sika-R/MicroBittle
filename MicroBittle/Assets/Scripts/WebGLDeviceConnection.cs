using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;


public class WebGLDeviceConnection : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenPort();
    //[DllImport("__Internal")]
    //private static extern void ReadLine();
    [DllImport("__Internal")]
    private static extern void SendLine(string str);

    [SerializeField]
    Text text;

   
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
    }

}
