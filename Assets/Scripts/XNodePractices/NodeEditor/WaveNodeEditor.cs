using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

#if UNITY_EDITOR
using XNodeEditor;
[CustomNodeEditor(typeof(WaveNode))]
public class WaveNodeEditor : NodeEditor
{
    
}
#endif
