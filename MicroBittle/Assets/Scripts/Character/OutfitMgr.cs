using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitMgr : MonoBehaviour
{
    public GameObject player;
    public static OutfitMgr Instance = null;
    public List<GameObject> divingSuits;
    public GameObject jackhammer;
    public GameObject headLight;
    public ObstacleType currentObstacleType = ObstacleType.None;
    [SerializeField]
    List<ObstacleType> allPossibleTypes = new List<ObstacleType>();

    void Start()
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Beatle");
        if(all.Length != 0)
        {
            Vector3 newPos = player.transform.position;
            newPos.y -= 0.5f;
            all[0].transform.position = newPos;
            all[0].transform.localScale = new Vector3(80f, 80f, 80f);
            Quaternion q = Quaternion.Euler(0f, 180f, 0f);
            all[0].transform.rotation = q; 
            all[0].transform.SetParent(player.transform);
        }
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
        else if(obstacleType == ObstacleType.Light)
        {
            headLight.SetActive(true);
        }
    }

    public void takeOffOutfit(ObstacleType obstacleType)
    {
        if (obstacleType == ObstacleType.Slider)
        {
            jackhammer.SetActive(false);
        }
        else if(obstacleType == ObstacleType.Light)
        {
            headLight.SetActive(false);
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
