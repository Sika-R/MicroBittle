using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgramUIMgr : MonoBehaviour
{
    public static ProgramUIMgr Instance = null;
    public List<ParamManager.Obstacle> allObstacles = new List<ParamManager.Obstacle>();
    [SerializeField]
    Dropdown obstacleTypeDropdown;
    [SerializeField]
    Dropdown liveDemoDropdown;
    [SerializeField]
    List<GameObject> allCodingBlocks = new List<GameObject>();
    [SerializeField]
    List<GameObject> allSensorImages = new List<GameObject>();
    [SerializeField]
    List<GameObject> allLiveDemos = new List<GameObject>();
    GameObject curLiveDemo = null;
    Dictionary<ParamManager.Obstacle, ObstacleType> obstacleMap = new Dictionary<ParamManager.Obstacle, ObstacleType>();
    public int successCnt = 0;
    [SerializeField]
    GameObject nextButton;
    public String nextSceneName = " ";
    [SerializeField]
    GameObject warningText;

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

    // Start is called before the first frame update
    void Start()
    {
        InitObstacleType();
        InitObstacleDropdown(obstacleTypeDropdown);
        InitObstacleDropdown(liveDemoDropdown);
        obstacleMap.Add(ParamManager.Obstacle.mouse, ObstacleType.Light);
        obstacleMap.Add(ParamManager.Obstacle.spiderweb, ObstacleType.Vacuum);
        obstacleMap.Add(ParamManager.Obstacle.rock, ObstacleType.Knob);
        obstacleMap.Add(ParamManager.Obstacle.wall, ObstacleType.Slider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitObstacleType()
    {
        if(CreativeMgr.Instance)
        {
            allObstacles.Clear();
            allObstacles = CreativeMgr.Instance.curObstacle;
            return;
        }
        String mazeName = PlayerPrefs.GetString("mazeselection");
        if(mazeName == "DesertPyramid")
        {
            allObstacles.Clear();
            allObstacles.Add(ParamManager.Obstacle.mouse);
            allObstacles.Add(ParamManager.Obstacle.spiderweb);
        }
        else if(mazeName == "TundraCave")
        {
            allObstacles.Clear();
            allObstacles.Add(ParamManager.Obstacle.wall);
            allObstacles.Add(ParamManager.Obstacle.spiderweb);
        }
        else if (mazeName == "ForestCavern")
        {
            allObstacles.Clear();
            allObstacles.Add(ParamManager.Obstacle.mouse);
            allObstacles.Add(ParamManager.Obstacle.rock);
        }
        else if(mazeName == "GrassLand")
        {
            allObstacles.Clear();
            allObstacles.Add(ParamManager.Obstacle.mouse);
            allObstacles.Add(ParamManager.Obstacle.rock);
        }
    }

    void InitObstacleDropdown(Dropdown dropdown)
    {
        dropdown.ClearOptions();
        Dropdown.OptionData newData;
        for (int i = 0; i < allObstacles.Count; i++)
        {
            newData = new Dropdown.OptionData();
            newData.text = ObstacleToFunctionName(allObstacles[i]);
            dropdown.options.Add(newData);
        }

        InitAllBlockCodingController();
        SwitchBlockCodingPanel();
    }

    public string ObstacleToFunctionName(ParamManager.Obstacle o)
    {
        if(o == ParamManager.Obstacle.mouse)
        {
            return "Headlamp";
        }
        if (o == ParamManager.Obstacle.wall)
        {
            return "Power Log";
        }
        if (o == ParamManager.Obstacle.rock)
        {
            return "JackHammer";
        }
        if (o == ParamManager.Obstacle.spiderweb)
        {
            return "Vacuum Cleaner";
        }
        return "";
    }

    private void InitAllBlockCodingController()
    {
        for (int i = 0; i < allCodingBlocks.Count; i++)
        {
            if (allCodingBlocks[i] && allObstacles.Contains((ParamManager.Obstacle)i))
            {
                ParamController ctrl = allCodingBlocks[i].GetComponent<ParamController>();
                ctrl.SetObstacleOptions(allObstacles);
                ParamManager.Instance.AddController(ctrl);
            }
        }
        
    }

    public void SwitchBlockCodingPanel()
    {
        for(int i = 0; i < allCodingBlocks.Count; i++)
        {
            if((int)allObstacles[obstacleTypeDropdown.value] == i)
            {
                allCodingBlocks[i].SetActive(true);
                allSensorImages[i].SetActive(true);
                /*if(allCodingBlocks[i].activeInHierarchy)
                {
                    allCodingBlocks[i].GetComponent<ParamController>().DelegationInit();
                }*/

            }
            else
            {
                if(allCodingBlocks[i])
                {
                    allCodingBlocks[i].SetActive(false);
                    allSensorImages[i].SetActive(false);
                }
                
            }
        }
    }
    
    public void InitLiveDemo()
    {
        SwitchLiveDemoPanel();
    }

    public void DisableLiveDemo()
    {
        if (curLiveDemo)
        {
            curLiveDemo.SetActive(false);
        }
        
    }

    public void SwitchLiveDemoPanel()
    {
        if(curLiveDemo)
        {
            curLiveDemo.SetActive(false);
        }
        
        for (int i = 0; i < allCodingBlocks.Count; i++)
        {
            if ((int)allObstacles[liveDemoDropdown.value] == i)
            {
                if(ParamManager.Instance.paramValidationCheck(allObstacles[liveDemoDropdown.value]))
                {
                    warningText.SetActive(false);
                    allLiveDemos[i].SetActive(true);
                    curLiveDemo = allLiveDemos[i];
                    Obstacle obstacle = curLiveDemo.GetComponentInChildren<Obstacle>();
                    if (obstacle)
                    {
                        obstacle.TryInit();
                    }
                    OutfitMgr.Instance.allPossibleTypes.Clear();
                    OutfitMgr.Instance.allPossibleTypes.Add(obstacleMap[allObstacles[liveDemoDropdown.value]]);
                    OutfitMgr.Instance.ChangeOutfit(true);
                    OutfitMgr.Instance.ChangeOutfit(false);
                }
                else
                {
                    warningText.SetActive(true);
                }
                
            }
            /*else
            {
                if (allLiveDemos[i])
                {
                    allLiveDemos[i].SetActive(false);
                }
            }*/
        }
    }

    public void AddSuccess()
    {
        programUI.Instance.breakornot = true;
        successCnt++;
        if(successCnt >= allObstacles.Count)
        {
            if(!ParamManager.Instance.allParamValidationCheck())
            {
                nextButton.SetActive(false);
                warningText.SetActive(true);
                return;
            }
            warningText.SetActive(false);
            nextButton.SetActive(true);
            programUI.Instance.changeDialogueForProgramming();
            if (PlayerPrefs.GetString("mode") == "creativemode")
            {
                nextButton.GetComponent<Button>().onClick.AddListener(() => programUI.Instance.movetospecificScene("customize"));
            }
            else
            {
                nextButton.GetComponent<Button>().onClick.AddListener(() => programUI.Instance.movetospecificScene(PlayerPrefs.GetString("mazeselection")));
            }
            
            // 
        }
    }
}
