using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

#if UNITY_EDITOR
using XNodeEditor;
[CustomNodeEditor(typeof(PathNode))]
public class PathNodeEditor : NodeEditor
{
    //Para call Path Node
    public PathNode pathNode;

    //////////Param for Manual Custom///////////
    public enum LevelEditTypes {VisualizeAdd, ManualAdd}
    public LevelEditTypes levelEditTypes = LevelEditTypes.VisualizeAdd;
    public bool FinishInitialize = false;
    public string spawnPoints;
    public string middlePoints;

    //Params determine Path of Enemies
    public int[] LeftBoundInt = GetNewArray(10, -1, false);
    public int[] RightBoundInt = GetNewArray(10, -1, false);
    public int[] UpBoundInt = GetNewArray(6, -1, false);
    public int[] LowBoundInt = GetNewArray(6, -1, false);
    public int [,] OnScreenInt = GetNew2DArray(5, 10, -1, false);

    ////////Attributes for UI Editor/////////
    GUIStyle OnScreenTableStyle;
    GUIStyle UpLowTableStyle;
    GUIStyle OnScreenTableStyleInt;
    GUIStyle UpLowTableStyleInt;
    GUIStyle LeftColumnStyle;
    GUIStyle RightColumnStyle;
    GUIStyle UpRowStyle;
    GUIStyle LowRowStyle;
    GUIStyle UpRowStyleInt;
    GUIStyle LowRowStyleInt;
    GUIStyle OnScreenColumnStyle;
    GUIStyle OnScreenRowStyle;
    GUIStyle rowHeaderStyle;
    GUIStyle TwoSideColStyle;
    GUIStyle columnLabelStyle;
    GUIStyle cornerLabelStyle;
    GUIStyle rowLabelStyle;
    GUIStyle OnScreenEnumStyle;
    GUIStyle OtherEnumStyle;

    public static int[,] GetNew2DArray(int x, int y, int initialValue, bool isIncrease)
    {
        int[,] nums = new int[x, y];
        for (int i = 0; i < x * y; i++) 
        {
            nums[i % x, i / x] = !isIncrease? initialValue : initialValue+i;
        }
        return nums;
    }
    public static int[] GetNewArray(int x, int initialValue, bool isIncrease)
    {
        int[] nums = new int[x];
        for (int i = 0; i < x; i++) 
        {
            nums[i] = !isIncrease? initialValue : initialValue+i;
        }
        return nums;
    }
    void OnEnable()
    {
        CreateLevel();
        ResetArgument();
    }

    void CreateLevel()
    {
        if(pathNode != null && pathNode.Output == null)
        {
            pathNode.Output = ScriptableObject.CreateInstance<EnemyPathData>();
        }  
    }

