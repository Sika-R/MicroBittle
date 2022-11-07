using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance = null;
    [SerializeField] List<UnityEvent> dialogues;
    [SerializeField] UnityEvent m_OnInteraction;
    [SerializeField] UnityEvent dialogueEndEvent;
    int dialogueIndex = 0;
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoInteraction();
        }
    }

    public void DoInteraction()
    {
        if (dialogueIndex > dialogues.Count - 1)
        {
            return;
        }
        dialogues[dialogueIndex].Invoke();
    }

    public void IncreaseDialogueIndex()
     {
        if (dialogueIndex == dialogues.Count - 1)
        {
            dialogueEndEvent.Invoke();
        }
        dialogueIndex++;
    }

    public void PlayDialogue(int nodeIndex)
    {
        dialogueIndex = nodeIndex;
        DoInteraction();
    }

    public void PlayDialogue()
    {
        DoInteraction();
    }
}
