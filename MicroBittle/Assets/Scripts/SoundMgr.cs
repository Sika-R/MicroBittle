using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundMgr : MonoBehaviour
{
    public List<AudioClip> dialogues;
    public List<AudioClip> audios;
    public static SoundMgr Instance = null;
    public AudioSource bgm;
    bool isPlaying = false;
    AudioSource audioSource;
    public Sprite[] muteSprite = new Sprite[2];
    public Image muteButton;

    private float ringClipLength = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
    void Update()
    {

    }
    public void PlayDialogue(string clipName)
    {
        audioSource.PlayOneShot(dialogues.Find(x => x.name.Equals(clipName)));
    }

    public void PlayDialogue(string clipName, AudioSource AS)
    {
        AS.PlayOneShot(dialogues.Find(x => x.name.Equals(clipName)));
    }

    public void PlayAudio(string clipName)
    {
        Debug.Log(clipName);
        AudioClip ac = audios.Find(x => x.name.Equals(clipName));
        if(isPlaying)
        {
            return;
        }
        if (ac)
        {
            StartCoroutine(Play(ac.length));
            audioSource.PlayOneShot(ac);
        }
        
    }
    public void PlayAudio(AudioClip clip)
    {
        if(isPlaying)
        {
            return;
        }
        if (clip)
        {
            StartCoroutine(Play(clip.length));
            audioSource.PlayOneShot(clip);
        }
        
    }

    public void PlayOnce(string clipName)
    {
        audioSource.PlayOneShot(audios.Find(x => x.name.Equals(clipName)));
    }

    public void PlayAudio(string clipName, AudioSource AS)
    {
        AS.PlayOneShot(audios.Find(x => x.name.Equals(clipName)));
    }

    public void StopAudio()
    {
        audioSource.Pause();
    }

    public void StopAudio(AudioSource AS)
    {
        AS.Pause();
    }

    public void Mute()
    {
        bgm.mute = !bgm.mute;
        muteButton.sprite = muteSprite[bgm.mute ? 1 : 0];
    }

    public void Mute(AudioSource AS)
    {
        AS.mute = !AS.mute;
    }

   
    IEnumerator Play(float time)
    {
        isPlaying = true;
        yield return new WaitForSeconds(time);
        isPlaying = false;
    }
}
