using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playbutton;
    public GameObject storybutton;
    public GameObject creativebutton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playbuttonClicked()
    {
        SceneManager.LoadScene("Intro");
    }
    public void storybuttonClicked()
    {

    }
    public void creativebuttonClicked()
    {

    }
}
