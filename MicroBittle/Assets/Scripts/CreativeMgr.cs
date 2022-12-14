using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Biome
{
    desert,
    tundra,
    grassland,
    forest
}
public class CreativeMgr : MonoBehaviour
{
    public static CreativeMgr Instance = null;

    #region Parameter Setup
    public Biome curBiome = Biome.desert;
    public int curLayout = 0;
    public List<ParamManager.Obstacle> curObstacle = new List<ParamManager.Obstacle>();
    [SerializeField]
    GameObject setupPanel;
    [SerializeField]
    GameObject diyPanel;
    public Text mazenamelabel;
    [SerializeField]
    Text obstacleCntWarning;
    [SerializeField]
    List<Toggle> allBiomeSelection = new List<Toggle>();
    [SerializeField]
    List<Toggle> allLayoutSelection = new List<Toggle>();
    [SerializeField]
    List<Toggle> allObstacleSelection = new List<Toggle>();
    #endregion

    #region Init
    [SerializeField]
    List<GameObject> allLayouts = new List<GameObject>();
    [SerializeField]
    List<GameObject> allButtons = new List<GameObject>();
    #endregion

    #region Object Placement
    [SerializeField]
    int maxObstacle = 5;
    public int obstacleCnt = 0;
    [SerializeField]
    int maxGem = 5;
    public int gemCnt = 0;
    [SerializeField]
    GameObject gemPrefab;
    [SerializeField]
    Text obstacleText;
    [SerializeField]
    Text gemText;
    [SerializeField]
    Text warningText;
    float warningTimer = 0.0f;
    [SerializeField]
    GameObject toProgrammingButton;
    #endregion


