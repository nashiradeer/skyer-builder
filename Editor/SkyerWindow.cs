using UnityEditor;
using UnityEngine;

namespace DeerSoftware.SkyerBuilder
{
    public class SkyerWindow : EditorWindow
    {
        private string outputPath = "";
        private bool configFoldout = true;

        private void Awake()
        {
            titleContent = new GUIContent("Skyer Builder");
        }

        [MenuItem("Window/Skyer Builder")]
        private static void Init()
        {
            GetWindow<SkyerWindow>().Show();
        }

        private void OnGUI()
        {
            if ((configFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(configFoldout, "Configuration")))
            {
                EditorGUILayout.BeginHorizontal();
                outputPath = EditorGUILayout.TextField("Output Path", "");
                if (GUILayout.Button("...", GUILayout.ExpandWidth(false)))
                    outputPath = EditorUtility.SaveFolderPanel("Choosing Output Path...", outputPath, PlayerSettings.productName);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}
