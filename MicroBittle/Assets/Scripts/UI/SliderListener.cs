using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderListener : MonoBehaviour
{
    [SerializeField] ObstacleType obstacleType;
    Slider mainSlider;
    // Start is called before the first frame update
    void Start()
    {
        mainSlider = this.gameObject.GetComponent<Slider>();
        mainSlider.onValueChanged.AddListener(delegate { ObstacleMgr.Instance.getInput(mainSlider.value, obstacleType); });
    }

    // Update is called once per frame
    void Update()

    {
        
    }
}
