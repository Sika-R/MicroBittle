using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlaytestLoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadForest()
    {
        SceneManager.LoadScene("ForestCavern");
    }

    public void loadPyramid()
    {
        SceneManager.LoadScene("DesertPyramid");
    }

    public void loadTundra()
    {
        SceneManager.LoadScene("TundraCave");
    }
}
