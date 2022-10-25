using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class programUI : MonoBehaviour
    //, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //[SerializeField] private Canvas canvas;
    //public RectTransform rectTransform;
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

    public GameObject sliderJack;
    public GameObject sliderdiv;
    public GameObject sliderHead;

    private bool[] sliderJackiftrue = new bool[2];
    private bool[] sliderdivingiftrue = new bool[2];
    private enum panelstage { Compo,Jack,Dive,Head};
    panelstage stagenow = panelstage.Compo;

    private bool[] imageread = new bool[2];

    void Awake()
    {
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

        sliderdiv.GetComponent<Slider>().value = a;
        if (a == 1.0f)
        {
            sliderdivingiftrue[0] = true;
        }

        if (sliderdivingiftrue[0] == true && a == 0.0f)
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
        
        if (a == 1.0f)
        {
            sliderJackiftrue[0] = true;
        }

        if (sliderJackiftrue[0] == true && a == 0.0f)
        {
            sliderJackiftrue[1] = true;
        }

        //set to next stage
        if (sliderJackiftrue[0] == true && sliderJackiftrue[1] == true)
            buttondive.GetComponent<Button>().interactable = true;

    }
    public void sliderforHeadLamp(float a)
    {

        sliderHead.GetComponent<Slider>().value = a;

    }
}
