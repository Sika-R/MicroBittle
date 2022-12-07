using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControllerPlayMode : DialogueController
{
    public GameObject DialogueUI;
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
    // Start is called before the first frame update
    void Start()
    {
        DialogueUI.SetActive(true);
        DoInteraction();
    }
    public override void IncreaseDialogueIndex()
    {
        if (dialogueIndex == 0)
        {
            DialogueUI.SetActive(false);
        }
        if (dialogueIndex == dialogues.Count - 1)
        {
            dialogueEndEvent.Invoke();
        }
        dialogueIndex++;
    }
}
