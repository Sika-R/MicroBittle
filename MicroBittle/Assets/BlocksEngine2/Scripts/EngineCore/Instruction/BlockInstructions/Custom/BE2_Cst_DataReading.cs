using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --- most used BE2 namespaces for instruction scripts 
using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Block;

// --- additional BE2 namespaces used for specific cases as accessing BE2 variables or the event manager
// using MG_BlocksEngine2.Core;
// using MG_BlocksEngine2.Environment;

public class BE2_Cst_DataReading : BE2_InstructionBase, I_BE2_Instruction
{
    // ► Refer to the documentation for more on the variables and methods

    // ### Execution Methods ###

    // --- Method used to implement Function Blocks (will only be called by types: simple, condition, loop, trigger)
    public new void Function()
    {
        // --- use Section0Inputs[inputIndex] to get the Block inputs from the first section (index 0).
        // --- Optionally, use GetSectionInputs(sectionIndex)[inputIndex] to get inputs from a different section
        // --- the input values can be retrieved as .StringValue, .FloatValue or .InputValues 
        // Section0Inputs[inputIndex];

        // ### Stack Pointer Calls ###
        
        // --- execute first block inside the indicated section, used to execute blocks inside this block (ex. if, if/else, repeat)
        //ExecuteSection(sectionIndex);
        
        // --- execute next block after this, used to finish the execution of this function
        ExecuteNextInstruction();
    }

    // --- Method used to implement Operation Blocks (will only be called by type: operation)
    public new string Operation()
    {
        string result = "";
        
        // --- use Section0Inputs[inputIndex] to get the Block inputs from the first section (index 0).
        // --- Optionally, use GetSectionInputs(sectionIndex)[inputIndex] to get inputs from a different section
        // --- the input values can be retrieved as .StringValue, .FloatValue or .InputValues 
        // Section0Inputs[inputIndex];
        
        // --- opeartion results are always of type string.
        // --- bool return strings are usually "1", "true", "0", "false".
        // --- numbers are returned as strings and converted on the input get.
        return result;
    }

    // ### Execution Setting ###

    // --- Use ExecuteInUpdate for functions that plays repeatedly in update, holding the blocks stack execution flow until completed (ex.: wait, lerp).
    // --- Default value is false. Loop Blocks are always executed in update (true).
    //public new bool ExecuteInUpdate => true; 

    // ### Additional Methods ###

    // --- executed after base Awake
    //protected override void OnAwake()
    //{
    //    
    //}
    
    // --- executed after base Start
    //protected override void OnStart()
    //{
    //    
    //}

    // --- Update can be overridden
    //void Update()
    //{
    //
    //}

    // --- executed on button play pressed
    //protected override void OnButtonPlay()
    //{
    //
    //}

    // --- executed on button stop pressed
    //protected override void OnButtonStop()
    //{
    //
    //}

    // --- executed after blocks stack is populated
    //public override void OnPrepareToPlay()
    //{
    //
    //}

    // --- executed on the stack transition from deactive to active
    //public override void OnStackActive()
    //{
    //
    //}
}
