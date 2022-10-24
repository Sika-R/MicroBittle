﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace MG_BlocksEngine2.Block
{
    public class BE2_BlockSectionHeader_InputField : MonoBehaviour, I_BE2_BlockSectionHeaderItem, I_BE2_BlockSectionHeaderInput
    {
        InputField _inputField;
        RectTransform _rectTransform;

        public Transform Transform => transform;
        public Vector2 Size => _rectTransform ? _rectTransform.sizeDelta : GetComponent<RectTransform>().sizeDelta;
        public I_BE2_Spot Spot { get; set; }
        public float FloatValue { get; set; }
        public string StringValue { get; set; }
        public BE2_InputValues InputValues { get; set; }

        void OnValidate()
        {
            Awake();
        }

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _inputField = GetComponent<InputField>();
            Spot = GetComponent<I_BE2_Spot>();
        }

        void OnEnable()
        {
            UpdateValues();
            _inputField.onEndEdit.AddListener(delegate { UpdateValues(); });
        }

        void OnDisable()
        {
            _inputField.onEndEdit.RemoveAllListeners();
        }

        void Start()
        {
            UpdateValues();
        }

        //void Update()
        //{
        //
        //}

        public void UpdateValues()
        {
            bool isText;
            string stringValue = "";
            if (_inputField.text != null)
            {
                stringValue = _inputField.text;
            }
            StringValue = stringValue;

            float floatValue = 0;
            try
            {
                floatValue = float.Parse(StringValue, CultureInfo.InvariantCulture);
                isText = false;
            }
            catch
            {
                isText = true;
            }
            FloatValue = floatValue;

            InputValues = new BE2_InputValues(StringValue, FloatValue, isText);
        }
    }
}