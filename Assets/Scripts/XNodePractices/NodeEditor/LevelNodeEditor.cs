using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

#if UNITY_EDITOR
using XNodeEditor;
[CustomNodeEditor(typeof(LevelNode))]
public class LevelNodeEditor : NodeEditor
{   
    //Para call Level Node
    public LevelNode levelNode;

    //Options to edit
    public enum LevelEditTypes {CustomizeEachWave, UsingNodeGraph}
    public LevelEditTypes levelEditTypes = LevelEditTypes.CustomizeEachWave;

    //Option parameters
    public int noLevelFiles = 1;
    public int startLevel = 3;
    public int increasePerLevel = 0;
    public GameObject Boss;
    public GameObject Meteos;
    public List<GameObject> Enemies;
    public float EnemyRate = 75f;
    public int BossEveryWave = 5;
    public bool isBossFinalWave = false;

    //Params For Update Level Node
    LevelSetting t;
    SerializedObject OutputLevel;
    SerializedProperty WaveList;
    int WaveNumber;

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
        if(levelNode == null)
        {
            levelNode = target as LevelNode;
        }

        GUILayout.Label("Welcome to Level Editor");
        GUILayout.Label("Level Editor for gameObj/ Script");

        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("CheckWaveListOK"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("CheckLevelOK"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("FileName"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Output"));

        if(OutputLevel != null && OutputLevel.targetObject != null)
        {
            OutputLevel.Update();
        }
        else
        {
            CreateLevel();
        }

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

        levelEditTypes = (LevelEditTypes)EditorGUILayout.EnumPopup("", levelEditTypes);
        if(levelEditTypes == LevelEditTypes.CustomizeEachWave)
        {
            EditorGUILayout.LabelField("Define the number of waves you want for this level");
            WaveNumber = WaveList.arraySize;
            WaveNumber = EditorGUILayout.IntField("Total Wave Number", WaveNumber);

            if(WaveNumber != WaveList.arraySize)
            {
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

            OutputLevel.ApplyModifiedProperties();
        }
        else
        {
            EditorGUILayout.LabelField("In Development, Please Use the First Option");
            EnemiesShape[] result1 = Array.ConvertAll(Resources.LoadAll("EnemiesShapes", typeof(EnemiesShape)), asset => (EnemiesShape)asset);
            EnemyPathData[] result2 = Array.ConvertAll(Resources.LoadAll("EnemiesPaths", typeof(EnemyPathData)), asset => (EnemyPathData)asset);
            GameObject[] result3 = Array.ConvertAll(Resources.LoadAll("EnemiesPrefabs", typeof(GameObject)), asset => (GameObject)asset);
            List<EnemiesShape> enemiesShapes = result1.ToList();
            List<EnemyPathData> enemyPaths = result2.ToList();
            Enemies = result3.ToList();

            noLevelFiles = EditorGUILayout.IntField("Files generated:", noLevelFiles);
            startLevel = EditorGUILayout.IntField("First Lv has:", startLevel);
            increasePerLevel = EditorGUILayout.IntField("Increase per Lv:", increasePerLevel);

            WaveNumber = EditorGUILayout.IntField("Total Wave Number", WaveNumber);                
            Boss = EditorGUILayout.ObjectField("BossPrefabs", Boss, typeof(GameObject), false) as GameObject;
            Meteos = EditorGUILayout.ObjectField("MeteoPrefabs", Meteos, typeof(GameObject), false) as GameObject;
            
            EnemyRate = EditorGUILayout.FloatField("Enemy Rate", EnemyRate);
            BossEveryWave = EditorGUILayout.IntField("BossEveryWave", BossEveryWave);
            isBossFinalWave = EditorGUILayout.Toggle("Final Wave is Boss", isBossFinalWave);

            if(GUILayout.Button("Auto Generate Level"))
            {
                var waveList = new List<Wave>();
                for(int i = 1; i <= WaveNumber; i++)
                {
                    float isEnemyWave = UnityEngine.Random.Range(0f, 100f);
                    if(i % BossEveryWave == 0 || (i == WaveNumber && isBossFinalWave))
                    {
                        Debug.Log("Boss At Wave: " + i + "/" + WaveNumber);
                        var wave = new Wave();
                        wave.ObjectsForThisWave = new List<GameObject>();
                        wave.ObjectsForThisWave.Add(Boss);
                        waveList.Add(wave);
                    }
                    else //if(i % BossEveryWave != 0 || (i != WaveNumber))
                    { 
                        if(isEnemyWave <= EnemyRate)
                        {
                            Debug.Log("Enemies At Wave: " + i + "/" + WaveNumber);
                            var wave = new Wave();
                            wave.ObjectsForThisWave = Enemies;
                            waveList.Add(wave);
                        }
                        else if(isEnemyWave > EnemyRate)
                        {
                            Debug.Log("Meteo At Wave: " + i + "/" + WaveNumber);
                            var wave = new Wave();
                            wave.ObjectsForThisWave = new List<GameObject>();
                            wave.ObjectsForThisWave.Add(Meteos);
                            waveList.Add(wave);
                        }
                    }
                }
                t.WaveList = waveList;
            }
        }

        EditorGUILayout.Space();

        if(GUILayout.Button("Confirm Setting Level for this Scene"))
        {
            // var selectedObj = Selection.activeGameObject;
            // if(OutputLevel != null && OutputLevel.targetObject != null)
            //     levelNode.Output = (LevelSetting)OutputLevel.targetObject;
            // if(selectedObj != null && selectedObj.tag == "GameManager")
            // {
            //     Debug.Log("OutputLevel: " + OutputLevel);
            //     Debug.Log("t: " + t);                
            //     selectedObj.GetComponent<GameManager>().Level = levelNode.Output;              
            // }
            // else
            // {
            //     GUILayout.Label("Please choose Game Manager in your scene to update level setting!!!");
            // }
            levelNode.Output = t;
            string path = "Assets/Resources/Levels/" + levelNode.FileName + ".asset";
            try
            {
                AssetDatabase.CreateAsset(levelNode.Output, path);
                levelNode.FileName = "";
            }
            catch(Exception e)
            {
                Debug.LogError("File name is exist: " + e.Message);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void drawBySerial(UnityEngine.Object obj)
    {
        SerializedObject test = new SerializedObject(obj);
        SerializedProperty sp = test.FindProperty("Wave");
        EditorGUILayout.PropertyField(sp);
    } 
}
#endif