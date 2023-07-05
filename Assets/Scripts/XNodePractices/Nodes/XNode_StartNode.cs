using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

public class XNode_StartNode : Node
{
    [Input] public float a;
    [Input] public float b;
    [Output] public float result;
    [Output] public float sum;
    public string FileName;
    public LevelSetting Output;

    // The value of 'mathType' will be displayed on the node in an editable format, similar to the inspector
    public MathType mathType = MathType.Add;
    public enum MathType { Add, Subtract, Multiply, Divide}

    
    // GetValue should be overridden to return a value for any specified output port
    public override object GetValue(NodePort port) 
    {

        // Get new a and b values from input connections. Fallback to field values if input is not connected
        float a = GetInputValue<float>("a", this.a);
        float b = GetInputValue<float>("b", this.b);

        // After you've gotten your input values, you can perform your calculations and return a value
        if(port.fieldName == "result")
            switch(mathType) 
            {
                case MathType.Add: default: return GetSum();
                case MathType.Subtract: return GetSub();
                case MathType.Multiply: return GetMul();
                case MathType.Divide: return GetDiv();
            }
        else if (port.fieldName == "sum") return GetSum();
        else return 0f;
    }

    public float GetSum()
    {
        return a+b;
    }
    public float GetSub()
    {
        return a-b;
    }
    public float GetMul()
    {
        return a*b;
    }
    public float GetDiv()
    {
        return a/b;
    }

    public void OnGUI()
    {
        GUILayout.Label("Welcome to Graph Editor");
    }
}
