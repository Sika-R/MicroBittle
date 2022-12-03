using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControllerProgramFlow_StoryMode : DialogueController
{
    public GameObject DialogueUI;
    public GameObject PowerLog;
    public static DialogueControllerProgramFlow_StoryMode Instance_ = null;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance_ != null && Instance_ != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance_ = this;
        }

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
        return;
    }
    // Start is called before the first frame update
    void Start()
    {
        DoInteraction();
    }

    public override void IncreaseDialogueIndex()
    {
        if (dialogueIndex == 0) {
            PowerLog.SetActive(true);
            StartCoroutine(doInteraction());
        }
        else if (dialogueIndex == 1)
        {
            PowerLog.SetActive(false);
            DialogueUI.SetActive(false);
        }
        else if (dialogueIndex == 2)
        {
            DialogueUI.SetActive(false);
        }
        if (dialogueIndex == dialogues.Count - 1)
        {
            dialogueEndEvent.Invoke();
        }
        dialogueIndex++;
    }

    public void AfterFollowingInstruction()
    {
        DialogueUI.SetActive(true);
        DoInteraction();
    }

    IEnumerator doInteraction()
    {
        yield return new WaitForSeconds(1);
        DoInteraction();
    }
}
