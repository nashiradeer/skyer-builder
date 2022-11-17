using System;
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
    }
}
