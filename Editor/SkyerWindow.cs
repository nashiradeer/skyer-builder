using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEditor.Build.Reporting;
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
        private bool targetsFoldout = true;

        private Vector2 targetsSVPos = Vector2.zero;

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
            bool startBuild = false;

            if ((configFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(configFoldout, "Configuration")))
            {
                EditorGUILayout.BeginHorizontal();

                outputPath = EditorGUILayout.TextField("Output Path", outputPath);

                if (GUILayout.Button("...", GUILayout.ExpandWidth(false)))
                    outputPath = EditorUtility.SaveFolderPanel("Choosing Output Path...", outputPath, PlayerSettings.productName);

                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Start Build"))
                    startBuild = true;
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

            if ((targetsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(targetsFoldout, "Targets")))
            {
                targetsSVPos = EditorGUILayout.BeginScrollView(targetsSVPos);

                EditorGUILayout.BeginVertical();

                for (int i = 0; i < targets.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    SkyerPlatform wPlatform = targets[i];

                    if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                        targets.RemoveAt(i);

                    string subtargetName = "";

                    if (wPlatform.Subtarget != SkyerSubtarget.None)
                        subtargetName = $" ({wPlatform.Subtarget})";

                    EditorGUILayout.LabelField($"{wPlatform.Target}{subtargetName}");

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            if (startBuild)
                StartBuild();

            EditorUtility.ClearProgressBar();
        }

        private string[] GetExtraSubtargets(SkyerTarget target, SkyerSubtarget[] subtargets)
        {
            string defaultSubtargetName = SkyerUtils.GetDefaultSubtargetName(target);

            if (subtargets.Length <= 1)
                return new string[] { defaultSubtargetName };

            List<string> subtargetNames = new List<string>();

            subtargetNames.Add(defaultSubtargetName);

            for (int i = 1; i < subtargets.Length; i++)
                subtargetNames.Add(Enum.GetName(typeof(SkyerSubtarget), subtargets[i]));

            return subtargetNames.ToArray();
        }

        private void StartBuild()
        {
            int waitTime = 1750;
            int clocks = 144;
            int errors = 0;

            foreach (SkyerPlatform platform in targets)
            {
                string subtargetName = "";

                if (platform.Subtarget != SkyerSubtarget.None)
                    subtargetName = $"-{platform.Subtarget}";

                string platformName = $"{platform.Target}{subtargetName}";

                for (int i = 0; i < clocks; i++)
                {
                    if (EditorUtility.DisplayCancelableProgressBar("Skyer Builder", $"Preparing for building '{platformName}'...", (float)i / clocks))
                    {
                        EditorUtility.ClearProgressBar();
                        return;
                    }

                    Thread.Sleep(waitTime / clocks);
                }

                string platformPath = Path.Combine(outputPath, platformName);

                Directory.CreateDirectory(platformPath);

                BuildPlayerOptions options = SkyerUtils.ToBuildPlayerOptions(platform, platformPath, PlayerSettings.productName);

                List<string> scenes = new List<string>();

                foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
                    if (scene.enabled) scenes.Add(scene.path);

                options.scenes = scenes.ToArray();

                if (BuildPipeline.BuildPlayer(options).summary.result != BuildResult.Succeeded)
                    errors++;
            }

            for (int i = 0; i < clocks; i++)
            {
                EditorUtility.DisplayProgressBar("Skyer Builder", $"Compiled with {errors} possible errors...", (float)i / clocks);

                Thread.Sleep(waitTime / clocks);
            }
        }
    }
}
