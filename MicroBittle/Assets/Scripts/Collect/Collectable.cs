using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
    new public Collider collider;
    public LayerMask layers;
    public GameObject collectEffect;
    public AudioClip onCollectAudio;
    public bool disableOnCollect = false;


    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }
    void Reset()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (CanCollect(other))
            Collect(other);
    }

    protected virtual void Collect(Collider other)
    {
        if (collectEffect)
        {
            Transform effectTransform = collectEffect.transform;
            Vector3 localScale = effectTransform.localScale;

            collectEffect.transform.SetParent(null);
            effectTransform.localScale = localScale;
            collectEffect.SetActive(true);
        }
        var collector = other.GetComponent<Collector>();
        if (onCollectAudio)
        {
            /*var audio = GetComponent<AudioSource>();
            if (audio) audio.PlayOneShot(onCollectAudio);*/
            if(collector)
            {
                collector.PlayerPlayAudio(onCollectAudio);
            }
        }
        
        if (collector)
            collector.OnCollect(this);
        if (disableOnCollect)
            gameObject.SetActive(false);
    }

    bool CanCollect(Collider other)
    {
        return 0 != (layers.value & 1 << other.gameObject.layer);
    }

}


