﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.DragDrop;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;
using MG_BlocksEngine2.Utils;
using System.Reflection;
using MG_BlocksEngine2.Attribute;

namespace MG_BlocksEngine2.Serializer
{
    public static class BE2_BlocksSerializer
    {
        // v2.3 - added method SaveCode to facilitate the save of code by script
        public static void SaveCode(string path, I_BE2_ProgrammingEnv targetProgrammingEnv)
        {
            StreamWriter sw = new StreamWriter(path, false);

            targetProgrammingEnv.UpdateBlocksList();

            foreach (I_BE2_Block block in targetProgrammingEnv.BlocksList)
            {
                sw.WriteLine(BE2_BlocksSerializer.SerializableToXML(BE2_BlocksSerializer.BlockToSerializable(block)));
                sw.WriteLine("#");
            }

            sw.Close();
        }

        // v2.9 - BlockToSerializable refactored to enable and facilitate the addition of custom variable types
        public static BE2_SerializableBlock BlockToSerializable(I_BE2_Block block)
        {
            BE2_SerializableBlock serializableBlock = new BE2_SerializableBlock();

            serializableBlock.blockName = block.Transform.name;
            // v2.4 - bugfix: fixed blocks load in wrong position if resolution changes
            serializableBlock.position = block.Transform.localPosition;

            System.Type instructionType = block.Instruction.GetType();
            SerializeAsVariableAttribute varAttribute = (SerializeAsVariableAttribute)System.Attribute.GetCustomAttribute(instructionType, typeof(SerializeAsVariableAttribute));

            if (varAttribute != null)
            {
                //  var varManagerField = block.Instruction.GetType().GetField("_variablesManager", BindingFlags.Public | BindingFlags.NonPublic |
                //                                                                                  BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                System.Type varManagerType = varAttribute.variablesManagerType;

                serializableBlock.varManagerName = varManagerType.ToString();
                // serializableBlock.isVariable = true;
                // v2.1 - using BE2_Text to enable usage of Text or TMP components
                BE2_Text varName = BE2_Text.GetBE2Text(block.Transform.GetChild(0).GetChild(0).GetChild(0));
                serializableBlock.varName = varName.text;
            }
            else
            {
                // serializableBlock.isVariable = false;
                serializableBlock.varManagerName = "";
            }

            foreach (I_BE2_BlockSection section in block.Layout.SectionsArray)
            {
                BE2_SerializableSection serializableSection = new BE2_SerializableSection();
                serializableBlock.sections.Add(serializableSection);

                foreach (I_BE2_BlockSectionHeaderInput input in section.Header.InputsArray)
                {
                    BE2_SerializableInput serializableInput = new BE2_SerializableInput();
                    serializableSection.inputs.Add(serializableInput);

                    I_BE2_Block inputBlock = input.Transform.GetComponent<I_BE2_Block>();
                    if (inputBlock != null)
                    {
                        serializableInput.isOperation = true;
                        serializableInput.operation = BlockToSerializable(inputBlock);
                    }
                    else
                    {
                        serializableInput.isOperation = false;
                        serializableInput.value = input.InputValues.stringValue;
                    }
                }

                if (section.Body != null)
                {
                    foreach (I_BE2_Block childBlock in section.Body.ChildBlocksArray)
                    {
                        serializableSection.childBlocks.Add(BlockToSerializable(childBlock));
                    }
                }
            }

            return serializableBlock;
        }

        public static string SerializableToXML(BE2_SerializableBlock serializableBlock)
        {
            // JsonUtility has a depth limitation but you can use another Json alternative
            return BE2_BlockXML.SBlockToXElement(serializableBlock).ToString();
        }

        // v2.3 - added method LoadCode to facilitate the load of code by script
        public static bool LoadCode(string path, I_BE2_ProgrammingEnv targetProgrammingEnv)
        {
            if (File.Exists(path))
            {
                var sr = new StreamReader(path);
                string xmlCode = sr.ReadToEnd();
                sr.Close();

                string[] xmlBlocks = xmlCode.Split('#');

                foreach (string xmlBlock in xmlBlocks)
                {
                    BE2_SerializableBlock serializableBlock = BE2_BlocksSerializer.XMLToSerializable(xmlBlock);
                    BE2_BlocksSerializer.SerializableToBlock(serializableBlock, targetProgrammingEnv);
                }

                return true;
            }

            return false;
        }

        public static BE2_SerializableBlock XMLToSerializable(string blockString)
        {
            // v2.2 - bugfix: fixed empty blockString from XML file causing error on load
            blockString = blockString.Trim();
            if (blockString.Length > 1)
            {
                // JsonUtility has a depth limitation but you can use another Json alternative
                XElement xBlock = XElement.Parse(blockString);
                return BE2_BlockXML.XElementToSBlock(xBlock);
            }
            else
            {
                return null;
            }
        }

