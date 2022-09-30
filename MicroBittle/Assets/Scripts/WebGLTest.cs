using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using UnityEngine.UI;


public class WebGLTest : MonoBehaviour
{
    [SerializeField]
    Text debugText;
    public void GetPortName(string str)
    {
        debugText.text += str + "\n";
    }

}
