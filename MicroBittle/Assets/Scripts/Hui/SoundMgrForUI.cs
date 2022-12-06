using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SoundMgrForUI : MonoBehaviour
{

    //========AudioEffect===============//
    public List<AudioClip> audioEffects;
    public AudioSource audioSource;
    public enum stageforstorymode {Opening,Wiring,Transition,Programming };
    stageforstorymode stageforstorynow;
    public enum stageforother { Opening, Wiring, Programming };
    stageforother stageforothernow;
    public List<string> dialogforstory;
    public List<string> dialogforother;
    private bool[] played = new bool[30];
    public TextMeshProUGUI big;
    public Text side;
    // Start is called before the first frame update
    void Start()
    {
        //only story mode needs this audio so...
        //switch (PlayerPrefs.GetString("mode"))
       // {
       //     case "storymode":
                stageforstorynow = stageforstorymode.Opening;

       //         break;
       //     case "playmode":
      //          stageforothernow = stageforother.Opening;
      //          break;
      //      case "creativemode":
      //          stageforothernow = stageforother.Opening;
      //          break;
      //  }
      //
    }

    // Update is called once per frame
    void Update()
    {
        audiostagechangeforstory();
    }
    public void audiostagechangeforstory()
    {
        string now;
        string nowbig;
        switch (stageforstorynow)
        {
            case stageforstorymode.Opening:
                nowbig = big.text;
                if (nowbig != "")
                {
                    
                    if (played[0] == false)
                    {
                        bool ifsame = checkifstringallthesame(nowbig, dialogforstory[0]);
                        if (ifsame)
                        {
                            PlayDialogue(0);
                            Debug.Log(nowbig);
                            played[0] = true;
                        }

                    }
                    
                    if (played[1] == false)
                    {
                        bool ifsame = checkifstringallthesame(nowbig, dialogforstory[1]);
                        if (ifsame)
                        {
                            PlayDialogue(1);
                            Debug.Log(nowbig);
                            played[1] = true;
                        }

                    }
/*                    
                    if (nowbig[0] == dialogforstory[1][0] && played[1] == false)
                    {
                        PlayDialogue(1);
                        Debug.Log(nowbig);
                        played[1] = true;
                    }
*/
                    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) && played[1] == true)
                    {
                        print("audio goes to wiring stage");
                        stageforstorynow = stageforstorymode.Wiring;
                    }
                }

                break;
            case stageforstorymode.Wiring:
                now = side.text;
                nowbig = big.text;
                if(now != "")
                {
                    if(played[2] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[2]);
                        if (ifsame)
                        {
                            PlayDialogue(2);
                            Debug.Log(now);
                            played[2] = true;

                        }
                        
                    }

                    if (played[3] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[3]);
                        if (ifsame)
                        {
                            PlayDialogue(3);
                            Debug.Log(now);
                            played[3] = true;
                        }

                    }

                    if (played[4] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[4]);
                        if (ifsame)
                        {
                            PlayDialogue(4);
                            Debug.Log(now);
                            played[4] = true;
                        }

                    }
                    if (played[5] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[5]);
                        if (ifsame)
                        {
                            PlayDialogue(5);
                            Debug.Log(now);
                            played[5] = true;
                        }

                    }
                    if (played[6] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[6]);
                        if (ifsame)
                        {
                            PlayDialogue(6);
                            Debug.Log(now);
                            played[6] = true;
                        }

                    }
                    

                    if(nowbig != "")
                    {
                        //move to transition
                        stageforstorynow = stageforstorymode.Transition;
                        Debug.Log("change to Transition");
                    }

                }
                break;
            case stageforstorymode.Transition:
                nowbig = big.text;
                if(nowbig != "")
                {
                    if (played[7] == false)
                    {
                        bool ifsame = checkifstringallthesame(nowbig, dialogforstory[7]);
                        if (ifsame)
                        {
                            PlayDialogue(7);
                            Debug.Log(nowbig);
                            played[7] = true;
                        }

                    }

                    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) && played[7] == true)
                    {
                        print("audio goes to programming stage");
                        stageforstorynow = stageforstorymode.Programming;
                    }
                }
                break;
            case stageforstorymode.Programming:
                now = side.text;
                if(now != "")
                {
                    if (played[8] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[8]);
                        if (ifsame)
                        {
                            PlayDialogue(8);
                            Debug.Log(now);
                            played[8] = true;
                        }

                    }

                    if (played[9] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[9]);
                        if (ifsame)
                        {
                            PlayDialogue(9);
                            Debug.Log(now);
                            played[9] = true;
                        }

                    }
                    if (played[10] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[10]);
                        if (ifsame)
                        {
                            PlayDialogue(10);
                            Debug.Log(now);
                            played[10] = true;
                        }

                    }
                    if (played[11] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[11]);
                        if (ifsame)
                        {
                            PlayDialogue(11);
                            Debug.Log(now);
                            played[11] = true;
                        }

                    }
                    if (played[12] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[12]);
                        if (ifsame)
                        {
                            PlayDialogue(12);
                            Debug.Log(now);
                            played[12] = true;
                        }

                    }
                    if (played[13] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[13]);
                        if (ifsame)
                        {
                            PlayDialogue(13);
                            Debug.Log(now);
                            played[13] = true;
                        }

                    }
                    if (played[14] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[14]);
                        if (ifsame)
                        {
                            PlayDialogue(14);
                            Debug.Log(now);
                            played[14] = true;
                        }

                    }
                    if (played[15] == false)
                    {
                        bool ifsame = checkifstringallthesame(now, dialogforstory[15]);
                        if (ifsame)
                        {
                            PlayDialogue(15);
                            Debug.Log(now);
                            played[15] = true;
                        }

                    }
                }
                break;
        }
    }
    public void PlayDialogue(int clip)
    {
        if (audioSource == null)
        {
            return;
        }
        audioSource.Stop();
        audioSource.PlayOneShot(audioEffects[clip]);
    }
    public bool checkifstringallthesame(string str1,string str2)
    {
        int len = str1.Length >= str2.Length ? str2.Length : str1.Length;
        for (int i = 0;i < len; i++)
        {
            if(str1[i] != str2[i])
            {
                return false;
            }
        }
        return true;
    }
}
