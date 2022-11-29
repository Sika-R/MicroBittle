using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintBlink : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine("Blink");
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            this.gameObject.GetComponent<Image>().enabled = !GetComponent<Image>().enabled;
        }
    }
}
