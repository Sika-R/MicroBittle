using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitMgr : MonoBehaviour
{
    public bool isProgram = false;
    public GameObject player;
    public static OutfitMgr Instance = null;
    public List<GameObject> divingSuits;
    public GameObject jackhammer;
    public GameObject powerlog;
    public GameObject headLight;
    public GameObject vacuum;
    public ObstacleType currentObstacleType = ObstacleType.None;
    [SerializeField]
    public List<ObstacleType> allPossibleTypes = new List<ObstacleType>();

    Vector3 logPos;

    void Start()
    {
        if(!isProgram)
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
        if(powerlog)
        {
            logPos = powerlog.transform.localPosition;
        }
        
        ChangeOutfit(true);
        ChangeOutfit(false);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.B))
        {
            ChangeOutfit(true);
            Debug.Log("Current outfit: " + currentObstacleType);
        }
#endif
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
            powerlog.SetActive(true);
            // jackhammer.SetActive(true);
            // PlayerMovement.Instance.canPass = obstacleType;
        }
        else if (obstacleType == ObstacleType.ButtonA)
        { 
        }
        else if(obstacleType == ObstacleType.Light)
        {
            if(ProgramUIMgr.Instance)
            {
                headLight.SetActive(true);
            }
            //headLight.SetActive(true);
        }
        else if(obstacleType == ObstacleType.Vacuum)
        {
            vacuum.SetActive(true);
        }
        else if(obstacleType == ObstacleType.Knob)
        {
            jackhammer.SetActive(true);
        }
    }

    public void takeOffOutfit(ObstacleType obstacleType)
    {
        if (obstacleType == ObstacleType.Slider)
        {
            powerlog.SetActive(false);
            //jackhammer.SetActive(false);
        }
        else if(obstacleType == ObstacleType.Light)
        {
            if (ProgramUIMgr.Instance)
            {
                headLight.SetActive(false);
            }

            //
        }
        else if (obstacleType == ObstacleType.Humid)
        { 
            foreach (var piece in divingSuits)
            {
                piece.SetActive(false);
            }
        }
        else if(obstacleType == ObstacleType.Vacuum)
        {
            vacuum.SetActive(false);
        }
        else if(obstacleType == ObstacleType.Knob)
        {
            jackhammer.SetActive(false);
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
        if (allPossibleTypes.Count == 0) return;
        // SoundMgr.Instance.StopAudio();
        int newOutfitIdx = allPossibleTypes.IndexOf(currentObstacleType) + (isLeft ? -1 : 1);
        int cnt = allPossibleTypes.Count;
        ObstacleType newType = allPossibleTypes[(newOutfitIdx + cnt) % cnt];
        chooseToolIcon(newType);
        
    }


    public void ControlJackhammer(int index)
    {
        Vector3 currentLocalPos = jackhammer.transform.localPosition;
        if (index == 0)
        {
            jackhammer.transform.localPosition = new Vector3(0.6f, currentLocalPos.y, currentLocalPos.z);
        }
        else if (index == 1)
        {
            jackhammer.transform.localPosition = new Vector3(0f, currentLocalPos.y, currentLocalPos.z);
        }
        else
        {
            jackhammer.transform.localPosition = new Vector3(-0.6f, currentLocalPos.y, currentLocalPos.z);
        }
    }

    public void getInput(float inputVal, ObstacleType obstacleType)
    {
        if(obstacleType == currentObstacleType)
        {
            if(currentObstacleType == ObstacleType.Slider)
            {
                MovePowerlog(inputVal);
            }
        }
    }

    private void MovePowerlog(float value)
    {
        Vector3 newPos = logPos;
        newPos.z += value / 2000;
        powerlog.transform.localPosition = newPos;
    }

}
