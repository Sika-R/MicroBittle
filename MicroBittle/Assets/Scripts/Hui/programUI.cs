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
    public GameObject panelJack;
    public GameObject paneldive;
    public GameObject panelHead;
    public GameObject buttonCompo;
    public GameObject buttonJack;
    public GameObject buttondive;
    public GameObject buttonHead;

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
    private enum panelstage { Compo,Jack,Dive,Head,ViewCode,ViewData};
    panelstage stagenow = panelstage.Compo;

    private bool[] imageread = new bool[6];


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
        buttonCompo.GetComponent<Image>().color = Color.white;
        buttonJack.GetComponent<Image>().color = Color.gray;
        buttondive.GetComponent<Image>().color = Color.gray;
        buttonHead.GetComponent<Image>().color = Color.gray;

        buttonJack.GetComponent<Button>().interactable = false;
        buttondive.GetComponent<Button>().interactable = false;
        buttonHead.GetComponent<Button>().interactable = false;

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
                checkifreadall();
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
        }
    }
    public void componentpress()
    {
        stagenow = panelstage.Compo;
        panelCompo.SetActive(true);
        panelJack.SetActive(false);
        paneldive.SetActive(false);
        panelHead.SetActive(false);

        buttonCompo.GetComponent<Image>().color = Color.white;
        buttonJack.GetComponent<Image>().color = Color.gray;
        buttondive.GetComponent<Image>().color = Color.gray;
        buttonHead.GetComponent<Image>().color = Color.gray;
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
        imageread[5] = true;
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
        //change mentor text

    }
    public void checkifhammermovefine()
    {


    }
    public void checkifdivinggearfine()
    {

    }
    public void checkifHeadLampfine()
    {

        //set to next stage
        
    }

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
        if(f == FunctionType.jackhammer)
        {
            sliderJack.GetComponent<Slider>().maxValue = max;
            sliderJack.GetComponent<Slider>().minValue = min;
        }
        if(f == FunctionType.headlamp)
        {
            sliderHead.GetComponent<Slider>().maxValue = max;
            sliderHead.GetComponent<Slider>().minValue = min;
        }
        if(f == FunctionType.divinggear)
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
        stagenow = panelstage.ViewCode;
    }


    //=============Programming==============//

    public void jackhammerprogrammingpressed()
    {
        paneljackProgramming.SetActive(true);
        paneldivProgramming.SetActive(false);
        panelheadlampProgramming.SetActive(false);
    }
    public void divinggearprogrammingpressed()
    {
        paneljackProgramming.SetActive(false);
        paneldivProgramming.SetActive(true);
        panelheadlampProgramming.SetActive(false);
    }
    public void headlampprogrammingpressed()
    {
        paneljackProgramming.SetActive(false);
        paneldivProgramming.SetActive(false);
        panelheadlampProgramming.SetActive(true);
    }

    public void jackhammershowdatapressed()
    {
        paneljackinputdata.SetActive(true);
        paneldivinputdata.SetActive(false);
        panelheadlampinputdata.SetActive(false);
    }
    public void divinggearshowdatapressed()
    {
        paneljackinputdata.SetActive(false);
        paneldivinputdata.SetActive(true);
        panelheadlampinputdata.SetActive(false);
    }
    public void headlampshowdatapressed()
    {
        paneljackinputdata.SetActive(false);
        paneldivinputdata.SetActive(false);
        panelheadlampinputdata.SetActive(true);
    }
    public void viewinputdataClicked()
    {
        stagenow = panelstage.ViewData;
        viewinputData.SetActive(true);
        viewdeviceCode.SetActive(false);
        viewinputDatabutton.SetActive(false);
        viewdeviceCodebutton.SetActive(true);
}
    public void viewinputViewCodeClicked()
    {
        stagenow = panelstage.ViewCode;
        viewinputData.SetActive(false);
        viewdeviceCode.SetActive(true);
        viewinputDatabutton.SetActive(true);
        viewdeviceCodebutton.SetActive(false);
    }
    public void viewDevicedemoClicked()
    {
        paneldemoProgramming.SetActive(true);
        viewdevicedemobutton.SetActive(false);
        backbutton.SetActive(true);

    }
    public void backbuttonprogrammingClicked()
    {
        paneldemoProgramming.SetActive(false);
        viewdevicedemobutton.SetActive(true);
        backbutton.SetActive(false);

    }
    public void movetonextScene()
    {
        SceneManager.LoadScene("SceneTest");
    }
}
