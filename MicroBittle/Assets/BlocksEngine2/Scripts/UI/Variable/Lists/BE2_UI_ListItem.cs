using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MG_BlocksEngine2.Utils;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;

namespace MG_BlocksEngine2.UI
{
    public class BE2_UI_ListItem : MonoBehaviour
    {
        BE2_UI_VariableListViewer _variableListViewer;
        public InputField inputField;
        Button _removeItemButton;

        void Awake()
        {
            _variableListViewer = GetComponentInParent<BE2_UI_VariableListViewer>();
            inputField = transform.GetChild(0).GetComponent<InputField>();
            _removeItemButton = transform.GetChild(1).GetComponent<Button>();
        }

        void OnEnable()
        {
            inputField.onEndEdit.AddListener(delegate { _variableListViewer.UpdateListValues(); });
            _removeItemButton.onClick.AddListener(RemoveItem);
        }

        void OnDisable()
        {
            inputField.onEndEdit.RemoveAllListeners();
            _removeItemButton.onClick.RemoveAllListeners();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void RemoveItem()
        {
            BE2_VariablesListManager.instance.RemoveListItem(_variableListViewer.listName, transform.GetSiblingIndex());
            gameObject.SetActive(false);
            BE2_UI_BlocksSelectionViewer.Instance.ForceRebuildLayout();
            Destroy(gameObject);
        }
    }
}