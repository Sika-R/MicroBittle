using UnityEngine;

public abstract class DialogueNode : ScriptableObject
{
    [SerializeField]
    private NarrationLine m_DialogueLine;

    public NarrationLine DialogueLine => m_DialogueLine;
    public AudioClip dialogueAudio;
    public string cutsceneImage;
    public string animationName;

    public abstract bool CanBeFollowedByNode(DialogueNode node);
    public abstract void Accept(DialogueNodeVisitor visitor);
}