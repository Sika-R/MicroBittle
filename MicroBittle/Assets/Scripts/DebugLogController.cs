using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DebugLogController : MonoBehaviour
{
    public Scrollbar verticalScrollbar;

    void Start()
    {
        
    }

    public IEnumerator ScrollBarBottom()
    {
        yield return null;
        verticalScrollbar.value = 0;
    }

}
