namespace DeerSoftware.SkyerBuilder
{
    /// <summary>
    /// Utilities to Skyer Builder like the target conversion.
    /// </summary>
    public static class SkyerUtils
    {
        /// <summary>
        /// Get the default subtarget name, like Player for Standalone or Generic for WebGL and Android.
        /// </summary>
        /// <param name="target">Target to get the default subtarget name.</param>
        /// <returns>Default subtarget name for the selected target.</returns>
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
    }
}
