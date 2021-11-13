using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NashiraDeer.SkyerBuilder
{
    public class SkyerWindow : EditorWindow
    {
        public string BuildPath = "";
        public bool UseVersion = false;

        public bool Windows = false;
        public bool Windows32 = false;
        public bool Linux = false;
        public bool MacOSX = false;
        public bool Android = false;
        public bool iOS = false;
        public bool WebGL = false;
        public bool WSA = false;
        public bool PS4 = false;
        public bool PS5 = false;
        public bool XboxOne = false;
        public bool Switch = false;
        public bool Stadia = false;
        public bool tvOS = false;
        public bool CloudRendenring = false;

        public bool DevelopmentBuild = false;

        [MenuItem("Window/Skyer Builder")]
        public static void ShowWindow()
        {
            SkyerWindow window = GetWindow<SkyerWindow>();
            window.titleContent = new GUIContent("Skyer Builder");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Engine Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Build Path", GUILayout.ExpandWidth(false));
            BuildPath = GUILayout.TextField(BuildPath, GUILayout.MinWidth(150));
            if (GUILayout.Button("...", GUILayout.ExpandWidth(false)))
            {
                BuildPath = EditorUtility.SaveFolderPanel("Build Path", BuildPath, PlayerSettings.productName);
            }
            EditorGUILayout.EndHorizontal();
            UseVersion = GUILayout.Toggle(UseVersion, "Use Version");

            GUILayout.Space(10);
            GUILayout.Label("Platform Targets", EditorStyles.boldLabel);

            Windows = GUILayout.Toggle(Windows, "Windows 64-bit");
            Windows32 = GUILayout.Toggle(Windows32, "Windows 32-bit");
            Linux = GUILayout.Toggle(Linux, "Linux 64-bit");
            MacOSX = GUILayout.Toggle(MacOSX, "MacOSX (Intel)");
            Android = GUILayout.Toggle(Android, "Android");
            iOS = GUILayout.Toggle(iOS, "iOS");
            WebGL = GUILayout.Toggle(WebGL, "WebGL");
            WSA = GUILayout.Toggle(WSA, "WSA");
            PS4 = GUILayout.Toggle(PS4, "PS4");
            PS5 = GUILayout.Toggle(PS5, "PS5");
            XboxOne = GUILayout.Toggle(XboxOne, "Xbox One");
            Switch = GUILayout.Toggle(Switch, "Switch");
            Stadia = GUILayout.Toggle(Stadia, "Stadia");
            tvOS = GUILayout.Toggle(tvOS, "tvOS");
            CloudRendenring = GUILayout.Toggle(CloudRendenring, "Cloud Rendenring");

            GUILayout.Space(10);
            GUILayout.Label("Build Settings", EditorStyles.boldLabel);

            DevelopmentBuild = GUILayout.Toggle(DevelopmentBuild, "Development Build");

            GUILayout.Space(10);
            GUILayout.Label("Skyer Control Panel", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Build", GUILayout.Width(118))) Build();
            EditorUtility.ClearProgressBar();
        }

        public void Build()
        {
            string buildpath = Path.GetFullPath(BuildPath);

            if (UseVersion) buildpath = Path.Combine(buildpath, Application.version);

            if (EditorUtility.DisplayCancelableProgressBar("Skyer Builder", "Creating options...", 0)) return;
            BuildOptions options = new BuildOptions();

            if (DevelopmentBuild) options |= BuildOptions.Development;

            if (EditorUtility.DisplayCancelableProgressBar("Skyer Builder", "Creating build steps...", 0)) return;

            List<SkyerBuildStep> steps = new List<SkyerBuildStep>();

            if (Windows) steps.Add(new SkyerBuildStep(SkyerTarget.Windows, options));
            if (Windows32) steps.Add(new SkyerBuildStep(SkyerTarget.Windows32, options));
            if (Linux) steps.Add(new SkyerBuildStep(SkyerTarget.Linux, options));
            if (MacOSX) steps.Add(new SkyerBuildStep(SkyerTarget.Mac, options));
            if (Android) steps.Add(new SkyerBuildStep(SkyerTarget.Android, options));
            if (iOS) steps.Add(new SkyerBuildStep(SkyerTarget.iOS, options));
            if (WebGL) steps.Add(new SkyerBuildStep(SkyerTarget.WebGL, options));
            if (WSA) steps.Add(new SkyerBuildStep(SkyerTarget.WSA, options));
            if (PS4) steps.Add(new SkyerBuildStep(SkyerTarget.PS4, options));
            if (PS5) steps.Add(new SkyerBuildStep(SkyerTarget.PS5, options));
            if (XboxOne) steps.Add(new SkyerBuildStep(SkyerTarget.XboxOne, options));
            if (Switch) steps.Add(new SkyerBuildStep(SkyerTarget.Switch, options));
            if (Stadia) steps.Add(new SkyerBuildStep(SkyerTarget.Stadia, options));
            if (tvOS) steps.Add(new SkyerBuildStep(SkyerTarget.tvOS, options));
            if (CloudRendenring) steps.Add(new SkyerBuildStep(SkyerTarget.CloudRendering, options));

            if (EditorUtility.DisplayCancelableProgressBar("Skyer Builder", "Creating scene list...", 0)) return;
            List<string> scenes = new List<string>();

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled) scenes.Add(scene.path);
            }

            if (EditorUtility.DisplayCancelableProgressBar("Skyer Builder", "Initializing engine...", 0)) return;

            SkyerEngine engine = new SkyerEngine(buildpath);

            if (EditorUtility.DisplayCancelableProgressBar("Skyer Builder", "Starting building...", 0)) return;
            engine.Build(steps.ToArray(), scenes.ToArray(), (SkyerBuildReport report, int builded, int building) =>
            {
                Debug.Log("Skyer Builder: Builded Target " + report.Step.Target.ToString() + " with result of " + report.Report.summary.result.ToString() + ".");
                return EditorUtility.DisplayCancelableProgressBar("Skyer Builder", "Building '" + report.Step.Target.ToString() + "'...", (float)builded / building);
            });
        }
    }
}
