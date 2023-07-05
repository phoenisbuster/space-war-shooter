using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

#if UNITY_EDITOR
using XNodeEditor;
[CustomNodeEditor(typeof(ShapeNode))]
public class ShapeNodeEditor : NodeEditor
{
    //Para call Shape Node
    public ShapeNode shapeNode;

    //Params determine Shape of Enemies
    public enum BlockValue {No, Yes};
    public BlockValue [,] board = new BlockValue[5, 10];
    public int rows;
    public int collumns;
    public int StartRowsIdx = 0;
    public int StartCollumnsIdx = 0;
    private bool isFinishSize = false;
    private bool isCustomSize = false;

    ////////Attributes for UI Editor/////////
    GUIStyle tableStyle;
    GUIStyle headerColumnStyle;
    GUIStyle columnStyle;
    GUIStyle rowStyle;
    GUIStyle rowHeaderStyle;
    GUIStyle columnHeaderStyle;
    GUIStyle columnLabelStyle;
    GUIStyle cornerLabelStyle;
    GUIStyle rowLabelStyle;
    GUIStyle enumStyle; 

    void OnEnable()
    {
        CreateLevel();
    }

    void CreateLevel()
    {
        if(shapeNode != null && shapeNode.Output == null)
        {
            shapeNode.Output = ScriptableObject.CreateInstance<EnemiesShape>();
        }        
    }

    public void SettingInOutParameters()
    {
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("FileName"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Output"));
        
        rows = EditorGUILayout.IntSlider(
            new GUIContent("Rows of array", "Row is greater than 0 and maximum is 10"), 
            rows, 0, 10);
        collumns = EditorGUILayout.IntSlider(
            new GUIContent("Cols of array", "Col is greater than 0 and maximum is 5"), 
            collumns, 0, 5);

        StartRowsIdx = EditorGUILayout.IntSlider(
            new GUIContent("Rows start at", "Default is 0, This value + Rows must <= 10"), 
            StartRowsIdx, 0, 10 - rows);
        StartCollumnsIdx = EditorGUILayout.IntSlider(
            new GUIContent("Collumns start at", "Default is 0, This value + Columns must <= 5"), 
            StartCollumnsIdx, 0, 5 - collumns);

        isFinishSize = EditorGUILayout.Toggle(
            new GUIContent("Finish Set Size", "Tooggle On To Finish Setting Size => Allow to customize enemy"), 
            isFinishSize);
    }
    public void InputProcessing()
    {
        if(rows > 0 && collumns > 0 && !isFinishSize)
        {
            board = new BlockValue[collumns, rows];
            isCustomSize = true;
        }
        else if((rows <= 0 || collumns <= 0) && !isFinishSize)
        {
            board = new BlockValue[5, 10];
            isCustomSize = false;
        }  
    }
    public void SettingEditorUI()
    {
        tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 60;

        headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 35;

        columnStyle = new GUIStyle();
        columnStyle.fixedWidth = 65;

        rowStyle = new GUIStyle();
        rowStyle.fixedWidth = 65;
        rowStyle.fixedHeight = 45;

        rowHeaderStyle = new GUIStyle();
        rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

        columnHeaderStyle = new GUIStyle();
        columnHeaderStyle.fixedWidth = 30;
        columnHeaderStyle.fixedHeight = 46;

        columnLabelStyle = new GUIStyle();
        columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
        columnLabelStyle.alignment = TextAnchor.MiddleCenter;
        columnLabelStyle.fontStyle = FontStyle.Bold;

        cornerLabelStyle = new GUIStyle();
        cornerLabelStyle.fixedWidth = 42;
        cornerLabelStyle.alignment = TextAnchor.MiddleRight;
        cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
        cornerLabelStyle.fontSize = 14;
        cornerLabelStyle.padding.top = -5;

        rowLabelStyle = new GUIStyle();
        rowLabelStyle.fixedWidth = 25;
        rowLabelStyle.alignment = TextAnchor.MiddleRight;
        rowLabelStyle.fontStyle = FontStyle.Bold;

        enumStyle = new GUIStyle("popup");
        enumStyle.fixedWidth = 60;
        enumStyle.fixedHeight = 40;
    }

    public override void OnBodyGUI()
    {
        if(shapeNode == null)
        {
            shapeNode = target as ShapeNode;
        }

        GUILayout.Label("Welcome to Shape Editor");

        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("CheckInput"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("CheckOutput"));

        SettingInOutParameters();
        InputProcessing();    
        SettingEditorUI();

        EditorGUILayout.BeginHorizontal(tableStyle);
        for (int x = -1; x < board.GetLength(0); x++) 
        {
            EditorGUILayout.BeginVertical((x == -1) ? headerColumnStyle : columnStyle);
            for (int y = -1; y < board.GetLength(1); y++) 
            {
                if (x == -1 && y == -1) 
                {
                    EditorGUILayout.BeginVertical(rowHeaderStyle);
                    EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
                    EditorGUILayout.EndHorizontal();
                } 
                else if (x == -1) 
                {
                    EditorGUILayout.BeginVertical(columnHeaderStyle);
                    EditorGUILayout.LabelField(y.ToString(), rowLabelStyle);
                    EditorGUILayout.EndHorizontal();
                } 
                else if (y == -1) 
                {
                    EditorGUILayout.BeginVertical(rowHeaderStyle);
                    EditorGUILayout.LabelField(x.ToString(), columnLabelStyle);
                    EditorGUILayout.EndHorizontal();
                }

                if (x >= 0 && y >= 0) 
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    board[x, y] = (BlockValue)EditorGUILayout.EnumPopup(board[x, y], enumStyle);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.Space();

        if(GUILayout.Button("Confirm Setting Shape to export file") && shapeNode.Output != null)
        {         
            List<int> newShapeIdx = new List<int>();

            if(!isCustomSize)
            {
                for(int i = 0; i < board.GetLength(0); i++)
                {
                    for(int j = 0; j < board.GetLength(1); j++)
                    {
                        if(board[i,j] == BlockValue.Yes)
                        {
                            newShapeIdx.Add(i + (j*5));
                        }
                    }
                }
            }
            else
            {
                for(int i = 0; i < board.GetLength(0); i++)
                {
                    for(int j = 0; j < board.GetLength(1); j++)
                    {
                        if(board[i,j] == BlockValue.Yes)
                        {
                            int rowIdx = i+StartRowsIdx <= 10? i+StartRowsIdx : 10;
                            int colIdx = j+StartCollumnsIdx <= 5? j+StartCollumnsIdx : 5;
                            
                            newShapeIdx.Add(rowIdx + (colIdx * 5));
                        }
                    }
                }
            }

            shapeNode.Output.ID_of_Pos = newShapeIdx;
            string path = "Assets/Resources/EnemiesShapes/" + shapeNode.FileName + ".asset";
            try
            {
                AssetDatabase.CreateAsset(shapeNode.Output, path);
                shapeNode.FileName = "";
                shapeNode.Output = null;
                CreateLevel();
                board = new BlockValue[5,10];
                GUILayout.Label("Adding new Shape file success!!!");
            }
            catch(Exception e)
            {
                Debug.LogError("File name is exist: " + e.Message);
            }
        }
        else
        {
            CreateLevel();
        }            
    }
}
#endif
