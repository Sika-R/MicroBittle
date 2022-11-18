using System.Collections.Generic;
using UnityEngine;
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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            if (finalUI)
            {
                showFinalPanel();
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
        if(collections.ContainsKey("crystal"))
        {
            cnt = collections["crystal"];
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
}


