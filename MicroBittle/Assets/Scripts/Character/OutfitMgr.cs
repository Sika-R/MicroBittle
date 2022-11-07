using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitMgr : MonoBehaviour
{
    public static OutfitMgr Instance = null;
    public List<GameObject> divingSuits;
    public GameObject jackhammer;
    public ObstacleType currentObstacleType = ObstacleType.None;
    [SerializeField]
    List<ObstacleType> allPossibleTypes = new List<ObstacleType>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            ChangeOutfit(true);
            Debug.Log("Current outfit: " + currentObstacleType);
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
            PlayerMovement.Instance.canPass = obstacleType;
        }
        else if (obstacleType == ObstacleType.ButtonA)
        { 

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

    public void TakeOffDivingSuitByPiece(int index)
    {
        divingSuits[index].SetActive(false);
    }

    public void ChangeOutfit(bool isLeft)
    {
        int newOutfitIdx = allPossibleTypes.IndexOf(currentObstacleType) + (isLeft ? -1 : 1);
        int cnt = allPossibleTypes.Count;
        ObstacleType newType = allPossibleTypes[(newOutfitIdx + cnt) % cnt];
        chooseToolIcon(newType);
    }
}
