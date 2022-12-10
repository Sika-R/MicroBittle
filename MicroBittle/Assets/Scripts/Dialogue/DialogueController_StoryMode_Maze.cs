using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController_StoryMode_Maze : DialogueController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void IncreaseDialogueIndex()
    {
        if (dialogueIndex == 0)
        {
            StartCoroutine(doInteraction());
        }
        else {
            StartCoroutine(ChangeScene());
        }
        dialogueIndex++;
    }

    IEnumerator doInteraction() {
        yield return new WaitForSeconds(5f);
        DoInteraction();
    }

    IEnumerator ChangeScene() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Start");
    }
}
