using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Collector : MonoBehaviour
{
    public bool attachCollectables = false;
    [SerializeField]
    Text gemText;

    Dictionary<string, int> collections = new Dictionary<string, int>();

    private void Start()
    {
        gemText.text = "        : 0 / 5";
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

        Debug.Log(collectable.name);
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
}


