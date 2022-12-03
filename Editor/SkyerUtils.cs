using System;
using System.IO;
using UnityEditor;

namespace DeerSoftware.SkyerBuilder
{
    /// <summary>
    /// Utilities to Skyer Builder like the target conversion.
    /// </summary>
    public static class SkyerUtils
    {
        /// <summary>
        /// Get the default sub-target name, like Player for Standalone or Generic for WebGL and Android.
        /// </summary>
        /// <param name="target">Target to get the default sub-target name.</param>
        /// <returns>Default sub-target name for the selected target.</returns>
        public static string GetDefaultSubtargetName(SkyerTarget target)
        {
            switch (target)
            {
                case SkyerTarget.Windows:
                case SkyerTarget.Windows32:
                case SkyerTarget.Linux:
                case SkyerTarget.Mac:
                    return "Player";

                case SkyerTarget.WebGL:
                case SkyerTarget.Android:
                    return "Generic";

                case SkyerTarget.XboxOne:
                    return "Development";

                case SkyerTarget.PS4:
                    return "PCHosted";

                default:
                    return "None";
            }
        }

        /// <summary>
        /// Get all sub-targets supported by a target.
        /// </summary>
        /// <param name="target">Target to check supported sub-targets.</param>
        /// <returns>All sub-targets supported by the target.</returns>
        public static SkyerSubtarget[] GetSkyerSubtargets(SkyerTarget target)
        {
            switch (target)
            {
                case SkyerTarget.Windows:
                case SkyerTarget.Windows32:
                case SkyerTarget.Linux:
                case SkyerTarget.Mac:
                    return new SkyerSubtarget[]
                    {
                        SkyerSubtarget.None,
                        SkyerSubtarget.Server,
                    };

                case SkyerTarget.WebGL:
                    return new SkyerSubtarget[]
                    {
                        SkyerSubtarget.None,
                        SkyerSubtarget.DXT,
                        SkyerSubtarget.ETC2,
                        SkyerSubtarget.ASTC,
                    };

                case SkyerTarget.Android:
                    return new SkyerSubtarget[]
                    {
                        SkyerSubtarget.None,
                        SkyerSubtarget.DXT,
                        SkyerSubtarget.ETC,
                        SkyerSubtarget.ETC2,
                        SkyerSubtarget.ASTC,
                        SkyerSubtarget.PVRTC,
                    };

                case SkyerTarget.XboxOne:
                    return new SkyerSubtarget[]
                    {
                        SkyerSubtarget.None,
                        SkyerSubtarget.Master,
                        SkyerSubtarget.Debug,
                    };

                case SkyerTarget.PS4:
                    return new SkyerSubtarget[]
                    {
                        SkyerSubtarget.None,
                        SkyerSubtarget.Package,
                    };

                default:
                    return new SkyerSubtarget[]
                    {
                        SkyerSubtarget.None,
                    };
            }
        }

        /// <summary>
        /// Converts a Skyer sub-target and target to a sub-target int to be used in Unity Build Pipeline.
        /// </summary>
        /// <param name="target">Target used as a sub-target filter.</param>
        /// <param name="subtarget">Sub-target to be converted.</param>
        /// <returns>Sub-target compatible with the Unity Build Pipeline.</returns>
        /// <exception cref="NotSupportedException">Throwed when a incompatible combination of <paramref name="target"/> and <paramref name="subtarget"/> is used.</exception>
        public static int FromSkyerSubtarget(SkyerTarget target, SkyerSubtarget subtarget)
        {
            switch (subtarget)
            {
                case SkyerSubtarget.None:
                    return 0;

                case SkyerSubtarget.Server:
                    if (target == SkyerTarget.Windows || target == SkyerTarget.Windows32 || target == SkyerTarget.Linux || target == SkyerTarget.Mac)
                        return (int)StandaloneBuildSubtarget.Server;
                    else
                        throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");

                case SkyerSubtarget.DXT:
                    if (target == SkyerTarget.WebGL)
                        return (int)WebGLTextureSubtarget.DXT;
                    else if (target == SkyerTarget.Android)
                        return (int)MobileTextureSubtarget.DXT;
                    else
                        throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");

                case SkyerSubtarget.ETC2:
                    if (target == SkyerTarget.WebGL)
                        return (int)WebGLTextureSubtarget.ETC2;
                    else if (target == SkyerTarget.Android)
                        return (int)MobileTextureSubtarget.ETC2;
                    else
                        throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");

                case SkyerSubtarget.ASTC:
                    if (target == SkyerTarget.WebGL)
                        return (int)WebGLTextureSubtarget.ASTC;
                    else if (target == SkyerTarget.Android)
                        return (int)MobileTextureSubtarget.ASTC;
                    else
                        throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");

                case SkyerSubtarget.ETC:
                    if (target == SkyerTarget.Android)
                        return (int)MobileTextureSubtarget.ETC;
                    else
                        throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");

                case SkyerSubtarget.PVRTC:
                    if (target == SkyerTarget.Android)
                        return (int)MobileTextureSubtarget.PVRTC;
                    else
                        throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");

                case SkyerSubtarget.Master:
                    if (target == SkyerTarget.XboxOne)
                        return (int)XboxBuildSubtarget.Master;
                    else
                        throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");

                case SkyerSubtarget.Debug:
                    if (target == SkyerTarget.XboxOne)
                        return (int)XboxBuildSubtarget.Debug;
                    else
                        throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");

                case SkyerSubtarget.Package:
                    if (target == SkyerTarget.PS4)
                        return (int)PS4BuildSubtarget.Package;
                    else
                        throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");

                default:
                    throw new NotSupportedException("Can't found a sub-target int for a target and/or subtarget");
            }
        }

