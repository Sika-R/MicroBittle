using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Collector : MonoBehaviour
{
    public bool attachCollectables = false;
    [SerializeField]
    Text gemText;

    [SerializeField]
    GameObject inGameUI;
    [SerializeField]
    GameObject finalUI;
    [SerializeField]
    List<Sprite> badgeSprites = new List<Sprite>();
    [SerializeField]
    Image finalBadge;
    [SerializeField]
    Text finalCnt;
    [SerializeField]
    GameObject DialogueUI;

    Dictionary<string, int> collections = new Dictionary<string, int>();

    private void Start()
    {
        gemText.text = "        : 0 / 5";
        if(inGameUI)
        {
            inGameUI.SetActive(true);
        }
        if(finalUI)
        {
            finalUI.SetActive(false);
        }
        if(DialogueUI)
        {
            DialogueUI.SetActive(true);
            Invoke("UnactiveDialogue", 0.16f);
        }


    }
    private void UnactiveDialogue()
    {
        DialogueUI.SetActive(false);
    }
    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            if (DialogueUI)
            {
                DialogueUI.SetActive(true);
                DialogueController.Instance.DoInteraction();
            }
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            if (finalUI)
            {
                GetComponent<PlayerMovement>().PlayerFreeze();
                if(DialogueUI)
                {
                    DialogueUI.SetActive(true);
                    DialogueController.Instance.DoInteraction();
                }
                
                //showFinalPanel();

                string mazeSelection = PlayerPrefs.GetString("mazeselection");
                if (PlayerPrefs.GetString("mode") == "storymode")
                {
                    StartCoroutine(doInteraction());
                }
                else if (mazeSelection == "DesertPyramid" || mazeSelection == "TundraCave" || mazeSelection == "GrassLand")
                {
                    StartCoroutine(goBackToHomePage());
                }
                else
                {
                    StartCoroutine(goBackToHomePage());
                }
            }
            
        }
    }
    public virtual void OnCollect(Collectable collectable)
    {
        if (attachCollectables)
            collectable.transform.parent = transform;
        var count = 0;
        if (collections.TryGetValue(collectable.name, out count))
            collections[collectable.name] = count + 1;
        else
            collections[collectable.name] = 1;

        if(collectable.name.Contains("crystal"))
        {
            gemText.text = "        : " + collections[collectable.name] + " / 5";
        }
    }

    public void PlayerPlayAudio(AudioClip onCollectAudio)
    {
        var audio = GetComponent<AudioSource>();
        if (audio) audio.PlayOneShot(onCollectAudio);
    }

    public bool HasCollectable(string name)
    {
        return collections.ContainsKey(name);
    }

    public bool HasCollectableQuantity(string name, int requiredCount)
    {
        int count;
        if (collections.TryGetValue(name, out count))
            return count >= requiredCount;
        return false;
    }


    public void showFinalPanel()
    {
        inGameUI.SetActive(false);
        finalUI.SetActive(true);
        float cnt = 0;
        /*if(collections.ContainsKey("crystal"))
        {
            cnt = collections["crystal"];
        }*/
        List<string> keyList = new List<string>(this.collections.Keys);

        if(keyList.Count > 0)
        {
            cnt = collections[keyList[0]];
        }
        else
        {
            cnt = 0;
        }
        
        finalCnt.text = cnt.ToString();
        if(cnt == 5)
        {
            finalBadge.sprite = badgeSprites[0];
        }
        else
        {
            finalBadge.sprite = badgeSprites[1];
        }
    }

    IEnumerator doInteraction()
    {
        yield return new WaitForSeconds(3);
        // StartCoroutine(goBackToHomePage());
        showFinalPanel();
    }

    IEnumerator goBackToHomePage()
    {
        showFinalPanel();
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Start");
        if(ParamManager.Instance)
        {
            Destroy(ParamManager.Instance.gameObject);
        }

        if (ParamManager.Instance)
        {
            Destroy(ParamManager.Instance.gameObject);
        }

        if (ParamManager.Instance)
        {
            Destroy(ParamManager.Instance.gameObject);
        }

        if(CreativeMgr.Instance)
        {
            Destroy(CreativeMgr.Instance.maze);
            Destroy(CreativeMgr.Instance.gameObject);
        }
        if(Photoresistor.Instance)
        {
            Photoresistor.Instance.LightOn();
        }

        GameObject[] all = GameObject.FindGameObjectsWithTag("Beatle");
        for(int i = 0; i < all.Length; i++)
        {
            Destroy(all[i]);
        }
    }
}