    Dictionary<int, ObstacleType> realObstacleType = new Dictionary<int, ObstacleType>();
    List<Obstacle> allObstacles = new List<Obstacle>();
    public GameObject maze;


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
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        startPlacement();
        toProgrammingButton.SetActive(false);
        foreach(Toggle selection in allObstacleSelection)
        {
            if(selection)
            {
                ColorBlock cb = selection.colors;
                cb.normalColor = Color.gray;
                cb.selectedColor = Color.gray;
                selection.colors = cb;
            }
        }
    }
    #region Parameter setup
    public void setBiome(int i)
    {
        curBiome = (Biome)(i);
        for(int j = 0; j < allBiomeSelection.Count; j++)
        {
            ColorBlock cb = allBiomeSelection[j].colors;
            if (i == j)
            {
                cb.normalColor = Color.white;
                cb.selectedColor = Color.white;
            }
            else
            {
                cb.normalColor = Color.gray;
                cb.selectedColor = Color.gray;
            }
            allBiomeSelection[j].colors = cb;
        }
    }
    public void setLayout(int i)
    {
        curLayout = i;
        for (int j = 0; j < allLayoutSelection.Count; j++)
        {
            ColorBlock cb = allLayoutSelection[j].colors;
            if (i == j)
            {
                cb.normalColor = Color.white;
                cb.selectedColor = Color.white;
            }
            else
            {
                cb.normalColor = Color.gray;
                cb.selectedColor = Color.gray;
            }
            allLayoutSelection[j].colors = cb;
        }
    }

    public void setObstacle(int i)
    {
        ParamManager.Obstacle type = (ParamManager.Obstacle)i;
        if(curObstacle.Contains(type))
        {
            ColorBlock cb = allObstacleSelection[i].colors;
            cb.normalColor = Color.gray;
            cb.selectedColor = Color.gray;
            allObstacleSelection[i].colors = cb;
            curObstacle.Remove(type);
        }
        else if(curObstacle.Count < 3)
        {
            ColorBlock cb = allObstacleSelection[i].colors;
            cb.normalColor = Color.white;
            cb.selectedColor = Color.white;
            allObstacleSelection[i].colors = cb;
            curObstacle.Add(type);
        }
        else
        {
            UpdateWarning("Can only choose at most three obstacles", obstacleCntWarning);
        }
    }

    public void endSelection()
    {
        if (curObstacle.Count == 0)
        {
            return;
        }
        getLayout();
        // setAllMaterials();
        setupPanel.SetActive(false);
        diyPanel.SetActive(true);
        setObstacleChoice();
        //save maze name
        PlayerPrefs.SetString("mazename", mazenamelabel.text);
    }
    public void goodtogo()
    {
        updateCurObstacle();
        maze.SetActive(false);
        SceneManager.LoadScene("programplaymode");
    }
    #endregion

    #region Init
    private void setAllMaterials(Transform transform)
    {
        foreach (Transform child in transform)
        {
            MaterialController ctrl = child.gameObject.GetComponent<MaterialController>();
            if(ctrl)
            {
                ctrl.ChangeMaterial();
            }
            
        }
    }
    private void getLayout()
    {
        maze = Instantiate(allLayouts[curLayout]) as GameObject;
        DontDestroyOnLoad(maze);
    
        //setAllMaterials(newLayout.transform);
    }

    private void setObstacleChoice()
    {
        allButtons[2].SetActive(true);
        Vector3 pos = allButtons[2].transform.position;
        for(int i = 0; i < curObstacle.Count; i++)
        {
            pos.y -= 160.0f / 1080.0f * Screen.height;
            allButtons[(int)curObstacle[i]].SetActive(true);
            allButtons[(int)curObstacle[i]].transform.position = pos;
        }
    }
    #endregion

    #region Object placements
    void startPlacement()
    {
        obstacleText.text = "Obstacle(3 ~ 5): " + obstacleCnt;
        gemText.text = "Gem(5): " + gemCnt;
        
        setupPanel.SetActive(true);
        for(int i = 0; i < allButtons.Count; i++)
        {
            if(allButtons[i])
            {
                allButtons[i].SetActive(false);
            }
            
        }
        diyPanel.SetActive(false);
    }
    public bool canAddGem()
    {
        if (gemCnt < maxGem)
        {
            
            // 
            return true;
        }
        UpdateWarning("Cannot add more Gems", warningText);
        return false;
    }
    public void addGem(int num)
    {
        gemCnt += num;
        gemText.text = "Gem(5): " + gemCnt;
        ObstacleMgr.Instance.hasCreate = null;
        canShowProgramming();
    }

    public void addObstacle(int num)
    {
        obstacleCnt += num;
        if(obstacleText)
        {
            obstacleText.text = "Obstacle(3 ~ 5): " + obstacleCnt;
        }
        
        if(ObstacleMgr.Instance.hasCreate && num > 0)
        {
            allObstacles.Add(ObstacleMgr.Instance.hasCreate.GetComponent<Obstacle>());
        }
        ObstacleMgr.Instance.hasCreate = null;
        canShowProgramming();
    }

    private void updateCurObstacle()
    {
        curObstacle.Clear();
        foreach(Obstacle o in allObstacles)
        {
            ParamManager.Obstacle obs = ObstacleTypeToObstacle(o.obstacleType);
            if (!curObstacle.Contains(obs))
            {
                curObstacle.Add(obs);
            }
        }
        
        
    }

    public bool canAddObstacle()
    {
        if(obstacleCnt < maxObstacle)
        {
            // obstacleCnt++;
            return true;
        }
        UpdateWarning("Cannot add more obstacles", warningText);
        return false;
    }

    private void canShowProgramming()
    {
        if(gemCnt ==  5 && obstacleCnt >= 3)
        {
            toProgrammingButton.SetActive(true);
        }
        else
        {
            toProgrammingButton.SetActive(false);
        }
    }


    public void CreateGem()
    {
        if(!canAddGem())
        {
            return;
        }
        GameObject newObstacle = Instantiate(gemPrefab);
        //GameObject newObstacle = Instantiate(Resources.Load<P>("Prefabs/Obstacles/Test" + obstacleType.ToString() + "Obstacle"));
        Obstacle obstacle = newObstacle.AddComponent<Obstacle>();
        obstacle.isMovingWithMouse = true;
        obstacle.obstacleType = ObstacleType.None;
        obstacle.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void UpdateWarning(string warning, Text text)
    {
        warningTimer = 5.0f;
        if(text)
        {
            text.text = warning;
            StartCoroutine(warningCountdown(text));
        }
    }

    IEnumerator warningCountdown(Text text)
    {
        while(warningTimer > 0)
        {
            warningTimer -= Time.deltaTime;
            yield return null;
        }
        if (text)
        {
            text.text = "";
        }
        yield break;
    }
    #endregion


    public ObstacleType GetObstacleType(int o)
    {
        return realObstacleType[o];
    }
    // Update is called once per frame
    void Update()
    {
        if (ProgramUIMgr.Instance == null && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            LayerMask mask = 1 << 9 | 1 << 10;
            if (Physics.Raycast(ray, out hitInfo, 2000, ~mask))
            {
                Debug.Log(hitInfo.collider.tag);
                if (hitInfo.collider.tag == "Obstacle" || hitInfo.collider.tag == "Gem")
                {
                    float x = hitInfo.point.x;
                    float y = hitInfo.point.y;
                    GameObject hitObj = hitInfo.collider.gameObject;
                    if (hitInfo.collider.tag == "Obstacle")
                    {
                        Obstacle obs = hitObj.GetComponent<Obstacle>();
                        if (obs && obs.obstacleType != ObstacleType.Wall)
                        {
                            if(!obs.isMovingWithMouse)
                            {
                                DrawGrid.Instance.DeleteFromMaze(hitInfo.point, false);
                                allObstacles.Remove(obs);
                                addObstacle(-1);
                            }
                            
                        }
                    }
                    else if(hitInfo.collider.tag == "Gem")
                    {
                        addGem(-1);
                    }
                    Destroy(hitObj);
                }
            }
        }
    }

    public ParamManager.Obstacle ObstacleTypeToObstacle(ObstacleType o)
    {
        switch(o)
        {
            case ObstacleType.Slider:
                return ParamManager.Obstacle.wall;
            case ObstacleType.Knob:
                return ParamManager.Obstacle.rock;
                break;
            case ObstacleType.Vacuum:
                return ParamManager.Obstacle.spiderweb;
                break;
            case ObstacleType.Light:
                return ParamManager.Obstacle.mouse;
                break;
        }
        return ParamManager.Obstacle.wall;
    }

    
}
