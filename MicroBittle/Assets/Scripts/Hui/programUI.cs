using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class programUI : MonoBehaviour
    //, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private static programUI _instance;
    public static programUI Instance { get { return _instance; } }

    //=============Wiring==============//
    
    public GameObject panelCompo;
    public GameObject panelCompo1;
    public GameObject panelCompo2;
    public GameObject panelCompo3;
    public GameObject panelCompo4;
    public GameObject panelJack;
    public GameObject paneldive;
    public GameObject panelHead;
    public GameObject buttonCompo;
    public GameObject buttonJack;
    public GameObject buttondive;
    public GameObject buttonHead;
    public GameObject buttonCompo1;
    public GameObject buttonCompo2;
    public GameObject buttonCompo3;
    public GameObject buttonCompo4;

    public GameObject Microbitintro;
    public GameObject Expansionintro;
    public GameObject Jumpintro;
    public GameObject Sliderintro;
    public GameObject Photointro;
    public GameObject Waterintro;
    public GameObject jackintro1;
    public GameObject jackintro2;
    public GameObject jackintro3;

    public GameObject sliderJack;
    public GameObject sliderdiv;
    public GameObject sliderHead;

    public GameObject buttonCheckWiring;
    public GameObject PanelForWiring;
    public GameObject PanelForprogramming;

    private bool[] sliderJackiftrue = new bool[2];
    private bool[] sliderdivingiftrue = new bool[2];
    private bool[] sliderheadiftrue = new bool[2];
    private enum panelstage { Compo, Compo1, Compo2, Compo3, Compo4,GetData,Jack, Dive,Head,ViewCode,ViewData,ViewDemo};
    panelstage stagenow = panelstage.Compo;

    private bool[] imageread = new bool[5];

    public Text characterdialog;
    public GameObject datashow;
    public List<string> stringdiag;
    public List<string> stringdata;
    public List<string> stringdiagforprogram;
    private bool readdataornot = false;
    private bool hasGetData = false;
    //============programming===========//

    public GameObject viewinputData;
    public GameObject viewdeviceCode;
    public GameObject viewinputDatabutton;
    public GameObject viewdeviceCodebutton;
    public GameObject viewdevicedemobutton;
    public GameObject backbutton;

    public GameObject paneljackProgramming;
    public GameObject paneldivProgramming;
    public GameObject panelheadlampProgramming;
    public GameObject paneljackinputdata;
    public GameObject paneldivinputdata;
    public GameObject panelheadlampinputdata;

    public GameObject paneldemoProgramming;
    //new used
    //dropdown
    public Dropdown dropdownforprogram;
    public Dropdown dropdownforshowdata;
    public Dropdown dropdownforshowdemo;

    public GameObject functionforJack;
    public GameObject functionforgear;

    public Text dataValue;
    private int curDataPin = -1;
    public GameObject dataforpin0;
    public GameObject dataforpin1;
    public GameObject dataforpin2;

    public GameObject panelprogram;
    public GameObject panelshowdata;
    public GameObject panelshowdemo;
    public GameObject livedemo;
    private bool[] demoworks = new bool[2];
    private bool demowork = false;
    public GameObject movetonextscnebutton;
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        //rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        /*
        buttonCompo.GetComponent<Image>().color = Color.white;
        buttonJack.GetComponent<Image>().color = Color.gray;
        buttondive.GetComponent<Image>().color = Color.gray;
        buttonHead.GetComponent<Image>().color = Color.gray;

        buttonJack.GetComponent<Button>().interactable = false;
        buttondive.GetComponent<Button>().interactable = false;
        buttonHead.GetComponent<Button>().interactable = false;
        
        buttonCompo1.GetComponent<Button>().interactable = false;
        buttonCompo2.GetComponent<Button>().interactable = false;
        buttonCompo3.GetComponent<Button>().interactable = false;
        buttonCompo4.GetComponent<Button>().interactable = false;
        buttonCompo1.GetComponent<Image>().color = Color.gray;
        buttonCompo2.GetComponent<Image>().color = Color.gray;
        buttonCompo3.GetComponent<Image>().color = Color.gray;
        buttonCompo4.GetComponent<Image>().color = Color.gray;
        

        */
        hasGetData = true;
    }

    // Update is called once per frame
    void Update()
    {
        Wiringchanger();
    }
    /*
    public void OnPointerDown(PointerEventData evenData)
    {
        //throw new System.NotImplementedException();
        Debug.Log("OnPointerDown");
    }
    public void OnBeginDrag(PointerEventData evenData)
    {
        Debug.Log("OnBeginDrag");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }*/

    //=============Wiring==============//
    public void Wiringchanger()
    {
        switch (stagenow)
        {
            case panelstage.Compo:
                datashow.GetComponent<Text>().text = stringdata[0];
                //checkifreadall();
                characterdialog.text = stringdiag[0];
                break;
            case panelstage.Compo1:
                datashow.GetComponent<Text>().text = stringdata[0];
                break;
            case panelstage.Compo2:
                datashow.GetComponent<Text>().text = stringdata[0];
                break;
            case panelstage.Compo3:
                datashow.GetComponent<Text>().text = stringdata[0];
                break;
            case panelstage.Compo4:
                datashow.GetComponent<Text>().text = stringdata[1];
                break;
            case panelstage.GetData:
                buttonCheckWiring.SetActive(true);
                characterdialog.text = stringdiag[4];
                datashow.GetComponent<Text>().text = stringdata[2];
                break;
            case panelstage.Jack:
                //checkifhammermovefine();
                break;
            case panelstage.Dive:
                //checkifdivinggearfine();
                break;
            case panelstage.Head:
                //checkifHeadLampfine();
                break;
            case panelstage.ViewCode:
                if(dropdownforprogram.value == 0)
                {
                    characterdialog.text = stringdiagforprogram[0];
                }
                else if(dropdownforprogram.value != 0 && readdataornot == false)
                {
                    characterdialog.text = stringdiagforprogram[1];
                }
                else if(dropdownforprogram.value != 0 && readdataornot == true)
                {
                    characterdialog.text = stringdiagforprogram[2];
                }
                break;
            case panelstage.ViewData:
                if(dropdownforshowdata.value == 0)
                {
                    characterdialog.text = stringdiagforprogram[3];
                }
                else
                {
                    characterdialog.text = stringdiagforprogram[4];
                }
                break;
            case panelstage.ViewDemo:
                checkifdemoworks();
                if (dropdownforshowdemo.value == 0)
                {
                    characterdialog.text = stringdiagforprogram[0];
                }
                else if(dropdownforshowdemo.value != 0 && !demowork)
                {
                    characterdialog.text = stringdiagforprogram[5];
                }
                else if(dropdownforshowdemo.value != 0 && demowork)
                {
                    characterdialog.text = stringdiagforprogram[6];
                    movetonextscnebutton.SetActive(true);
                }
                
                break;
        }
    }
    public void componentpress()
    {
        hasGetData = true;
        stagenow = panelstage.Compo;
        /*
        panelCompo.SetActive(true);
        panelJack.SetActive(false);
        paneldive.SetActive(false);
        panelHead.SetActive(false);
        
        buttonCompo.GetComponent<Image>().color = Color.white;
        buttonJack.GetComponent<Image>().color = Color.gray;
        buttondive.GetComponent<Image>().color = Color.gray;
        buttonHead.GetComponent<Image>().color = Color.gray;
        */
        panelCompo.SetActive(true);
        panelCompo1.SetActive(false);
        panelCompo2.SetActive(false);
        panelCompo3.SetActive(false);
        panelCompo4.SetActive(false);
        characterdialog.text = stringdiag[0];
    }
    public void jackhammerpress()
    {
        stagenow = panelstage.Jack;
        panelCompo.SetActive(false);
        panelJack.SetActive(true);
        paneldive.SetActive(false);
        panelHead.SetActive(false);
        buttonCompo.GetComponent<Image>().color = Color.gray;
        buttonJack.GetComponent<Image>().color = Color.white;
        buttondive.GetComponent<Image>().color = Color.gray;
        buttonHead.GetComponent<Image>().color = Color.gray;
    }
    public void divinggearpress()
    {
        stagenow = panelstage.Dive;
        panelCompo.SetActive(false);
        panelJack.SetActive(false);
        paneldive.SetActive(true);
        panelHead.SetActive(false);
        buttonCompo.GetComponent<Image>().color = Color.gray;
        buttonJack.GetComponent<Image>().color = Color.gray;
        buttondive.GetComponent<Image>().color = Color.white;
        buttonHead.GetComponent<Image>().color = Color.gray;
    }
    public void headlamppress()
    {
        stagenow = panelstage.Head;
        panelCompo.SetActive(false);
        panelJack.SetActive(false);
        paneldive.SetActive(false);
        panelHead.SetActive(true);
        buttonCompo.GetComponent<Image>().color = Color.gray;
        buttonJack.GetComponent<Image>().color = Color.gray;
        buttondive.GetComponent<Image>().color = Color.gray;
        buttonHead.GetComponent<Image>().color = Color.white;
    }
    public void component1press()
    {
        hasGetData = true;
        stagenow = panelstage.Compo1;
        characterdialog.text = stringdiag[1];

        panelCompo.SetActive(false);
        panelCompo1.SetActive(true);
        panelCompo2.SetActive(false);
        panelCompo3.SetActive(false);
        panelCompo4.SetActive(false);
    }
    public void component2press()
    {
        hasGetData = true;
        stagenow = panelstage.Compo2;
        characterdialog.text = stringdiag[2];

        panelCompo.SetActive(false);
        panelCompo1.SetActive(false);
        panelCompo2.SetActive(true);
        panelCompo3.SetActive(false);
        panelCompo4.SetActive(false);
    }
    public void component3press()
    {
        hasGetData = true;
        stagenow = panelstage.Compo3;
        characterdialog.text = stringdiag[3];

        panelCompo.SetActive(false);
        panelCompo1.SetActive(false);
        panelCompo2.SetActive(false);
        panelCompo3.SetActive(true);
        panelCompo4.SetActive(false);
    }
    public void component4press()
    {
        hasGetData = false;
        stagenow = panelstage.Compo4;
        characterdialog.text = stringdiag[3];
        panelCompo.SetActive(false);
        panelCompo1.SetActive(false);
        panelCompo2.SetActive(false);
        panelCompo3.SetActive(false);
        panelCompo4.SetActive(true);
    }
    public void micbrobitintro()
    {
        Microbitintro.SetActive(true);
        imageread[0] = true;
    }
    public void micbrobitintroclose()
    {
        Microbitintro.SetActive(false);
    }
    public void expanintro()
    {
        Expansionintro.SetActive(true);
        imageread[1] = true;
    }
    public void expanclose()
    {
        Expansionintro.SetActive(false);
    }
    public void jumpintro()
    {
        Jumpintro.SetActive(true);
        imageread[2] = true;
    }
    public void jumpclose()
    {
        Jumpintro.SetActive(false);
    }
    public void slderintro()
    {
        Sliderintro.SetActive(true);
        imageread[3] = true;
    }
    public void sliderclose()
    {
        Sliderintro.SetActive(false);
    }
    public void photointro()
    {
        Photointro.SetActive(true);
        imageread[4] = true;
    }
    public void photoclose()
    {
        Photointro.SetActive(false);
    }
    public void waterintro()
    {
        Waterintro.SetActive(true);
        imageread[4] = true;
    }
    public void waterclose()
    {
        Waterintro.SetActive(false);
    }
    public void wiringintroopen(GameObject objinto)
    {
        objinto.SetActive(true);
    }
    public void wiringintroclose(GameObject objinto)
    {
        objinto.SetActive(false);
    }
    public void checkifreadall()
    {
        foreach(var af in imageread)
        {
            if (!af)
            {
                return;
            }
        }
        //set to next stage
        buttonJack.GetComponent<Button>().interactable = true;
        buttonCompo1.GetComponent<Button>().interactable = true;
        buttonCompo2.GetComponent<Button>().interactable = true;
        buttonCompo3.GetComponent<Button>().interactable = true;
        buttonCompo4.GetComponent<Button>().interactable = true;
        buttonCompo1.GetComponent<Image>().color = Color.white;
        buttonCompo2.GetComponent<Image>().color = Color.white;
        buttonCompo3.GetComponent<Image>().color = Color.white;
        buttonCompo4.GetComponent<Image>().color = Color.white;
        //change mentor text

    }
    public void checkifdemoworks()
    {

        foreach (var af in demoworks)
        {
            if (!af)
            {
                demowork = false;
                return;
            }
        }
        demowork = true;
    }

    //public void checkifhammermovefine()
    //{


    //}
    //public void checkifdivinggearfine()
    //{

    //}
    //public void checkifHeadLampfine()
    //{

    //    //set to next stage

    //}

    //public void checkifgetdata(float a)
    //{
    //    if(a != 0.0f)
    //    {
    //        stagenow = panelstage.GetData;

    //    }
    //}

    public void sliderforDivingGear(float a)
    {
        // ??why >0 and < 0
        sliderdiv.GetComponent<Slider>().value = a;
        if (a >= sliderdiv.GetComponent<Slider>().maxValue)
        {
            sliderdivingiftrue[0] = true;
        }

        if (sliderdivingiftrue[0] == true && a <= sliderdiv.GetComponent<Slider>().minValue)
        {
            sliderdivingiftrue[1] = true;
        }

        //set to next stage
        if (sliderdivingiftrue[0] == true && sliderdivingiftrue[1] == true)
            buttonHead.GetComponent<Button>().interactable = true;

    }
    public void sliderforJackhamer(float a)
    {
        sliderJack.GetComponent<Slider>().value = a;

        if (a >= sliderJack.GetComponent<Slider>().maxValue)
        {
            sliderJackiftrue[0] = true;
        }

        if (sliderJackiftrue[0] == true && a == sliderJack.GetComponent<Slider>().minValue)
        {
            sliderJackiftrue[1] = true;
        }

        //set to next stage
        if (sliderJackiftrue[0] == true && sliderJackiftrue[1] == true)
            buttondive.GetComponent<Button>().interactable = true;

    }

    public void setValueforSlider(FunctionType f, float min, float max)
    {
        if (f == FunctionType.jackhammer)
        {
            sliderJack.GetComponent<Slider>().maxValue = max;
            sliderJack.GetComponent<Slider>().minValue = min;
        }
        if (f == FunctionType.headlamp)
        {
            sliderHead.GetComponent<Slider>().maxValue = max;
            sliderHead.GetComponent<Slider>().minValue = min;
        }
        if (f == FunctionType.divinggear)
        {
            sliderdiv.GetComponent<Slider>().maxValue = max;
            sliderdiv.GetComponent<Slider>().minValue = min;
        }

    }

    public void sliderforHeadLamp(float a2)
    {

        sliderHead.GetComponent<Slider>().value = a2;

    }
    public void moveontopragramming()
    {
        PanelForWiring.SetActive(false);
        PanelForprogramming.SetActive(true);
        datashow.SetActive(false);
        stagenow = panelstage.ViewCode;
        hasGetData = true;
    }


    //=============Programming==============//

