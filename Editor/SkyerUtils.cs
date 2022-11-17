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
    }
}
