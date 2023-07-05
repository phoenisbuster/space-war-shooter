using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

[NodeWidth(300)]
public class LevelNode : Node
{
    [Input] public bool CheckWaveListOK;
    [Output] public bool CheckLevelOK;
    public string FileName;
    public LevelSetting Output;
    
    // GetValue should be overridden to return a value for any specified output port
    public override object GetValue(NodePort port) 
    {

        // Get new a and b values from input connections. Fallback to field values if input is not connected
        bool a = GetInputValue<bool>("CheckWaveListOK", this.CheckWaveListOK);

        CheckLevelOK = a;

        // After you've gotten your input values, you can perform your calculations and return a value
        return CheckLevelOK;
    }
}
