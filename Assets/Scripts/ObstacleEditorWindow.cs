using UnityEditor;
using UnityEngine;

public class ObstacleEditorWindow : EditorWindow
{
    private ObstacleData obstacleData;

    [MenuItem("Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    void OnGUI()
    {
        if (obstacleData == null)
        {
            obstacleData = AssetDatabase.LoadAssetAtPath<ObstacleData>("Assets/ObstacleData.asset");
            if (obstacleData == null)
            {
                obstacleData = CreateInstance<ObstacleData>();
                AssetDatabase.CreateAsset(obstacleData, "Assets/ObstacleData.asset");
                AssetDatabase.SaveAssets();
            }
        }

        for (int y = 0; y < 10; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                obstacleData.obstacles[index] = EditorGUILayout.Toggle(obstacleData.obstacles[index], GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(obstacleData);
            AssetDatabase.SaveAssets();
        }
    }
}
