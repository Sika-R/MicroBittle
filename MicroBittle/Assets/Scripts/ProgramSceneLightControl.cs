using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramSceneLightControl : MonoBehaviour
{
    [SerializeField]
    GameObject black;
    bool hasAdd = false;
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
                if (ProgramUIMgr.Instance && !hasAdd)
                {
                    hasAdd = true;
                    ProgramUIMgr.Instance.AddSuccess();
                }
                return;
            }
        }
        black.SetActive(false);
    }
}
