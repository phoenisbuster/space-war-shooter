using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

#if UNITY_EDITOR
using XNodeEditor;
[CustomNodeEditor(typeof(XNode_StartNode))]
public class XNode_Editor : NodeEditor
{
    public XNode_StartNode startNode;

    public enum OperatorType { Add, Subtract, Multiply, Divide};
    public OperatorType mathType = OperatorType.Add;

    LevelSetting t;
    SerializedObject OutputLevel;
    SerializedProperty WaveList;
    int WaveNumber;
    Vector2 scrollPos = Vector2.zero;

    void OnEnable()
    {
        CreateLevel();
    }

    void CreateLevel()
    {
        if(t == null)
            t = ScriptableObject.CreateInstance<LevelSetting>();
        OutputLevel = new SerializedObject(t);
        WaveList = OutputLevel.FindProperty("WaveList"); // Find the List in our script and create a refrence of it
    }

    public override void OnBodyGUI()
    {
        if(startNode == null)
        {
            startNode = target as XNode_StartNode;
        }

        GUILayout.Label("Welcome to Level Editor");
        GUILayout.Label("Level Editor for gameObj/ Script");

        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("a"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("b"));

        mathType = (OperatorType)EditorGUILayout.EnumPopup("", mathType);
        startNode.mathType = (XNode_StartNode.MathType)mathType;
        
        EditorGUILayout.LabelField("The operator is: " + startNode.mathType);
        float result = 0;
        switch((int)startNode.mathType)
        {
            case 0: default: 
                result = startNode.GetSum();
                break;
            case 1: 
                result = startNode.GetSub();
                break;
            case 2: 
                result = startNode.GetMul();
                break;
            case 3: 
                result = startNode.GetDiv(); 
                break;
        }
        EditorGUILayout.LabelField("The operator is: " + result);
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("result"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("sum"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("FileName"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Output"));

        if(OutputLevel != null && OutputLevel.targetObject != null)
        {
            //Debug.Log(OutputLevel);
            //Debug.Log(OutputLevel == null);
            OutputLevel.Update();
        }
        else
        {
            //Debug.Log(OutputLevel);
            //Debug.Log(OutputLevel == null);
            CreateLevel();
        }

        //scrollPos = GUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.LabelField("Define properties for this level");

        EditorGUILayout.PropertyField(OutputLevel.FindProperty("isStrongerEachWave"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("statMultipler"));

        EditorGUILayout.PropertyField(OutputLevel.FindProperty("MeteoriteGenSpeed"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("maxInterval"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("minInterval"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("maxSize"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("minSize"));

        EditorGUILayout.Space ();
        EditorGUILayout.Space ();
        EditorGUILayout.LabelField("Define the number of waves you want for this level");
        WaveNumber = WaveList.arraySize;
        WaveNumber = EditorGUILayout.IntField("Total Wave Number", WaveNumber);

        if(WaveNumber != WaveList.arraySize){
            while(WaveNumber > WaveList.arraySize)
            {
                WaveList.InsertArrayElementAtIndex(WaveList.arraySize);
            }
            while(WaveNumber < WaveList.arraySize)
            {
                WaveList.DeleteArrayElementAtIndex(WaveList.arraySize - 1);
            }
        }

        EditorGUILayout.Space ();
        EditorGUILayout.LabelField("Or");
        EditorGUILayout.Space ();

        //Or add a new item to the List<> with a button
        EditorGUILayout.LabelField("Add a new wave with a button");

        if(GUILayout.Button("Add New"))
        {
            if(t.WaveList == null)
            {
                t.WaveList = new List<Wave>();
            }
            t.WaveList.Add(new Wave());
        }

        EditorGUILayout.Space ();
        
        //Display our list to the inspector window
        for(int i = 0; i < WaveList.arraySize; i++)
        {
            SerializedProperty MyListRef = WaveList.GetArrayElementAtIndex(i);
            SerializedProperty MyEnemyPrefabs = MyListRef.FindPropertyRelative("ObjectsForThisWave");
            SerializedProperty MyEnemiesShapes = MyListRef.FindPropertyRelative("enemiesShapes");
            SerializedProperty MyEnemyPathDatas = MyListRef.FindPropertyRelative("enemyPathDatas"); 
    
            // Display the property fields in two ways.
            EditorGUILayout.LabelField("Define your wave properties below");
            EditorGUILayout.PropertyField(MyEnemyPrefabs);
            EditorGUILayout.PropertyField(MyEnemiesShapes);
            EditorGUILayout.PropertyField(MyEnemyPathDatas);
    
            EditorGUILayout.Space ();
    
            //Remove this index from the List
            EditorGUILayout.LabelField("Remove an index from the List<> with a button");
            if(GUILayout.Button("Remove This Index (" + i.ToString() + ")"))
            {
                WaveList.DeleteArrayElementAtIndex(i);
            }
        }

        //GUILayout.EndScrollView();
        OutputLevel.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if(GUILayout.Button("Confirm Setting Level for this Scene"))
        {
            var selectedObj = Selection.activeGameObject;
            if(OutputLevel != null && OutputLevel.targetObject != null)
                startNode.Output = (LevelSetting)OutputLevel.targetObject;
            if(selectedObj != null && selectedObj.tag == "GameManager")
            {
                Debug.Log("OutputLevel: " + OutputLevel);
                Debug.Log("t: " + t);                
                selectedObj.GetComponent<GameManager>().Level = startNode.Output;              
            }
            else
            {
                GUILayout.Label("Please choose Game Manager in your scene to update level setting!!!");
            }
            string path = "Assets/Resources/Levels/" + startNode.FileName + ".asset";
            try
            {
                AssetDatabase.CreateAsset(startNode.Output, path);
                startNode.FileName = "";
            }
            catch(Exception e)
            {
                Debug.LogError("File name is exist: " + e.Message);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif