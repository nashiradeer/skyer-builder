using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace NashiraDeer.SkyerBuilder.Test
{
    public class PackageCompiler : EditorWindow
    {
        [MenuItem("Assets/Compile Package")]
        public static void CompilePackage()
        {
            Client.Pack("Assets/SkyerBuilder", ".");
        }
    }
}
