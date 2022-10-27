using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG_BlocksEngine2.UI
{
    [ExecuteInEditMode]
    public class BE2_UI_SelectionBlock : MonoBehaviour
    {
        public GameObject prefabBlock;

        void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child0 = transform.GetChild(i);
                Transform pref0 = prefabBlock.transform.GetChild(i);
                child0.GetComponent<RectTransform>().sizeDelta = pref0.GetComponent<RectTransform>().sizeDelta;

                for (int j = 0; j < child0.childCount; j++)
                {
                    Transform child1 = child0.GetChild(j);
                    Transform pref1 = pref0.GetChild(j);

                    child1.GetComponent<RectTransform>().sizeDelta = pref1.GetComponent<RectTransform>().sizeDelta;

                    for (int h = 0; h < child1.childCount; h++)
                    {
                        Transform child2 = child1.GetChild(h);
                        Transform pref2 = pref1.GetChild(h);

                        child2.GetComponent<RectTransform>().sizeDelta = pref2.GetComponent<RectTransform>().sizeDelta;
                    }
                }
            }
        }
    }
}