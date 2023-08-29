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
    /// <summary>
    /// Unity Editor window for Skyer Builder.
    /// </summary>
    public class SkyerWindow : EditorWindow
    {
        // Values from the fields.
        private string outputPath = "";
        private int target = 0;
        private int subtarget = 0;

        // Values used to make foldouts works.
        private bool configFoldout = true;
        private bool addTargetFoldout = true;
        private bool targetsFoldout = true;

        // Values used by Scroll Views.
        private Vector2 targetsSVPos = Vector2.zero;

        // A list containing selected targets.
        private List<SkyerPlatform> targets = new List<SkyerPlatform>();

        /// <summary>
        /// Runs when the object is created, only sets the window title.
        /// </summary>
        private void Awake()
        {
            titleContent = new GUIContent("Skyer Builder");
        }

        /// <summary>
        /// Method runned by Unity when someone clicks in 'Window > Skyer Builder', only initializes the window.
        /// </summary>
        [MenuItem("Window/Skyer Builder")]
        private static void Init()
        {
            GetWindow<SkyerWindow>().Show();
        }

        /// <summary>
        /// On GUI event, used to draw the GUI in the window.
        /// </summary>
        private void OnGUI()
        {
            // Boolean used to activate the build engine in the last line of On GUI event.
            bool startBuild = false;

            // Engine configuration foldout.
            if ((configFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(configFoldout, "Configuration")))
            {
                // Select path field.
                EditorGUILayout.BeginHorizontal();

                outputPath = EditorGUILayout.TextField("Output Path", outputPath);

                if (GUILayout.Button("...", GUILayout.ExpandWidth(false)))
                    outputPath = EditorUtility.SaveFolderPanel("Choosing Output Path...", outputPath, PlayerSettings.productName);

                EditorGUILayout.EndHorizontal();

                // Button that sets true to start build boolean and activate the build engine.
                if (GUILayout.Button("Start Build"))
                    startBuild = true;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            // Add target foldout.
            if ((addTargetFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(addTargetFoldout, "Add Target")))
            {
                // Field for select the target.
                int newTarget = EditorGUILayout.Popup("Target", target, Enum.GetNames(typeof(SkyerTarget)).Skip(1).ToArray());

                // Resets subtarget when a new target is choose to avoid bugs.
                if (newTarget != target)
                    subtarget = 0;

                target = newTarget;

                // Skip the "Unknown" type that isn't showed in window.
                SkyerTarget selectedTarget = (SkyerTarget)target + 1;

                // Field for select sub-targets.
                SkyerSubtarget[] subtargets = SkyerUtils.GetSkyerSubtargets(selectedTarget);

                string[] subtargetOptions = GetExtraSubtargets(selectedTarget, subtargets);

                subtarget = EditorGUILayout.Popup("Sub-target", subtarget, subtargetOptions);

                SkyerSubtarget selectedSubtarget = subtargets[subtarget];

                // Button used to add the selected target and sub-target to the target list.
                if (GUILayout.Button("Add"))
                {
                    SkyerPlatform platform = new SkyerPlatform()
                    {
                        Target = selectedTarget,
                        Subtarget = selectedSubtarget,
                    };

                    // Don't add if already has a target for the selected platform.
                    if (!targets.Contains(platform))
                        targets.Add(platform);
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            // Target list foldout.
            if ((targetsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(targetsFoldout, "Targets")))
            {
                targetsSVPos = EditorGUILayout.BeginScrollView(targetsSVPos);

                EditorGUILayout.BeginVertical();

                // Parses each item in the target list.
                for (int i = 0; i < targets.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    SkyerPlatform wPlatform = targets[i];

                    // Button used to remove this item.
                    if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                        targets.RemoveAt(i);

                    // Generates the sub-target if isn't 'None'.
                    string subtargetName = "";

                    if (wPlatform.Subtarget != SkyerSubtarget.None)
                        subtargetName = $" ({wPlatform.Subtarget})";

                    // Shows the name of the current platform.
                    EditorGUILayout.LabelField($"{wPlatform.Target}{subtargetName}");

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            // Activates the start build if needed.
            if (startBuild)
                StartBuild();

            // Clear any progress bar created in build engine.
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// Extracts the names from the sub-targets supported by a target.
        /// </summary>
        /// <param name="target">Target used to get the default sub-target name.</param>
        /// <param name="subtargets">Sub-target supported by the <paramref name="target"/>.</param>
        /// <returns>The supported sub-types names in a string array.</returns>
        private string[] GetExtraSubtargets(SkyerTarget target, SkyerSubtarget[] subtargets)
        {
            string defaultSubtargetName = SkyerUtils.GetDefaultSubtargetName(target);

            // Only return the default if haven't any other sub-target supported.
            if (subtargets.Length <= 1)
                return new string[] { defaultSubtargetName };

            // Creates a array with the default sub-target and all others sub-targets supported.
            List<string> subtargetNames = new List<string>();

            subtargetNames.Add(defaultSubtargetName);

            for (int i = 1; i < subtargets.Length; i++)
                subtargetNames.Add(Enum.GetName(typeof(SkyerSubtarget), subtargets[i]));

            return subtargetNames.ToArray();
        }

        /// <summary>
        /// Starts the build engine used by Skyer Builder Window.
        /// </summary>
        private void StartBuild()
        {
            // Time waited before a platform if built.
            int waitTime = 1750;
            // Numbers of times that is checked if user has cancelled before a platform built.
            int clocks = 250;
            // Errors number increased for each platform that is cancelled or errored.
            int errors = 0;

            for (int i = 0; i < targets.Count; i++)
            {
                SkyerPlatform platform = targets[i];

                // Generates the sub-target if isn't 'None'.
                string subtargetName = "";

                if (platform.Subtarget != SkyerSubtarget.None)
                    subtargetName = $"-{platform.Subtarget}";

                string platformName = $"{platform.Target}{subtargetName}";

                // Update the progress bar and check if build is cancelled.
                for (int o = 0; o < clocks; o++)
                {
                    if (EditorUtility.DisplayCancelableProgressBar("Skyer Builder", $"Preparing for building '{platformName}'...", (float)o / clocks))
                    {
                        // Cancels the execution of the build engine.
                        EditorUtility.ClearProgressBar();
                        return;
                    }

                    // Make the build engine execution remains until wait time.
                    Thread.Sleep(waitTime / clocks);
                }

                // Joins the output path with the platform name.
                string platformPath = Path.Combine(outputPath, platformName);

                // Ensures that the platform path exists.
                Directory.CreateDirectory(platformPath);

                // Sets Editor User Build Settings.
                SkyerUtils.SetEditorSettings(platform);

                // Generates the Build Player Options for the platform using Skyer Utils.
                BuildPlayerOptions options = SkyerUtils.ToBuildPlayerOptions(platform, platformPath, PlayerSettings.productName);

                // Catch the enabled scenes from Editor Build Settings.
                List<string> scenes = new List<string>();

                for (int o = 0; o < EditorBuildSettings.scenes.Length; o++)
                    if (EditorBuildSettings.scenes[o].enabled) scenes.Add(EditorBuildSettings.scenes[o].path);

                options.scenes = scenes.ToArray();

                // Starts the Build Pipeline.
                BuildResult result = BuildPipeline.BuildPlayer(options).summary.result;

                if (result == BuildResult.Cancelled)
                {
                    // Do the same behavior as cancel before that Build Pipeline is triggered.
                    EditorUtility.ClearProgressBar();
                    return;
                }
                else if (result != BuildResult.Succeeded)
                    errors++;
            }

            // Update the progress bar that shows the errors.
            for (int i = 0; i < clocks; i++)
            {
                EditorUtility.DisplayProgressBar("Skyer Builder", $"Compiled with {errors} possible errors...", (float)i / clocks);

                // Make the build engine execution remains until wait time.
                Thread.Sleep(waitTime / clocks);
            }

            // Open the output directory in Explorer/Finder.
            EditorUtility.RevealInFinder(outputPath);
        }
    }
}
