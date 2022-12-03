using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DeerSoftware.SkyerBuilder
{
    /// <summary>
    /// Unity Editor command line handler for Skyer Builder.
    /// </summary>
    public static class SkyerCommand
    {
        /// <summary>
        /// Enum created to track the command args parsing.
        /// </summary>
        private enum SkyerCommandParsing : int
        {
            /// <summary>
            /// Waiting a option argument.
            /// </summary>
            Waiting = 0,

            /// <summary>
            /// Waiting the option to set the platform.
            /// </summary>
            Platform = 1,

            /// <summary>
            /// Waiting the option to set the location path.
            /// </summary>
            Location = 2,

            /// <summary>
            /// Waiting the option to set the name.
            /// </summary>
            Name = 3,
        }


        /// <summary>
        /// Command to be run using the command line of Unity Editor.
        /// </summary>
        public static void Run()
        {
            // Store the parameters from command line.
            List<SkyerPlatform> platforms = new List<SkyerPlatform>();
            string locationPath = "";
            string name = "";

            // Parse the command line.
            SkyerCommandParsing state = SkyerCommandParsing.Waiting;
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                Debug.Log($"SkyerBuilder: Parsing '{arg}'...");
                switch (state)
                {
                    case SkyerCommandParsing.Waiting:
                        // Detect the next argument type.
                        switch (arg)
                        {
                            case "-skyerPlatform":
                                state = SkyerCommandParsing.Platform;
                                break;

                            case "-skyerLocation":
                                state = SkyerCommandParsing.Location;
                                break;

                            case "-skyerName":
                                state = SkyerCommandParsing.Name;
                                break;
                        }
                        break;

                    case SkyerCommandParsing.Platform:
                        // Parse the platform and add to multibuild.
                        string[] platformArgs = arg.Split(",");

                        if (platformArgs.Length >= 1)
                        {
                            try
                            {
                                SkyerTarget target = Enum.Parse<SkyerTarget>(platformArgs[0], true);

                                if (target == SkyerTarget.Unknown)
                                {
                                    // Enum parse from target has fail.
                                    Debug.LogError("SkyerBuilder: Target is Unknown");
                                    EditorApplication.Exit(1);
                                    return;
                                }

                                SkyerSubtarget subtarget = SkyerSubtarget.None;

                                try
                                {
                                    // If has sub-target parse it too.
                                    if (platformArgs.Length >= 2)
                                        subtarget = Enum.Parse<SkyerSubtarget>(platformArgs[1], true);

                                    if (!SkyerUtils.GetSkyerSubtargets(target).Contains(subtarget))
                                    {
                                        // Subtarget aren't supported by the target.
                                        Debug.LogError("SkyerBuilder: Subtarget not supported by the target");
                                        EditorApplication.Exit(3);
                                        return;
                                    }

                                    // Add platform to be built if isn't added.
                                    SkyerPlatform platform = new SkyerPlatform()
                                    {
                                        Target = target,
                                        Subtarget = subtarget,
                                    };
                                    Debug.Log($"SkyerBuilder: Adding platform '{target},{subtarget}'...");

                                    if (!platforms.Contains(platform))
                                        platforms.Add(platform);
                                }
                                catch (ArgumentException e)
                                {
                                    // Enum parse from sub-target has fail.
                                    Debug.LogError("SkyerBuilder: Subtarget not found");
                                    Debug.LogError(e);
                                    EditorApplication.Exit(2);
                                    return;
                                }
                            }
                            catch (ArgumentException e)
                            {
                                // Enum parse from target has fail.
                                Debug.LogError("SkyerBuilder: Target not found");
                                Debug.LogError(e);
                                EditorApplication.Exit(1);
                                return;
                            }
                        }
                        state = SkyerCommandParsing.Waiting;
                        break;

                    case SkyerCommandParsing.Location:
                        // Set location.
                        locationPath = arg;
                        Debug.Log($"SkyerBuilder: Current location path: {locationPath}...");
                        state = SkyerCommandParsing.Waiting;
                        break;

                    case SkyerCommandParsing.Name:
                        // Set name.
                        name = arg;
                        Debug.Log($"SkyerBuilder: Current location path: {name}...");
                        state = SkyerCommandParsing.Waiting;
                        break;
                }
            }

            // Errors number increased for each platform that is cancelled or errored.
            int errors = 0;

            for (int i = 0; i < platforms.Count; i++)
            {
                SkyerPlatform platform = platforms[i];

                // Generates the sub-target if isn't 'None'.
                string subtargetName = "";

                if (platform.Subtarget != SkyerSubtarget.None)
                    subtargetName = $"-{platform.Subtarget}";

                string platformName = $"{platform.Target}{subtargetName}";


                Debug.Log($"SkyerBuilder: Preparing to build: {platformName}...");

                // Joins the output path with the platform name.
                string platformPath = Path.Combine(locationPath, platformName);

                // Ensures that the platform path exists.
                Directory.CreateDirectory(platformPath);

                // Generates the Build Player Options for the platform using Skyer Utils.
                BuildPlayerOptions options = SkyerUtils.ToBuildPlayerOptions(platform, platformPath, PlayerSettings.productName);

                // Catch the enabled scenes from Editor Build Settings.
                List<string> scenes = new List<string>();

                for (int o = 0; o < EditorBuildSettings.scenes.Length; o++)
                    if (EditorBuildSettings.scenes[o].enabled) scenes.Add(EditorBuildSettings.scenes[o].path);

                options.scenes = scenes.ToArray();

                // Starts the Build Pipeline.
                Debug.Log($"SkyerBuilder: Building: {platformName}...");
                BuildReport report = BuildPipeline.BuildPlayer(options); ;

                if (report.summary.result == BuildResult.Cancelled)
                {
                    Debug.LogError($"SkyerBuilder: Build cancelled: {platformName}");
                    EditorApplication.Exit(4);
                    return;
                }
                else if (report.summary.result != BuildResult.Succeeded)
                {
                    Debug.LogError($"SkyerBuilder: Error detected in: {platformName}");
                    errors++;
                }
            }

            // Show the result of the build.
            Debug.Log($"SkyerBuilder: Compiled with {errors} errors");
        }
    }
}
