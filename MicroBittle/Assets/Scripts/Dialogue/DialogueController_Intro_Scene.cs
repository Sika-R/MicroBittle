using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController_Intro_Scene : DialogueController
{
    public GameObject background;
    public GameObject platform;
    void Start()
    {
        
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void IncreaseDialogueIndex()
    {
        if (dialogueIndex == 0) {
            background.SetActive(true);
            platform.SetActive(false);
        } else if (dialogueIndex == 1)
        {
            background.SetActive(false);
            platform.SetActive(true);
        }
        if (dialogueIndex == dialogues.Count - 1)
        {
            dialogueEndEvent.Invoke();
        }
        dialogueIndex++;
    }
}
