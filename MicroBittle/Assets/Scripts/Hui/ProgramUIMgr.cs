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
    List<GameObject> allLiveDemos = new List<GameObject>();
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
        InitObstacleDropdown(obstacleTypeDropdown);
        InitObstacleDropdown(liveDemoDropdown);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void InitObstacleDropdown(Dropdown dropdown)
    {
        dropdown.ClearOptions();
        Dropdown.OptionData newData;
        for(int i = 0; i < allObstacles.Count; i++)
        {
            newData = new Dropdown.OptionData();
            newData.text = Enum.GetName(typeof(ParamManager.Obstacle), allObstacles[i]);
            dropdown.options.Add(newData);
        }
        SwitchBlockCodingPanel();
    }

    public void SwitchBlockCodingPanel()
    {
        for(int i = 0; i < allCodingBlocks.Count; i++)
        {
            if((int)allObstacles[obstacleTypeDropdown.value] == i)
            {
                allCodingBlocks[i].SetActive(true);
            }
            else
            {
                if(allCodingBlocks[i])
                {
                    allCodingBlocks[i].SetActive(false);
                }
                
            }
        }
    }

    public void SwitchLiveDemoPanel()
    {
        for (int i = 0; i < allCodingBlocks.Count; i++)
        {
            if ((int)allObstacles[obstacleTypeDropdown.value] == i)
            {
                allLiveDemos[i].SetActive(true);
            }
            else
            {
                if (allLiveDemos[i])
                {
                    allLiveDemos[i].SetActive(false);
                }

            }
        }
    }
}
