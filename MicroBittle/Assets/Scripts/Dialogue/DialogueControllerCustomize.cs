using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControllerCustomize : DialogueController
{
    public GameObject DialogueUI;
    public static DialogueControllerCustomize Instance_ = null;
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

    public override void IncreaseDialogueIndex()
    {
        if (dialogueIndex == 0 || dialogueIndex == 1)
        {
            DialogueUI.SetActive(false);
        }
        if (dialogueIndex == dialogues.Count - 1)
        {
            dialogueEndEvent.Invoke();
        }
        dialogueIndex++;
    }

    public void AfterTypeName()
    {
        DialogueUI.SetActive(true);
        DoInteraction();
    }
}