        static IEnumerator C_AddInputs(I_BE2_Block block, BE2_SerializableBlock serializableBlock, I_BE2_ProgrammingEnv programmingEnv)
        {
            yield return new WaitForEndOfFrame();

            I_BE2_BlockSection[] sections = block.Layout.SectionsArray;
            for (int s = 0; s < sections.Length; s++)
            {
                I_BE2_BlockSectionHeaderInput[] inputs = sections[s].Header.InputsArray;
                for (int i = 0; i < inputs.Length; i++)
                {
                    BE2_SerializableInput serializableInput = serializableBlock.sections[s].inputs[i];
                    if (serializableInput.isOperation)
                    {
                        I_BE2_Block operation = SerializableToBlock(serializableInput.operation, programmingEnv);
                        BE2_DragDropManager.Instance.CurrentSpot = inputs[i].Transform.GetComponent<I_BE2_Spot>();
                        operation.Transform.GetComponent<I_BE2_Drag>().OnPointerDown();
                        operation.Transform.GetComponent<I_BE2_Drag>().OnPointerUp();
                    }
                    else
                    {
                        InputField inputText = inputs[i].Transform.GetComponent<InputField>();
                        Dropdown inputDropdown = inputs[i].Transform.GetComponent<Dropdown>();
                        if (inputText)
                        {
                            inputText.text = serializableInput.value;
                        }
                        else if (inputDropdown)
                        {
                            inputDropdown.value = inputDropdown.options.FindIndex(option => option.text == serializableInput.value);
                        }
                    }
                    inputs[i].UpdateValues();
                }

                I_BE2_BlockSectionBody body = sections[s].Body;
                if (body != null)
                {
                    // add children
                    foreach (BE2_SerializableBlock serializableChildBlock in serializableBlock.sections[s].childBlocks)
                    {
                        I_BE2_Block childBlock = SerializableToBlock(serializableChildBlock, programmingEnv);
                        childBlock.Transform.SetParent(body.RectTransform);
                    }
                }

                sections[s].Header.UpdateItemsArray();
                sections[s].Header.UpdateInputsArray();
            }
        }

        // v2.9 - SerializableToBlock refactored to enable and facilitate the addition of custom variable types
        public static I_BE2_Block SerializableToBlock(BE2_SerializableBlock serializableBlock, I_BE2_ProgrammingEnv programmingEnv)
        {
            I_BE2_Block block = null;

            if (serializableBlock != null)
            {
                string prefabName = serializableBlock.blockName;
                GameObject loadedPrefab = Resources.Load<GameObject>("Blocks/" + prefabName);
                if (!loadedPrefab)
                    loadedPrefab = Resources.Load<GameObject>("Blocks/Custom/" + prefabName);
                // v2.3 - using settable paths
                if (!loadedPrefab)
                    loadedPrefab = Resources.Load<GameObject>(BE2_Paths.PathToResources(BE2_Paths.TranslateMarkupPath(BE2_Paths.NewBlockPrefabPath)) + prefabName);

                if (loadedPrefab)
                {
                    GameObject blockGo = MonoBehaviour.Instantiate(
                        loadedPrefab,
                        serializableBlock.position,
                        Quaternion.identity,
                        programmingEnv.Transform) as GameObject;

                    blockGo.name = prefabName;

                    // v2.6 - adjustments on position and angle of blocks for supporting all canvas render modes              
                    // v2.4 - bugfix: fixed blocks load in wrong position if resolution changes
                    blockGo.transform.localPosition = new Vector3(serializableBlock.position.x, serializableBlock.position.y, 0);
                    blockGo.transform.localEulerAngles = Vector3.zero;

                    block = blockGo.GetComponent<I_BE2_Block>();

                    if (serializableBlock.isVariable)
                    {
                        // v2.1 - using BE2_Text to enable usage of Text or TMP components
                        //                                        | block        | section   | header    | text      |
                        BE2_Text newVarName = BE2_Text.GetBE2Text(block.Transform.GetChild(0).GetChild(0).GetChild(0));
                        newVarName.text = serializableBlock.varName;

                        BE2_VariablesManager.instance.CreateAndAddVarToPanel(serializableBlock.varName);
                    }
                    else if (serializableBlock.varManagerName != null && serializableBlock.varManagerName != "")
                    {
                        //                                        | block        | section   | header    | text      |
                        BE2_Text newVarName = BE2_Text.GetBE2Text(block.Transform.GetChild(0).GetChild(0).GetChild(0));
                        newVarName.text = serializableBlock.varName;

                        System.Type varManagerType = System.Type.GetType(serializableBlock.varManagerName);
                        if (varManagerType != null)
                        {
                            I_BE2_VariablesManager varManager = MonoBehaviour.FindObjectOfType(varManagerType) as I_BE2_VariablesManager;
                            varManager.CreateAndAddVarToPanel(serializableBlock.varName);
                        }
                        else
                        {
                            Debug.Log("Variables manager of type *" + 0000 + "* was not found.");
                        }
                    }

                    // add inputs
                    BE2_ExecutionManager.Instance.StartCoroutine(C_AddInputs(block, serializableBlock, programmingEnv));

                    if (block.Type == BlockTypeEnum.trigger)
                    {
                        BE2_ExecutionManager.Instance.AddToBlocksStackArray(block.Instruction.InstructionBase.BlocksStack, programmingEnv.TargetObject);
                    }
                }
            }

            return block;
        }
    }
}