//    public void jackhammerprogrammingpressed()
//    {
//        paneljackProgramming.SetActive(true);
//        paneldivProgramming.SetActive(false);
//        panelheadlampProgramming.SetActive(false);
//    }
//    public void divinggearprogrammingpressed()
//    {
//        paneljackProgramming.SetActive(false);
//        paneldivProgramming.SetActive(true);
//        panelheadlampProgramming.SetActive(false);
//    }
//    public void headlampprogrammingpressed()
//    {
//        paneljackProgramming.SetActive(false);
//        paneldivProgramming.SetActive(false);
//        panelheadlampProgramming.SetActive(true);
//    }

//    public void jackhammershowdatapressed()
//    {
//        paneljackinputdata.SetActive(true);
//        paneldivinputdata.SetActive(false);
//        panelheadlampinputdata.SetActive(false);
//    }
//    public void divinggearshowdatapressed()
//    {
//        paneljackinputdata.SetActive(false);
//        paneldivinputdata.SetActive(true);
//        panelheadlampinputdata.SetActive(false);
//    }
//    public void headlampshowdatapressed()
//    {
//        paneljackinputdata.SetActive(false);
//        paneldivinputdata.SetActive(false);
//        panelheadlampinputdata.SetActive(true);
//    }
//    public void viewinputdataClicked()
//    {
//        stagenow = panelstage.ViewData;
//        viewinputData.SetActive(true);
//        viewdeviceCode.SetActive(false);
//        viewinputDatabutton.SetActive(false);
//        viewdeviceCodebutton.SetActive(true);
//}
//    public void viewinputViewCodeClicked()
//    {
//        stagenow = panelstage.ViewCode;
//        viewinputData.SetActive(false);
//        viewdeviceCode.SetActive(true);
//        viewinputDatabutton.SetActive(true);
//        viewdeviceCodebutton.SetActive(false);
//    }
//    public void viewDevicedemoClicked()
//    {
//        paneldemoProgramming.SetActive(true);
//        viewdevicedemobutton.SetActive(false);
//        backbutton.SetActive(true);

