using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CreativeMgr : MonoBehaviour
{
    public enum Biome
    { 
        desert,
        tundra,
        grassland
    }

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
        startPlacement();
        toProgrammingButton.SetActive(false);
    }
    #region Parameter setup
    public void setBiome(int i)
    {
        curBiome = (Biome)(i);
    }
    public void setLayout(int i)
    {
        curLayout = i;
    }

    public void setObstacle(int i)
    {
        ParamManager.Obstacle type = (ParamManager.Obstacle)i;
        if(curObstacle.Contains(type))
        {
            curObstacle.Remove(type);
        }
        else
        {
            curObstacle.Add(type);
        }
    }

    public void endSelection()
    {
        if (curObstacle.Count == 0)
        {
            return;
        }
        getLayout();
        setAllMaterials();
        setupPanel.SetActive(false);
        diyPanel.SetActive(true);
        setObstacleChoice();
        //save maze name
        PlayerPrefs.SetString("mazename", mazenamelabel.text);
    }
    public void goodtogo()
    {
        SceneManager.LoadScene("programplaymode");
    }
    #endregion

    #region Init
    private void setAllMaterials()
    {

    }
    private void getLayout()
    {
        GameObject newLayout = Instantiate(allLayouts[curLayout]) as GameObject;
    }

    private void setObstacleChoice()
    {
        allButtons[2].SetActive(true);
        Vector3 pos = allButtons[2].transform.position;
        for(int i = 0; i < curObstacle.Count; i++)
        {
            pos.y -= 160;
            allButtons[(int)curObstacle[i]].SetActive(true);
            allButtons[(int)curObstacle[i]].transform.position = pos;
        }
    }
    #endregion

    #region Object placements
    void startPlacement()
    {
        obstacleText.text = "Obstacle: " + obstacleCnt;
        gemText.text = "Gem: " + gemCnt;
        
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
        UpdateWarning("Cannot add more Gems");
        return false;
    }
    public void addGem(int num)
    {
        gemCnt += num;
        gemText.text = "Gem: " + gemCnt;
        canShowProgramming();
    }

    public void addObstacle(int num)
    {
        obstacleCnt += num;
        obstacleText.text = "Obstacle: " + obstacleCnt;
        canShowProgramming();
    }

    public bool canAddObstacle()
    {
        if(obstacleCnt < maxObstacle)
        {
            // obstacleCnt++;
            return true;
        }
        UpdateWarning("Cannot add more obstacles");
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

    public void UpdateWarning(string warning)
    {
        warningTimer = 5.0f;
        warningText.text = warning;
        StartCoroutine(warningCountdown());
        
    }

    IEnumerator warningCountdown()
    {
        while(warningTimer > 0)
        {
            warningTimer -= Time.deltaTime;
            yield return null;
        }
        warningText.text = "";
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
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            LayerMask mask = 1 << 9 | 1 << 10;
            if (Physics.Raycast(ray, out hitInfo, 2000, ~mask))
            {
                Debug.Log(hitInfo.collider.tag);
                if (hitInfo.collider.tag == "Cube" || hitInfo.collider.tag == "Gem")
                {
                    float x = hitInfo.point.x;
                    float y = hitInfo.point.y;
                    GameObject hitObj = hitInfo.collider.gameObject;
                    if (hitInfo.collider.tag == "Cube")
                    {
                        Obstacle obs = hitObj.GetComponent<Obstacle>();
                        if (obs && obs.obstacleType != ObstacleType.Wall)
                        {
                            if(!obs.isMovingWithMouse)
                            {
                                DrawGrid.Instance.DeleteFromMaze(hitInfo.point, false);
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
}