        /// <summary>
        /// Converts a Skyer target to a Target compatible with Unity.
        /// </summary>
        /// <param name="target">Target to be converted.</param>
        /// <returns>Target compatible with the Unity Build Pipeline.</returns>
        /// <exception cref="NotSupportedException">Throwed when a invalid <paramref name="target"/> is used.</exception>
        public static BuildTarget ToBuildTarget(SkyerTarget target)
        {
            switch (target)
            {
                case SkyerTarget.Windows32: return BuildTarget.StandaloneWindows;
                case SkyerTarget.Windows: return BuildTarget.StandaloneWindows64;
                case SkyerTarget.Linux: return BuildTarget.StandaloneLinux64;
                case SkyerTarget.Mac: return BuildTarget.StandaloneOSX;
                case SkyerTarget.WebGL: return BuildTarget.WebGL;
                case SkyerTarget.Android: return BuildTarget.Android;
                case SkyerTarget.iOS: return BuildTarget.iOS;
                case SkyerTarget.XboxOne: return BuildTarget.XboxOne;
                case SkyerTarget.PS4: return BuildTarget.PS4;
                case SkyerTarget.PS5: return BuildTarget.PS5;
                case SkyerTarget.Switch: return BuildTarget.Switch;
                case SkyerTarget.Stadia: return BuildTarget.Stadia;
                case SkyerTarget.WSA: return BuildTarget.WSAPlayer;
                case SkyerTarget.LinuxHeadlessSimulation: return BuildTarget.LinuxHeadlessSimulation;
                default: throw new NotSupportedException("Can't convert from Skyer Target to Build Target");
            }
        }

        /// <summary>
        /// Converts a Skyer target to a Target Group compatible with Unity.
        /// </summary>
        /// <param name="target">Target to be converted.</param>
        /// <returns>Target Group compatible with the Unity Build Pipeline.</returns>
        /// <exception cref="NotSupportedException">Throwed when a invalid <paramref name="target"/> is used.</exception>
        public static BuildTargetGroup ToBuildTargetGroup(SkyerTarget target)
        {
            switch (target)
            {
                case SkyerTarget.Windows32:
                case SkyerTarget.Windows:
                case SkyerTarget.Linux:
                case SkyerTarget.Mac: return BuildTargetGroup.Standalone;
                case SkyerTarget.WebGL: return BuildTargetGroup.WebGL;
                case SkyerTarget.Android: return BuildTargetGroup.Android;
                case SkyerTarget.iOS: return BuildTargetGroup.iOS;
                case SkyerTarget.XboxOne: return BuildTargetGroup.XboxOne;
                case SkyerTarget.PS4: return BuildTargetGroup.PS4;
                case SkyerTarget.PS5: return BuildTargetGroup.PS5;
                case SkyerTarget.Switch: return BuildTargetGroup.Switch;
                case SkyerTarget.Stadia: return BuildTargetGroup.Stadia;
                case SkyerTarget.WSA: return BuildTargetGroup.WSA;
                case SkyerTarget.LinuxHeadlessSimulation: return BuildTargetGroup.LinuxHeadlessSimulation;
                default: throw new NotSupportedException("Can't convert from Skyer Target to Build Target");
            }
        }

        /// <summary>
        /// Modify the <paramref name="path"/> to be compatible with the location path required for determined target.
        /// </summary>
        /// <param name="target">Target used to detect the necessary modifications.</param>
        /// <param name="path">Output path to place the compiled files.</param>
        /// <param name="name">Product or binary name for some targets.</param>
        /// <returns>Location path ready to be used.</returns>
        public static string FixLocationPath(SkyerTarget target, string path, string name)
        {
            switch (target)
            {
                case SkyerTarget.Windows:
                case SkyerTarget.Windows32:
                    return Path.Combine(path, name.Trim() + ".exe");
                case SkyerTarget.Mac:
                    return Path.Combine(path, name);
                case SkyerTarget.Linux:
                    return Path.Combine(path, name.Replace(" ", "") + ".x86_64");
                case SkyerTarget.Android:
                    return Path.Combine(path, name + ".apk");
                case SkyerTarget.WebGL:
                    return Path.Combine(path, name);
                default:
                    return path;
            }
        }

        /// <summary>
        /// Generates a Build Player Options from Skyer.
        /// </summary>
        /// <param name="platform">Platform to generate the Build Player Options.</param>
        /// <param name="path">Output path to place the compiled files.</param>
        /// <param name="name">Product or binary name for some targets.</param>
        /// <returns>Generated Build Player Options, you still need to set scenes or anything you want in the returned Build Player Options.</returns>
        public static BuildPlayerOptions ToBuildPlayerOptions(SkyerPlatform platform, string path, string name)
        {
            return new BuildPlayerOptions()
            {
                locationPathName = FixLocationPath(platform.Target, path, name),
                target = ToBuildTarget(platform.Target),
                targetGroup = ToBuildTargetGroup(platform.Target),
                subtarget = FromSkyerSubtarget(platform.Target, platform.Subtarget),
            };
        }
    }
}
