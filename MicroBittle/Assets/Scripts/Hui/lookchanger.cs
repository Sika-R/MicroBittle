using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class lookchanger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject debug;
    [Header("Mesh to change")]
    public MeshFilter meshtochange;
    public GameObject meshchange;
    private int currentOptionForMesh = 1;
    [Header("Mesh List")]
    public List<Mesh> meshlist = new List<Mesh>();

    [Header("Texture change")]
    public MeshRenderer materialchange;
    private int currentOptionForTexture = -1;
    [Header("Texture List")]
    public List<Texture> texturelist = new List<Texture>();

    [Header("Base Color change")]
    public List<Color> colorList = new List<Color>();
    private int currentcolor = 0;

    [Header("GameObject List")]
    public List<GameObject> gameObjectList = new List<GameObject>();
    public List<GameObject> gameObjectListtosave = new List<GameObject>();
    private int characterNum = 3;
    public GameObject objecttoTransfrom;
    public Material newmaterial;
    public GameObject panelTexture;
    public GameObject panelShape;
    public GameObject panelColor;
    public int curUI = 1;

    public GameObject namepanel;
    public GameObject afternamepanel;
    public Text namelabel;
    public Text namedtext;

    public GameObject mazeselectionpanel;

    [SerializeField]
    NarrationLine line;
    [SerializeField]
    DialogueNode node;
    [SerializeField]
    AudioClip clip;

    void Start()
    {
       // meshtochange = meshchange.GetComponent<MeshFilter>();
       // meshtochange.mesh = meshlist[0];
        materialchange = gameObjectList[1].GetComponent<MeshRenderer>();
        InitIntro();

        //gameObjectList[1].GetComponent<MeshRenderer>().material = newmaterial;// load the saved material
        //materialchange.material.SetTexture("_MainTex", texturelist[0]);
        //materialchange.material.SetColor("_Color", colorList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    public void nextOptionForMesh()
    {
        currentOptionForMesh++;
        if(currentOptionForMesh >= meshlist.Count)
        {
            currentOptionForMesh = 0;
        }
        meshtochange.mesh = meshlist[currentOptionForMesh];
    }
    public void previousOptionForMesh()
    {
        currentOptionForMesh--;
        if (currentOptionForMesh < 0)
        {
            currentOptionForMesh = meshlist.Count - 1;
        }
        meshtochange.mesh = meshlist[currentOptionForMesh];
    }*/

    public void nextOptionForMesh()
    {
        currentOptionForMesh++;
        currentcolor = 0;
        currentOptionForTexture = 0;
        if (currentOptionForMesh >= 2)
        {
            currentOptionForMesh = 2;
        }
        switch (currentOptionForMesh)
        {
            case 0:
                objecttoTransfrom.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -180, 0), 0.4f);
                materialchange = gameObjectList[currentOptionForMesh].GetComponent<MeshRenderer>();
                newmaterial.SetTexture("_MainTex", texturelist[currentOptionForTexture]);
                newmaterial.SetTexture("_MainTex", materialchange.material.GetTexture("_MainTex"));
                newmaterial.SetColor("_BaseColor", materialchange.material.GetColor("_BaseColor"));

                break;
            case 1:
                objecttoTransfrom.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.4f);
                materialchange = gameObjectList[currentOptionForMesh].GetComponent<MeshRenderer>();
                newmaterial.SetTexture("_MainTex", texturelist[currentOptionForTexture]);
                newmaterial.SetTexture("_MainTex", materialchange.material.GetTexture("_MainTex"));
                newmaterial.SetColor("_BaseColor", materialchange.material.GetColor("_BaseColor"));

                break;
            case 2:
                objecttoTransfrom.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), 0.4f);
                materialchange = gameObjectList[currentOptionForMesh].GetComponent<MeshRenderer>();
                newmaterial.SetTexture("_MainTex", texturelist[currentOptionForTexture]);
                newmaterial.SetTexture("_MainTex", materialchange.material.GetTexture("_MainTex"));
                newmaterial.SetColor("_BaseColor", materialchange.material.GetColor("_BaseColor"));

                break;
        }
        
    }
    public void previousOptionForMesh()
    {
        currentOptionForMesh--;
        currentcolor = 0;
        currentOptionForTexture = 0;
        if (currentOptionForMesh <= 0)
        {
            currentOptionForMesh = 0;
        }
        switch (currentOptionForMesh)
        {
            case 0:
                objecttoTransfrom.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -180, 0), 0.4f);
                materialchange = gameObjectList[currentOptionForMesh].GetComponent<MeshRenderer>();
                newmaterial.SetTexture("_BaseMap", materialchange.material.GetTexture("_BaseMap"));
                newmaterial.SetColor("_BaseColor", materialchange.material.GetColor("_BaseColor"));

                break;
            case 1:
                objecttoTransfrom.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.4f);
                materialchange = gameObjectList[currentOptionForMesh].GetComponent<MeshRenderer>();
                newmaterial.SetTexture("_BaseMap", materialchange.material.GetTexture("_BaseMap"));
                newmaterial.SetColor("_BaseColor", materialchange.material.GetColor("_BaseColor"));

                break;
            case 2:
                objecttoTransfrom.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), 0.4f);
                materialchange = gameObjectList[currentOptionForMesh].GetComponent<MeshRenderer>();
                newmaterial.SetTexture("_BaseMap", materialchange.material.GetTexture("_BaseMap"));
                newmaterial.SetColor("_BaseColor", materialchange.material.GetColor("_BaseColor"));

                break;
        }
    }
    public void nextOptionForTexture()
    {
        currentOptionForTexture++;
        if(currentOptionForTexture >= texturelist.Count)
        {
            currentOptionForTexture = 0;
        }
        materialchange.material.SetTexture("_BaseMap", texturelist[currentOptionForTexture]);
        newmaterial.SetTexture("_BaseMap", materialchange.material.GetTexture("_BaseMap"));
        newmaterial.SetColor("_BaseColor", materialchange.material.GetColor("_BaseColor"));
    }
    public void preOptionForTexture()
    {
        currentOptionForTexture--;
        if (currentOptionForTexture < 0)
        {
            currentOptionForTexture = texturelist.Count - 1;
        }
        materialchange.material.SetTexture("_BaseMap", texturelist[currentOptionForTexture]);
        newmaterial.SetTexture("_BaseMap", materialchange.material.GetTexture("_BaseMap"));
        newmaterial.SetColor("_BaseColor", materialchange.material.GetColor("_BaseColor"));
    }
    /*
    public void nextOptionForColor()
    {
        currentcolor++;
        if(currentcolor >= colorList.Count)
        {
            currentcolor = 0;
        }
        materialchange.material.SetColor("_Color",colorList[currentcolor]);
        newmaterial.SetTexture("_MainTex", materialchange.material.GetTexture("_MainTex"));
        newmaterial.SetColor("_Color", materialchange.material.GetColor("_Color"));
    }

    public void preOptionForColor()
    {
        currentcolor--;
        if (currentcolor < 0)
        {
            currentcolor = colorList.Count - 1;
        }
        materialchange.material.SetColor("_Color", colorList[currentcolor]);
        newmaterial.SetTexture("_MainTex", materialchange.material.GetTexture("_MainTex"));
        newmaterial.SetColor("_Color", materialchange.material.GetColor("_Color"));
    }*/
    public void UIchangerShape()
    {
        curUI = 1;
        panelTexture.SetActive(false);
        panelShape.SetActive(true);
        panelColor.SetActive(false);
        foreach(GameObject eachone in gameObjectList){
            eachone.SetActive(true);
        }
    }
    public void UIchangerTexture()
    {
        curUI = 0;
        panelTexture.SetActive(true);
        panelShape.SetActive(false);
        panelColor.SetActive(false);
        foreach (GameObject eachone in gameObjectList)
        {
            if(eachone == gameObjectList[currentOptionForMesh])
                eachone.SetActive(true);
            else
            {
                eachone.SetActive(false);
            }
        }
    }
    public void UIchangerColor()
    {
        curUI = 2;
        panelTexture.SetActive(false);
        panelShape.SetActive(false);
        panelColor.SetActive(true);
        foreach (GameObject eachone in gameObjectList)
        {
            if (eachone == gameObjectList[currentOptionForMesh])
                eachone.SetActive(true);
            else
            {
                eachone.SetActive(false);
            }
        }
    }
    public void UIcolorpicker(GameObject buttonClick)
    {
        Color colrochange = buttonClick.GetComponent<Image>().color;
        materialchange.GetComponent<Renderer>().material.SetColor("_BaseColor", colrochange);
        Debug.Log(colrochange);
        Debug.Log(currentOptionForMesh);
        //newmaterial.SetTexture("_BaseMap", materialchange.material.GetTexture("_BaseMap"));
        //newmaterial.SetColor("_Color", materialchange.material.GetColor("_Color"));
        //gameObjectList[currentOptionForMesh].SetTexture("_BaseMap", materialchange.material.GetTexture("_BaseMap"));
        // gameObjectList[currentOptionForMesh].SetColor("_Color", materialchange.material.GetColor("_Color"));
    }
    public void UItexturepicker(GameObject buttonClick)
    {
        Texture texturechange = buttonClick.GetComponent<Image>().sprite.texture;
        materialchange.material.SetTexture("_BaseMap", texturechange);
        newmaterial.SetTexture("_BaseMap", materialchange.material.GetTexture("_BaseMap"));
        newmaterial.SetColor("_BaseColor", materialchange.material.GetColor("_BaseColor"));
    }
    public void saveasprefab()
    {
        GameObject character = gameObjectList[currentOptionForMesh];
        character.transform.localScale = new Vector3(0f, 0f, 0f);
        character.transform.SetParent(null);
        DontDestroyOnLoad(character);
        // PrefabUtility.SaveAsPrefabAsset(gameObjectListtosave[currentOptionForMesh],"Assets/Player.prefab");
        Debug.Log(currentOptionForMesh);
        //AssetDatabase.CreateAsset(newmaterial, "Assets/Player.mat");
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
        
        if(PlayerPrefs.GetString("mode") == "storymode")
        {
            //story mode
            Debug.Log("mode is " + PlayerPrefs.GetString("mode"));
            SceneManager.LoadScene("programstorymodenew");
        }
        else if(PlayerPrefs.GetString("mode") == "playmode")
        {
            Debug.Log("mode is " + PlayerPrefs.GetString("mode"));
            mazeselectionpanel.SetActive(true);
        }
        else if (PlayerPrefs.GetString("mode") == "creativemode")
        {
            Debug.Log("mode is " + PlayerPrefs.GetString("mode"));
            SceneManager.LoadScene("EmptyCreativeScene");//switch to maze gameplay
            // PlayerPrefs.SetString("mazeselection", "EmptyCreativeScene");
        }
        
        //SceneManager.LoadScene("test");
    }
    public void mazeselectionclicked(int i)
    {
        if( i == 0)
        {
            //desert clicked
            PlayerPrefs.SetString("mazeselection", "DesertPyramid");
            //SceneManager.LoadScene("DesertPyramid");
            SceneManager.LoadScene("programplaymode");
            
        }
        else if(i == 1)
        {
            //tundra clicked
            PlayerPrefs.SetString("mazeselection", "TundraCave");
            //SceneManager.LoadScene("TundraCave");
            SceneManager.LoadScene("programplaymode");
        }
        else if(i == 2)
        {
            //grassland clicked
            // PlayerPrefs.SetString("mazeselection", "ForestCavern");
            PlayerPrefs.SetString("mazeselection", "GrassLand");
            SceneManager.LoadScene("programplaymode");
        }
    }

    public void namebuttonClicked()
    {
        namelabel.text = namedtext.text;
        PlayerPrefs.SetString("name", namelabel.text);
        Debug.Log("name is " + PlayerPrefs.GetString("name"));
        
        namepanel.SetActive(false);
        afternamepanel.SetActive(true);
        if(PlayerPrefs.GetString("mode") == "storymode")
        {
            DialogueControllerCustomize.Instance_.AfterTypeName();
        }
        // 
    }
    public void changetoWiring()
    {
        GameObject character = gameObjectList[currentOptionForMesh];
        character.transform.localScale = new Vector3(0f, 0f, 0f);
        character.transform.SetParent(null);
        DontDestroyOnLoad(character);
        SceneManager.LoadScene("program");
    }

    private void InitIntro()
    {
        string mode = PlayerPrefs.GetString("mode");
        if(mode == "creativemode")
        {
            line.m_Text = "Here?s a chance to name and design your micro bittle (or to invite another player to do so)!";
            node.dialogueAudio = null;
        }
        else if(mode == "playmode")
        {
            line.m_Text = "Here?s a chance to name and design (or rename and redesign) your micro bittle!";
            node.dialogueAudio = null;
        }
        else
        {
            line.m_Text = "Enter your micro bittle name and click on the check mark when you?re done!";
            node.dialogueAudio = clip;
        }
    }

}
