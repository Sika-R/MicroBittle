using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramSceneLightControl : MonoBehaviour
{
    [SerializeField]
    GameObject black;
    int hasAdd = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Photoresistor.Instance && Photoresistor.Instance.gameObject.activeInHierarchy)
        {
            if(Photoresistor.Instance.lightStatus == Photoresistor.LightStatus.OFF)
            {
                black.SetActive(true);
                if (ProgramUIMgr.Instance && hasAdd == 0)
                {
                    hasAdd = 1;
                }
                return;
            }
            else
            {
                if (hasAdd == 1)
                {
                    hasAdd = 2;
                    ProgramUIMgr.Instance.AddSuccess();
                }
            }
        }
        black.SetActive(false);
        
    }
}
