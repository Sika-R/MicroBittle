using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController_Intro_Scene : DialogueController
{
    public GameObject background;
    public GameObject platform;
    public GameObject cutscene1;
    public GameObject cutscene2;
    public GameObject cutscene3;
    public GameObject cutscene4;
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
            //background.SetActive(true);
            //platform.SetActive(false);
            StartCoroutine(doInteraction());
        } else if (dialogueIndex == 1)
        {
            background.SetActive(false);
            //platform.SetActive(true);
            StartCoroutine(doInteraction());
        }
        if (dialogueIndex == dialogues.Count - 1)
        {
            dialogueEndEvent.Invoke();
        }
        dialogueIndex++;
    }

    IEnumerator doInteraction()
    {
        yield return new WaitForSeconds(1);
        DoInteraction();
    }
}
