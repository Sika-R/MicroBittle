using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MG_BlocksEngine2.UI;
using MG_BlocksEngine2.Core;

namespace MG_BlocksEngine2.Environment
{
    // v2.7 - added a class to the extras that implements the logic for show/hide the Blocks Selection panel  
    public class BE2_HideBlocksSelection : MonoBehaviour
    {
        BE2_Canvas _blocksSelectionCanvas;
        public Vector3 hidePosition;
        Dictionary<Transform, Vector3> _envs = new Dictionary<Transform, Vector3>();

        void Start()
        {
            _blocksSelectionCanvas = GetComponentInParent<BE2_Canvas>();
            hidePosition = _blocksSelectionCanvas.transform.GetChild(0).position;

            GetComponent<Button>().onClick.AddListener(HideBlocksSelection);

            foreach (BE2_UI_SelectionButton button in FindObjectsOfType<BE2_UI_SelectionButton>())
            {
                button.GetComponent<Button>().onClick.AddListener(ShowBlocksSelection);
            }

            foreach (I_BE2_ProgrammingEnv env in BE2_ExecutionManager.Instance.ProgrammingEnvsList)
            {
                _envs.Add(env.Transform.GetComponentInParent<BE2_Canvas>().Canvas.transform.GetChild(0), env.Transform.position);
            }
        }

        // void Update()
        // {

        // }

        public void HideBlocksSelection()
        {
            _blocksSelectionCanvas.gameObject.SetActive(false);

            foreach (KeyValuePair<Transform, Vector3> env in _envs)
            {
                env.Key.position = hidePosition;
            }
        }

        public void ShowBlocksSelection()
        {
            if (!_blocksSelectionCanvas.gameObject.activeSelf)
            {
                _blocksSelectionCanvas.gameObject.SetActive(true);

                foreach (KeyValuePair<Transform, Vector3> env in _envs)
                {
                    env.Key.position = env.Value;
                }
            }
        }
    }
}