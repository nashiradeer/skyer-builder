using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DeerSoftware.SkyerBuilder
{
    public class SkyerWindow : EditorWindow
    {
        private string outputPath = "";
        private int target = 0;
        private int subtarget = 0;
        private bool configFoldout = true;
        private bool addTargetFoldout = true;
        private List<SkyerPlatform> targets = new List<SkyerPlatform>();

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

            if ((addTargetFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(addTargetFoldout, "Add Target")))
            {
                int newTarget = EditorGUILayout.Popup("Target", target, Enum.GetNames(typeof(SkyerTarget)).Skip(1).ToArray());

                if (newTarget != target)
                    subtarget = 0;

                target = newTarget;

                SkyerTarget selectedTarget = (SkyerTarget)target + 1;

                SkyerSubtarget[] subtargets = SkyerUtils.GetSkyerSubtargets(selectedTarget);

                string[] subtargetOptions = GetExtraSubtargets(selectedTarget, subtargets);

                subtarget = EditorGUILayout.Popup("Sub-target", subtarget, subtargetOptions);

                SkyerSubtarget selectedSubtarget = subtargets[subtarget];

                if (GUILayout.Button("Add"))
                {
                    SkyerPlatform platform = new SkyerPlatform()
                    {
                        Target = selectedTarget,
                        Subtarget = selectedSubtarget,
                    };

                    if (!targets.Contains(platform))
                        targets.Add(platform);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private string[] GetExtraSubtargets(SkyerTarget target, SkyerSubtarget[] subtargets)
        {
            List<string> subtargetNames = new List<string>();

            subtargetNames.Add(SkyerUtils.GetDefaultSubtargetName(target));

            for (int i = 1; i < subtargets.Length; i++)
                subtargetNames.Add(Enum.GetName(typeof(SkyerSubtarget), subtargets[i]));

            return subtargetNames.ToArray();
        }
    }
}
