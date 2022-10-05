using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitMgr : MonoBehaviour
{
    public static OutfitMgr Instance = null;
    public List<GameObject> divingSuits;
    public GameObject jackhammer;
    [SerializeField] ObstacleType currentObstacleType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void chooseToolIcon(ObstacleType obstacleType)
    {
        if (currentObstacleType != obstacleType)
        {
            takeOffOutfit(currentObstacleType);
            currentObstacleType = obstacleType;
        }
        if (obstacleType == ObstacleType.Slider)
        {
            jackhammer.SetActive(true);
        }
    }

    public void takeOffOutfit(ObstacleType obstacleType)
    {
        if (obstacleType == ObstacleType.Slider)
        {
            jackhammer.SetActive(false);
        }
        else if (obstacleType == ObstacleType.Humid)
        { 
            foreach (var piece in divingSuits)
            {
                piece.SetActive(false);
            }
        }
    }

    public void PutOnDivingSuit(int index)
    {
        divingSuits[index].SetActive(true);
    }
}
