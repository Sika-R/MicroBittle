using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        PlayerPrefs.SetString("mode", "playmode");
        SceneManager.LoadScene("customize");//playmode jump to customrize scene
    }
    public void storybuttonClicked()
    {
        PlayerPrefs.SetString("mode", "storymode");
        SceneManager.LoadScene("Intro");//storymode jump to customrize scene
    }
    public void creativebuttonClicked()
    {
        PlayerPrefs.SetString("mode", "creativemode");
        SceneManager.LoadScene("CreativeMode");//creativemode jump to CreativeMode scene
    }
}