//    }
//    public void backbuttonprogrammingClicked()
//    {
//        paneldemoProgramming.SetActive(false);
//        viewdevicedemobutton.SetActive(true);
//        backbutton.SetActive(false);

//    }
    public void movetonextScene()
    {
        DontDestroyOnLoad(ParamManager.Instance);
        SceneManager.LoadScene("ForestCavern");
    }
    //new
    public void dropdownvaluechange()
    {
        switch (dropdownforprogram.value)
        {
            case 0:
                functionforJack.SetActive(false);
                break;
            case 1:
                functionforJack.SetActive(true);
                //functionforgear.SetActive(false);
                break;
            case 2:
                functionforJack.SetActive(false);
                //functionforgear.SetActive(true);
                break;
        }
    }
    public void dropdownshowdatavaluechange()
    {
        dataValue.text = "";
        switch (dropdownforshowdata.value)
        {
            case 0:
                dataforpin0.SetActive(false);
                dataforpin1.SetActive(false);
                dataforpin2.SetActive(false);
                curDataPin = -1;
                break;
            case 1:
                // dataforpin0.SetActive(true);
                dataforpin1.SetActive(false);
                dataforpin2.SetActive(false);
                curDataPin = 0;
                break;
            case 2:
                dataforpin0.SetActive(false);
                // dataforpin1.SetActive(true);
                dataforpin2.SetActive(false);
                curDataPin = 1;
                break;
            case 3:
                dataforpin0.SetActive(false);
                dataforpin1.SetActive(false);
                // dataforpin2.SetActive(true);
                curDataPin = 2;
                break;
        }
    }
    public void programpress()
    {
        panelprogram.SetActive(true);
        panelshowdata.SetActive(false);
        panelshowdemo.SetActive(false);
        livedemo.SetActive(false);
        stagenow = panelstage.ViewCode;
    }
    public void showdatapress()
    {
        panelprogram.SetActive(false);
        panelshowdata.SetActive(true);
        panelshowdemo.SetActive(false);
        livedemo.SetActive(false);
        stagenow = panelstage.ViewData;
        readdataornot = true;
    }
    public void showdemopress()
    {
        panelprogram.SetActive(false);
        panelshowdata.SetActive(false);
        panelshowdemo.SetActive(true);
        livedemo.SetActive(true);
        livedemo.GetComponentInChildren<SliderObstacle>().TryInit();
        ObstacleMgr.Instance.SetCurrentObstacle(livedemo.GetComponentInChildren<SliderObstacle>());
        stagenow = panelstage.ViewDemo;
    }

    //new Interface functions for robin
    //new function for setting up pin value and get value
    public void showpinvalue(int i,float value)
    {
        if(i == curDataPin)
        {
            dataValue.text += "Pin " + i + ": " + value + "\n";
            StartCoroutine(dataValue.gameObject.GetComponent<DebugLogController>().ScrollBarBottom());
        }
        /*switch (i)
        {
            case 0:
                dataforpin0.GetComponent<Text>().text = value.ToString();
                break;
            case 1:
                dataforpin1.GetComponent<Text>().text = value.ToString();
                break;
            case 2:
                dataforpin2.GetComponent<Text>().text = value.ToString();
                break;
        }*/
    }
    public void getdataornot(float value, ObstacleType type)
    {
        if (hasGetData) return;
        switch (type)
        {
            case ObstacleType.Slider:
                if(value > 0.0f)
                {
                    stagenow = panelstage.GetData;
                }
                break;
                /*case 1:
                    if (value > 0.0f)
                    {
                        stagenow = panelstage.GetData;
                    }
                    break;
                case 2:
                    if (value > 0.0f)
                    {
                        stagenow = panelstage.GetData;
                    }
                    break;*/
        }
    }

    public void setDemoWork()
    {
        demoworks[0] = true;
        demoworks[1] = false;
        movetonextscnebutton.SetActive(true);
        

    }
    public void demoworkornot(float a, FunctionType f, float min,float max)
    {
        // if the float a change from max to min or min to max the demo work
        if (f == FunctionType.jackhammer)
        { 
            if(a == min)
            {
                demoworks[0] = true;
            }
            if (a == max)
            {
                demoworks[1] = true;
            }
        }
    }

}
