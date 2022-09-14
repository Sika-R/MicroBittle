using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    Text text;
    int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = cnt + "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            cnt++;
            text.text = cnt + "";
        }
    }
}
