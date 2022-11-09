using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class test : MonoBehaviour
{
    public Text namelabel;

    // Start is called before the first frame update
    void Start()
    {
        namelabel.text = PlayerPrefs.GetString("name");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
