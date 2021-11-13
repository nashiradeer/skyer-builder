using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace NashiraDeer.SkyerBuilder
{
    /// <summary>
    /// A step to be followed by the <see cref="SkyerEngine"/>.
    /// </summary>
    public class SkyerBuildStep
    {
        /// <summary>
        /// Platform target for this step.
        /// </summary>
        public SkyerTarget Target = SkyerTarget.Unknown;

        /// <summary>
        /// <see cref="BuildOptions"/> used in this step.
        /// </summary>
        public BuildOptions Options = BuildOptions.None;

        /// <summary>
        /// Initialize a Skyer Build Step with target and options defined.
        /// </summary>
        /// <param name="target">Skyer Target used by this step.</param>
        /// <param name="options">Unity Build Options used by this step.</param>
        public SkyerBuildStep(SkyerTarget target, BuildOptions options)
        {
            Target = target;
            Options = options;
        }
    }

    /// <summary>
    /// A single build report from <see cref="SkyerEngine"/>.
    /// </summary>
    public class SkyerBuildReport
    {
        /// <summary>
        /// Step executed during the build.
        /// </summary>
        public SkyerBuildStep Step = null;

        /// <summary>
        /// Report returned by the Unity Build Pipeline.
        /// </summary>
        public BuildReport Report = null;
    }

    /// <summary>
    /// Platform targets used by the <see cref="SkyerEngine"/> and <see cref="SkyerSettings"/>, represents a simplified and crossversion target from <see cref="BuildTarget"/> and <see cref="BuildTargetGroup"/>.
    /// </summary>
    public enum SkyerTarget : int
    {
        /// <summary>
        /// Default value of <see cref="SkyerTarget"/>, doesn't represent any platform.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents the Standalone Player for Windows 64-bits.
        /// </summary>
        Windows = 1,

        /// <summary>
        /// Represents the Standalone Player for Windows 32-bits.
        /// </summary>
        Windows32 = 2,

        /// <summary>
        /// Represents the Standalone Player for Linux 64-bits.
        /// </summary>
        Linux = 4,

        /// <summary>
        /// Represents the Standalone Player for Linux 32-bits.
        /// </summary>
        Linux32 = 8,

        /// <summary>
        /// Represents the Standalone Player for Linux 32-bits and 64-bits.
        /// </summary>
        LinuxUniversal = 16,

        /// <summary>
        /// Represents the Standalone Player for MacOS Intel.
        /// </summary>
        Mac = 32,

        /// <summary>
        /// Represents the WebGL Player.
        /// </summary>
        WebGL = 64,

        /// <summary>
        /// Represents the Universal Windows Platform Player.
        /// </summary>
        WSA = 128,

        /// <summary>
        /// Represents the Android Player.
        /// </summary>
        Android = 256,

        /// <summary>
        /// Represents the iOS Player.
        /// </summary>
        iOS = 512,

        /// <summary>
        /// Represents the PlayStation 4 Player.
        /// </summary>
        PS4 = 1024,

        /// <summary>
        /// Represents the PlayStation 5 Player.
        /// </summary>
        PS5 = 2048,

        /// <summary>
        /// Represents the Xbox One Player.
        /// </summary>
        XboxOne = 4096,

        /// <summary>
        /// Represents the Nintendo Switch Player.
        /// </summary>
        Switch = 8192,

        /// <summary>
        /// Represents the Google Stadia Player.
        /// </summary>
        Stadia = 16384,

        /// <summary>
        /// Represents the tvOS Player.
        /// </summary>
        tvOS = 32768,

        /// <summary>
        /// Represents the CloudRendering Player.
        /// </summary>
        CloudRendering = 65536
    }

    /// <summary>
    /// The build engine used by the Skyer Builder.
    /// </summary>
    public class SkyerEngine
    {
        /// <summary>
        /// Resolve the <see cref="SkyerTarget"/> to a <see cref="BuildTarget"/> to be used in <see cref="BuildPipeline"/>.
        /// </summary>
        /// <param name="skyerTarget">Platform Target from Skyer Builder.</param>
        /// <returns>Build Target used the Unity Build Pipeline.</returns>
        public static BuildTarget GetTarget(SkyerTarget skyerTarget)
        {
            switch (skyerTarget)
            {
                case SkyerTarget.Windows: return BuildTarget.StandaloneWindows64;
                case SkyerTarget.Windows32: return BuildTarget.StandaloneWindows;
                case SkyerTarget.Linux: return BuildTarget.StandaloneLinux64;
                case SkyerTarget.Mac: return BuildTarget.StandaloneOSX;
                case SkyerTarget.PS4: return BuildTarget.PS4;
                case SkyerTarget.PS5: return BuildTarget.PS5;
                case SkyerTarget.Stadia: return BuildTarget.Stadia;
                case SkyerTarget.XboxOne: return BuildTarget.XboxOne;
                case SkyerTarget.Switch: return BuildTarget.Switch;
                case SkyerTarget.tvOS: return BuildTarget.tvOS;
                case SkyerTarget.WebGL: return BuildTarget.WebGL;
                case SkyerTarget.WSA: return BuildTarget.WSAPlayer;
                case SkyerTarget.CloudRendering: return BuildTarget.CloudRendering;
                case SkyerTarget.Android: return BuildTarget.Android;
                case SkyerTarget.iOS: return BuildTarget.iOS;
                default: throw new NotSupportedException("Can't get Unity Target for a invalid or not supported Skyer Target");
            }
        }

        /// <summary>
        /// Resolve the <see cref="SkyerTarget"/> to a <see cref="BuildTargetGroup"/> to be used in <see cref="BuildPipeline"/>.
        /// </summary>
        /// <param name="skyerTarget">Platform Target from Skyer Builder.</param>
        /// <returns>Build Target Group used the Unity Build Pipeline.</returns>
        public static BuildTargetGroup GetTargetGroup(SkyerTarget skyerTarget)
        {
            switch(skyerTarget)
            {
                case SkyerTarget.Windows:
                case SkyerTarget.Windows32:
                case SkyerTarget.Linux:
                case SkyerTarget.Mac: return BuildTargetGroup.Standalone;
                case SkyerTarget.PS4: return BuildTargetGroup.PS4;
                case SkyerTarget.PS5: return BuildTargetGroup.PS5;
                case SkyerTarget.Stadia: return BuildTargetGroup.Stadia;
                case SkyerTarget.XboxOne: return BuildTargetGroup.XboxOne;
                case SkyerTarget.Switch: return BuildTargetGroup.Switch;
                case SkyerTarget.tvOS: return BuildTargetGroup.tvOS;
                case SkyerTarget.WebGL: return BuildTargetGroup.WebGL;
                case SkyerTarget.WSA: return BuildTargetGroup.WSA;
                case SkyerTarget.CloudRendering: return BuildTargetGroup.CloudRendering;
                case SkyerTarget.Android: return BuildTargetGroup.Android;
                case SkyerTarget.iOS: return BuildTargetGroup.iOS;
                default: throw new NotSupportedException("Can't get Unity Target Group for a invalid or not supported Skyer Target");
            }
        }

        /// <summary>
        /// Directory path for putting the players builded by this engine.
        /// </summary>
        public string BuildPath { get; set; }

        /// <summary>
        /// Initialize a <see cref="SkyerEngine"/> with a custom resolver.
        /// </summary>
        /// <param name="buildpath">Directory path for putting the players builded by this engine.</param>
        /// <param name="resolver">A custom target resolver.</param>
        public SkyerEngine(string buildpath)
        {
            BuildPath = buildpath;
        }

        /// <summary>
        /// Starts the build engine, building the steps specified in <paramref name="steps"/> using the scenes specified in <paramref name="scenes"/>.
        /// </summary>
        /// <param name="steps">Steps for the build.</param>
        /// <param name="scenes">Scenes to be added in the builds.</param>
        /// <returns>All <see cref="BuildReport"/> returned by the Unity BuildPipeline.</returns>
        public SkyerBuildReport[] Build(SkyerBuildStep[] steps, string[] scenes, Func<SkyerBuildReport, int, int, bool> progressCb)
        {
            int finalized = 0;

            List<SkyerBuildReport> result = new List<SkyerBuildReport>();

            foreach (SkyerBuildStep step in steps)
            {
                BuildPlayerOptions options = new BuildPlayerOptions();
                options.scenes = scenes;
                options.target = GetTarget(step.Target);
                options.targetGroup = GetTargetGroup(step.Target);
                options.options = step.Options;
                options.locationPathName = Path.Combine(BuildPath, step.Target.ToString());

                Directory.CreateDirectory(options.locationPathName);

                if (step.Target == SkyerTarget.Windows || step.Target == SkyerTarget.Windows32) options.locationPathName = Path.Combine(options.locationPathName, PlayerSettings.productName + ".exe");
                if (step.Target == SkyerTarget.Mac) options.locationPathName = Path.Combine(options.locationPathName, PlayerSettings.productName);
                if (step.Target == SkyerTarget.Linux) options.locationPathName = Path.Combine(options.locationPathName, PlayerSettings.productName.Replace(" ", "") + ".x86_64");
                if (step.Target == SkyerTarget.Android) options.locationPathName = Path.Combine(options.locationPathName, PlayerSettings.productName + ".apk");

                SkyerBuildReport buildReport = new SkyerBuildReport();
                buildReport.Step = step;
                buildReport.Report = BuildPipeline.BuildPlayer(options);

                result.Add(buildReport);

                finalized++;

                if (progressCb != null)
                {
                    if (progressCb(buildReport, finalized, steps.Length)) break;
                }
            }

            return result.ToArray();
        }
    }
}
