using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photoresistor : MonoBehaviour
{
    public Light FlashLight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputLightVal(int val)
    {
        FlashLight.GetComponent<Light>().intensity = val;
    }
}
