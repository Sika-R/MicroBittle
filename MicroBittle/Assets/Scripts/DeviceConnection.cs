using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using UnityEngine.UI;
using System.IO;
using Microsoft.Win32; 

public enum MicrobitEventType
{
    Connected,
    Disconnected,
    ButtonAPressed,
    ButtonBPressed,
    LightLvl
};

public enum UnityEventType
{
    Connected,
    Disconnected
}

public class DeviceConnection : MonoBehaviour
{
    public const string MICROBIT_CONNECTED = "__MicrobitConnected__";

    [SerializeField]
    SerialController serialController;
    [SerializeField]
    Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        serialController.gameObject.SetActive(false);
        int cnt = 0;
        while(serialController.gameObject.activeSelf == false && cnt < 3)
        {
            cnt++;
            foreach (string portname in SerialPort.GetPortNames())
            {
                debugText.text += portname + "\n";
                try
                {
                    serialController.portName = portname;
                    serialController.gameObject.SetActive(true);
                    serialController.SendSerialMessage("0__Connected__\n");
                    Thread.Sleep(500);
                    if(!VerifyConnection(portname))
                    {
                        serialController.gameObject.SetActive(false);
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Debug.Log("device NOT connected to: " + portname);
                }
            }
        }
        string[] allDevices = LinuxGetPortNames();
        foreach(string deviceName in allDevices)
        {
            debugText.text += deviceName + "\n";
        }

        // IEnumerable<string> portPaths = Directory.GetFiles("/dev/", "tty*").Where(p => p.StartsWith("/dev/ttyS") || p.StartsWith("/dev/ttyUSB") || p.StartsWith("/dev/ttyACM" || p.StartsWith("/dev/ttyAMA")));
        // debugText.text += portPaths.Count() + "\n";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AttemptConnection(String portName)
    {
        SerialPort serialPort = new SerialPort(portName, 115200);
        serialPort.ReadTimeout = 100;
        serialPort.WriteTimeout = 100;
        serialPort.Open();
        // SendToWire(serialPort, SerialController.SERIAL_DEVICE_CONNECTED);
    }

    bool VerifyConnection(string portname)
    {
        string message = serialController.ReadSerialMessage();
        while(message != null)
        {
            debugText.text += message + "\n";
            Debug.Log(message);
            int prefix = (int)Char.GetNumericValue(message[0]);
            if(prefix == (int)MicrobitEventType.Connected)
            {
                return true;
            }
            message = serialController.ReadSerialMessage();
        }
        return false;
    }

    public static string [] LinuxGetPortNames()
    {
        int p = (int) Environment.OSVersion.Platform;
        List<string> serial_ports = new List<string>();
        
        // Are we on Unix?
        if (p == 4 || p == 128 || p == 6) {
            string[] ttys = Directory.GetFiles("/dev/", "tty*");
            bool linux_style = false;

            //
            // Probe for Linux-styled devices: /dev/ttyS* or /dev/ttyUSB*
            // 
            foreach (string dev in ttys) {
                if (dev.StartsWith("/dev/ttyS") || dev.StartsWith("/dev/ttyUSB") || dev.StartsWith("/dev/ttyACM")) {
                    linux_style = true;
                    break;
                }
            }

            foreach (string dev in ttys) {
                if (linux_style){
                    if (dev.StartsWith("/dev/ttyS") || dev.StartsWith("/dev/ttyUSB") || dev.StartsWith("/dev/ttyACM"))
                        serial_ports.Add (dev);
                } else {
                    if (dev != "/dev/tty" && dev.StartsWith ("/dev/tty") && !dev.StartsWith ("/dev/ttyC"))
                        serial_ports.Add (dev);
                }
            }
        } else {
            using (RegistryKey subkey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM"))
            {
                if (subkey != null) {
                    string[] names = subkey.GetValueNames();
                    foreach (string value in names) {
                        string port = subkey.GetValue(value, "").ToString();
                        if (port != "")
                            serial_ports.Add(port);
                    }
                }
            }
        }
        return serial_ports.ToArray();
    }


}
