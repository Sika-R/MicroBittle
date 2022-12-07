using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControllerProgramFlow_PlayMode : DialogueController
{
    public GameObject DialogueUI;
    public GameObject vacuum;
    public GameObject jackhammer;
    public GameObject headlamp;
    public GameObject powerlog;
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
        if (PlayerPrefs.GetString("mode") == "creativemode")
        {
            foreach (ParamManager.Obstacle o in ProgramUIMgr.Instance.allObstacles) { 
                string obstacleName = ProgramUIMgr.Instance.ObstacleToFunctionName(o);
                if (obstacleName == "Headlamp")
                {
                    headlamp.SetActive(true);
                }
                else if (obstacleName == "JackHammer")
                {
                    jackhammer.SetActive(true);
                }
                else if (obstacleName == "Vacuum Cleaner")
                {
                    vacuum.SetActive(true);
                }
                else {
                    powerlog.SetActive(true);
                }
            }
        }
        else {
            string mazeSelection = PlayerPrefs.GetString("mazeselection");
            if (mazeSelection == "DesertPyramid")
            {
                headlamp.SetActive(true);
                vacuum.SetActive(true);
            }
            else if (mazeSelection == "TundraCave")
            {
                jackhammer.SetActive(true);
                vacuum.SetActive(true);
            }
            else if (mazeSelection == "GrassLand")
            {
                headlamp.SetActive(true);
                jackhammer.SetActive(true);
            }
            DialogueUI.SetActive(true);
            DoInteraction();
        }
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
