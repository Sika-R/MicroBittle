using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControllerTransitionToStoryMode : DialogueController
{
    public GameObject Keys;
    public GameObject Gem;
    public GameObject Microbit;
    public static DialogueControllerTransitionToStoryMode Instance_ = null;
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

    // Start is called before the first frame update
    void Start()
    {
        DoInteraction();
        Keys.SetActive(true);
    }

    public override void IncreaseDialogueIndex()
    {
        if (dialogueIndex == 0)
        {
            Gem.SetActive(true);
            Keys.SetActive(false);
            StartCoroutine(doInteraction());
        }
        else if (dialogueIndex == 1)
        {
            Gem.SetActive(false);
            Microbit.SetActive(true);
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
