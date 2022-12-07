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
    private List<AudioSource> allAS;
    private float ringClipLength = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        allAS = new List<AudioSource>();
        AudioSource[] asObject = (AudioSource[])Object.FindObjectsOfType(typeof(AudioSource));
        foreach (AudioSource asobject in asObject)
        {
            allAS.Add(asobject);
        }
        Debug.Log(PlayerPrefs.GetInt("IsMuted"));
        if (PlayerPrefs.GetInt("IsMuted") == 1) {
            MuteAllAs();
        }
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

    public void PlayDialogue(AudioClip clip)
    {
        if (audioSource == null)
        {
            return;
        }
        audioSource.Stop();
        audioSource.PlayOneShot(clip);
    }

    public void PlayDialogue(string clipName, AudioSource AS)
    {
        AS.PlayOneShot(dialogues.Find(x => x.name.Equals(clipName)));
    }

    public void PlayAudio(string clipName)
    {
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
        //bgm.mute = !bgm.mute;
        if (PlayerPrefs.GetInt("IsMuted") == 0){
            PlayerPrefs.SetInt("IsMuted", 1);
            Debug.Log(PlayerPrefs.GetInt("IsMuted"));
        } else{
            PlayerPrefs.SetInt("IsMuted", 0);
        }
        MuteAllAs();
    }

    public void MuteAllAs() {
        int isEnabledAllAudioSource = PlayerPrefs.GetInt("IsMuted");
        if(muteButton)
        {
            muteButton.sprite = muteSprite[isEnabledAllAudioSource];
        }
        foreach (AudioSource audioSource in allAS)
        {
            audioSource.mute = (isEnabledAllAudioSource == 1);
        }
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