    void ResetArgument()
    {
        LeftBoundInt = GetNewArray(10, -1, false);
        RightBoundInt = GetNewArray(10, -1, false);
        UpBoundInt = GetNewArray(6, -1, false);
        LowBoundInt = GetNewArray(6, -1, false);
        OnScreenInt = GetNew2DArray(5, 10, -1, false);
    }
    public void SettingInOutParameters()
    {
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("FileName"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Output"));        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("TimeBetweeenPoints"));

        levelEditTypes = (LevelEditTypes)EditorGUILayout.EnumPopup("", levelEditTypes);
    }
    public void InputProcessing()
    {
        if(levelEditTypes == LevelEditTypes.ManualAdd)
        {
            LeftBoundInt = GetNewArray(10, 0, true);
            RightBoundInt = GetNewArray(10, 10, true);
            LowBoundInt = GetNewArray(6, 20, true);
            UpBoundInt = GetNewArray(6, 26, true);            
            OnScreenInt = GetNew2DArray(5, 10, 32, true);
        }
        else if(levelEditTypes == LevelEditTypes.VisualizeAdd && !FinishInitialize)
        {
            ResetArgument();
        }

        if(levelEditTypes == LevelEditTypes.ManualAdd)
        {
            spawnPoints = EditorGUILayout.TextField(
                new GUIContent("Spawn Points", "Unordered, separated by ','"), 
                spawnPoints);
            middlePoints = EditorGUILayout.TextField(
                new GUIContent("Middle Points", "Ordered, separated by ','"), 
                middlePoints);
        }
        else
        {
            FinishInitialize = EditorGUILayout.Toggle(
                new GUIContent("Finish Init", "Tooggle On To Finish Setting => Allow to customize path"), 
                FinishInitialize);
        }
    }
    public void SettingEditorUI()
    {
        /*
            Set table UI
        */
        OnScreenTableStyle = new GUIStyle("box");
        OnScreenTableStyle.padding = new RectOffset(10, 10, 10, 10);
        OnScreenTableStyle.margin.left = 60;

        UpLowTableStyle = new GUIStyle("box");
        UpLowTableStyle.padding = new RectOffset(10, 10, 10, 10);
        UpLowTableStyle.margin.left = 90;

        OnScreenTableStyleInt = new GUIStyle("box");
        OnScreenTableStyleInt.padding = new RectOffset(10, 10, 10, 10);
        OnScreenTableStyleInt.margin.left = 600;

        UpLowTableStyleInt = new GUIStyle("box");
        UpLowTableStyleInt.padding = new RectOffset(10, 10, 10, 10);
        UpLowTableStyleInt.margin.left = 630;

        /*
            Set components UI
        */
        LeftColumnStyle = new GUIStyle();
        LeftColumnStyle.fixedWidth = 60;
        LeftColumnStyle.fixedHeight = 30;

        RightColumnStyle = new GUIStyle();
        RightColumnStyle.fixedWidth = 60;
        RightColumnStyle.fixedHeight = 30;

        UpRowStyle = new GUIStyle();
        UpRowStyle.fixedWidth = 60;
        UpRowStyle.fixedHeight = 30;

        LowRowStyle = new GUIStyle();
        LowRowStyle.fixedWidth = 60;
        LowRowStyle.fixedHeight = 30;

        UpRowStyleInt = new GUIStyle();
        UpRowStyleInt.fixedWidth = 60;
        UpRowStyleInt.fixedHeight = 45;

        LowRowStyleInt = new GUIStyle();
        LowRowStyleInt.fixedWidth = 60;
        LowRowStyleInt.fixedHeight = 45;

        OnScreenColumnStyle = new GUIStyle();
        OnScreenColumnStyle.fixedWidth = 60;
        OnScreenColumnStyle.margin.top = 15;

        TwoSideColStyle = new GUIStyle();
        TwoSideColStyle.fixedWidth = 60;
        TwoSideColStyle.fixedHeight = 50;

        OnScreenRowStyle = new GUIStyle();
        OnScreenRowStyle.fixedWidth = 60;
        OnScreenRowStyle.fixedHeight = 30;

        /*
            Set components UI
        */
        OnScreenEnumStyle = new GUIStyle("popup");
        OnScreenEnumStyle.fixedWidth = 50;
        OnScreenEnumStyle.fixedHeight = 20;

        OtherEnumStyle = new GUIStyle("popup");
        OtherEnumStyle.fixedWidth = 50;
        OtherEnumStyle.fixedHeight = 30;
    }

    public override void OnBodyGUI()
    {
        if(pathNode == null)
        {
            pathNode = target as PathNode;
        }

        GUILayout.Label("Welcome to Path Editor");
        
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("CheckInput"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("CheckOutput"));
        
        SettingInOutParameters();   
        InputProcessing();
        SettingEditorUI();

        EditorGUILayout.BeginVertical(OnScreenColumnStyle);
        EditorGUILayout.BeginHorizontal(UpLowTableStyle);
        for(int i = 0; i < UpBoundInt.Length; i++)
        {
            EditorGUILayout.BeginVertical(UpRowStyle);                    
            UpBoundInt[i] = EditorGUILayout.IntField(UpBoundInt[i], OtherEnumStyle);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(OnScreenTableStyle);
        for (int x = -1; x < OnScreenInt.GetLength(0)+1; x++) 
        {
            EditorGUILayout.BeginVertical((x == -1 || x == OnScreenInt.GetLength(0))? TwoSideColStyle : OnScreenColumnStyle);
            for (int y = -1; y < OnScreenInt.GetLength(1); y++) 
            {
                if (x == -1) 
                {
                    if(y > -1 && y < LeftBoundInt.Length)
                    {
                        EditorGUILayout.BeginVertical(LeftColumnStyle);
                        LeftBoundInt[y] = EditorGUILayout.IntField(LeftBoundInt[y], OtherEnumStyle);
                        EditorGUILayout.EndVertical();
                    }    
                } 
                else if(x == OnScreenInt.GetLength(0))
                {
                    if(y > -1 && y < RightBoundInt.Length)
                    {
                        EditorGUILayout.BeginVertical(RightColumnStyle);
                        RightBoundInt[y] = EditorGUILayout.IntField(RightBoundInt[y], OtherEnumStyle);
                        EditorGUILayout.EndVertical();
                    }
                }

                if (x >= 0 && y >= 0 && x < OnScreenInt.GetLength(0) && y < OnScreenInt.GetLength(1)) 
                {
                    EditorGUILayout.BeginHorizontal(OnScreenRowStyle);
                    OnScreenInt[x, y] = EditorGUILayout.IntField(OnScreenInt[x, y]);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(UpLowTableStyle);
        for(int i = 0; i < LowBoundInt.Length; i++)
        {
            EditorGUILayout.BeginVertical(LowRowStyle);                    
            LowBoundInt[i] = EditorGUILayout.IntField(LowBoundInt[i], OtherEnumStyle);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        if(GUILayout.Button("Confirm Setting Path to export file") && pathNode.Output != null)
        {         
            List<int> _spawnPoints = new List<int>();
            List<int> _middlePoints = new List<int>();
            if(levelEditTypes == LevelEditTypes.VisualizeAdd)
            {    
                SortedDictionary<int, int> middelPointsDict = new SortedDictionary<int, int>();
                
                for(int i=0; i < LeftBoundInt.Length; i++)
                {
                    if(LeftBoundInt[i]==0)
                    {
                        _spawnPoints.Add(i);
                    }
                    else if(LeftBoundInt[i] > 0)
                    {
                        middelPointsDict.Add(LeftBoundInt[i]-1, i);                    
                        //middelPoints.Insert(LeftBoundInt[i]-1, i);
                    }
                }
                for(int i=0; i < RightBoundInt.Length; i++)
                {
                    if(RightBoundInt[i]==0)
                    {
                        _spawnPoints.Add(i+10);
                    }
                    else if(RightBoundInt[i] > 0)
                    {
                        middelPointsDict.Add(RightBoundInt[i]-1, i+10);
                        //middelPoints.Insert(RightBoundInt[i]-1, i+10);
                    }
                }
                for(int i=0; i < LowBoundInt.Length; i++)
                {
                    if(LowBoundInt[i]==0)
                    {
                        _spawnPoints.Add(i+20);
                    }
                    else if(LowBoundInt[i] > 0)
                    {
                        middelPointsDict.Add(LowBoundInt[i]-1, i+20);
                        //middelPoints.Insert(LowBoundInt[i]-1, i+20);
                    }
                }
                for(int i=0; i < UpBoundInt.Length; i++)
                {
                    if(UpBoundInt[i]==0)
                    {
                        _spawnPoints.Add(i+26);
                    }
                    else if(UpBoundInt[i] > 0)
                    {
                        middelPointsDict.Add(UpBoundInt[i]-1, i+26);
                        //middelPoints.Insert(UpBoundInt[i]-1, i+26);
                    }
                }
                
                for(int i=0; i < OnScreenInt.GetLength(0); i++)
                {
                    for(int j=0; j < OnScreenInt.GetLength(1); j++)
                    {    
                        if(OnScreenInt[i,j]==0)
                        {
                            _spawnPoints.Add(i + (j*5) + 32);
                        }
                        else if(OnScreenInt[i,j] > 0)
                        {
                            middelPointsDict.Add(OnScreenInt[i,j]-1, i + (j*5) + 32);
                            //middelPoints.Insert(OnScreenInt[i,j]-1, i + (j*5) + 32);
                        }
                    }
                }

                foreach(var i in middelPointsDict)
                {
                    //Debug.Log("Values: " + i.Key + ", " + i.Value);
                    _middlePoints.Add(i.Value);
                }
            }
            else
            {
                try
                {
                    List<string> t1 = spawnPoints.Split(',').ToList<string>();
                    List<string> t2 = middlePoints.Split(',').ToList<string>();
                    foreach(var i in t1)
                    {
                        try
                        {
                            int value = Int32.Parse(i);
                            _spawnPoints.Add(value);
                        }
                        catch(FormatException e)
                        {
                            Debug.LogError("Cannot convert string to int at " + i + " in " + t1 + ".Error " + e.Message);
                        }
                    }
                    foreach(var j in t2)
                    {
                        try
                        {
                            int value = Int32.Parse(j);
                            _middlePoints.Add(value);
                        }
                        catch(FormatException e)
                        {
                            Debug.LogError("Cannot convert string to int at " + j + " in " + t2 + ".Error " + e.Message);
                        }
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError("The input String is not in the correct format!!!" + e.Message);
                }
            }
            pathNode.Output.firstPoint = _spawnPoints;
            pathNode.Output.middlePointIndex = _middlePoints;
            pathNode.Output.time = pathNode.TimeBetweeenPoints;

            string path = "Assets/Resources/EnemiesPaths/" + pathNode.FileName + ".asset";
            try
            {
                AssetDatabase.CreateAsset(pathNode.Output, path);
                pathNode.FileName = "";
                pathNode.Output = null;
                CreateLevel();
                ResetArgument();
                GUILayout.Label("Adding new Path file success!!!");
            }
            catch(Exception e)
            {
                Debug.LogError("File name is exist: " + e.Message);
                CreateLevel();
            }
        }
        else
        {
            CreateLevel();
        }

    }
}
#endif
