using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStartAtBeginning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogueController.Instance.DoInteraction();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
