using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class EditorWindowLevelSetting : EditorWindow 
{
    public float paramTest_float;
    public int paramTest_int;
    public bool paramTest_bool;
    public string paramTest_string;

    public GameObject GameManagerPrefab;
    public LevelSetting Level;
    public Wave Wave;

    enum displayFieldType {Enemy_Wave, Meteorites_Wave, Boss_Fight}
    displayFieldType DisplayFieldType;
 
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
    
    [MenuItem("Window/LevelEditor")]
    public static void ShowWindow() 
    {
        var window = GetWindow<EditorWindowLevelSetting>("LevelEditor");
        window.Show();
    }

    private void OnGUI() 
    {
        GUILayout.Label("Welcome to Level Editor");

        paramTest_string = EditorGUILayout.TextField("File Name", paramTest_string);

        GUILayout.Label("Level Editor for gameObj/ Script");

        GameManagerPrefab = EditorGUILayout.ObjectField("GameManager Prefab", GameManagerPrefab, typeof(GameObject), true) as GameObject;
        Level = EditorGUILayout.ObjectField("Level Setting", Level, typeof(LevelSetting), true) as LevelSetting;

        // if(Wave == null) Wave = new Wave();

        // drawBySerial(this);

        //Update our list
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
            

        scrollPos = GUILayout.BeginScrollView(scrollPos);
    
        //Resize our list
        EditorGUILayout.LabelField("Define properties for this level");

        EditorGUILayout.PropertyField(OutputLevel.FindProperty("BG_Image"));

        EditorGUILayout.PropertyField(OutputLevel.FindProperty("isStrongerEachWave"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("statMultipler"));

        EditorGUILayout.PropertyField(OutputLevel.FindProperty("MeteoriteGenSpeed"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("maxInterval"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("minInterval"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("maxSize"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("minSize"));

        EditorGUILayout.PropertyField(OutputLevel.FindProperty("PowerUpRate"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("ChangeSpaceShipRate"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("HealthRate"));
        EditorGUILayout.PropertyField(OutputLevel.FindProperty("MoneyRate"));

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
            //DisplayFieldType = (displayFieldType)EditorGUILayout.EnumPopup("", DisplayFieldType);

            if(DisplayFieldType == displayFieldType.Enemy_Wave)
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
            }
            else if(DisplayFieldType == displayFieldType.Meteorites_Wave)
            {
                SerializedProperty MyListRef = WaveList.GetArrayElementAtIndex(i);
                SerializedProperty MyEnemyPrefabs = MyListRef.FindPropertyRelative("ObjectsForThisWave");
                SerializedProperty MyEnemiesShapes = MyListRef.FindPropertyRelative("enemiesShapes");
                SerializedProperty MyEnemyPathDatas = MyListRef.FindPropertyRelative("enemyPathDatas"); 
        
                // Display the property fields in two ways.
                EditorGUILayout.LabelField("Define ypur wave properties below");
                EditorGUILayout.PropertyField(MyEnemyPrefabs);
                EditorGUILayout.PropertyField(MyEnemiesShapes);
                EditorGUILayout.PropertyField(MyEnemyPathDatas);
            }
            else if(DisplayFieldType == displayFieldType.Boss_Fight)
            {
                SerializedProperty MyListRef = WaveList.GetArrayElementAtIndex(i);
                SerializedProperty MyEnemyPrefabs = MyListRef.FindPropertyRelative("ObjectsForThisWave");
                SerializedProperty MyEnemiesShapes = MyListRef.FindPropertyRelative("enemiesShapes");
                SerializedProperty MyEnemyPathDatas = MyListRef.FindPropertyRelative("enemyPathDatas"); 
        
                // Display the property fields in two ways.
                EditorGUILayout.LabelField("Define ypur wave properties below");
                EditorGUILayout.PropertyField(MyEnemyPrefabs);
                EditorGUILayout.PropertyField(MyEnemiesShapes);
                EditorGUILayout.PropertyField(MyEnemyPathDatas);
            }
    
            EditorGUILayout.Space ();
    
            //Remove this index from the List
            EditorGUILayout.LabelField("Remove an index from the List<> with a button");
            if(GUILayout.Button("Remove This Index (" + i.ToString() + ")"))
            {
                WaveList.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
        }

        GUILayout.EndScrollView();
        //Apply the changes to our list
        OutputLevel.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if(GUILayout.Button("Confirm Setting Level for this Scene"))
        {
            Debug.Log("Confirm => TO DO here");
            var selectedObj = Selection.activeGameObject;
            if(OutputLevel != null && OutputLevel.targetObject != null)
                Level = (LevelSetting)OutputLevel.targetObject;
            if(selectedObj != null && selectedObj.tag == "GameManager")
            {
                Debug.Log("OutputLevel: " + OutputLevel);
                Debug.Log("t: " + t);                
                selectedObj.GetComponent<GameManager>().Level = Level;              
            }
            else
            {
                GUILayout.Label("Please choose Game Manager in your scene to update level setting!!!");
            }
            string path = "Assets/Resources/Levels/" + paramTest_string + ".asset";
            try
            {
                AssetDatabase.CreateAsset(Level, path);
                paramTest_string = "";
            }
            catch(Exception e)
            {
                Debug.LogError("File name is exist: " + e.Message);
            }
        }

    }

    public void drawBySerial(UnityEngine.Object obj)
    {
        SerializedObject test = new SerializedObject(obj);
        SerializedProperty sp = test.FindProperty("Wave");
        EditorGUILayout.PropertyField(sp);
    }      
}
#endif
