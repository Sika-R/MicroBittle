using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControllerProgramFlow_PlayMode : DialogueController
{
    public GameObject DialogueUI;
    public GameObject vacuum;
    public GameObject jackhammer;
    public GameObject headlamp;
    public static DialogueControllerProgramFlow_PlayMode Instance_ = null;
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

    private void Start()
    {
        //PlayerPrefs.SetString("mazeselection", "DesertPyramid");
        string mazeSelection = PlayerPrefs.GetString("mazeselection");
        if (mazeSelection == "DesertPyramid") {
            headlamp.SetActive(true);
            vacuum.SetActive(true);
        }
        else if (mazeSelection == "TundraCave") {
            jackhammer.SetActive(true);
            vacuum.SetActive(true);
        }
        else if (mazeSelection == "GrassLand") {
            headlamp.SetActive(true);
            jackhammer.SetActive(true);
        }
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
