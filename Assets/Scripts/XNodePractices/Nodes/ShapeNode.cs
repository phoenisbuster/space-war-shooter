using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

[NodeWidth(650)]
public class ShapeNode : Node
{
    [Input] public bool CheckInput;
    [Output] public bool CheckOutput;
    public string FileName;
    public EnemiesShape Output;
    
    // GetValue should be overridden to return a value for any specified output port
    public override object GetValue(NodePort port) 
    {

        // Get new a and b values from input connections. Fallback to field values if input is not connected
        bool a = GetInputValue<bool>("CheckInput", this.CheckInput);

        CheckOutput = a;

        // After you've gotten your input values, you can perform your calculations and return a value
        return CheckOutput;
    }
}
