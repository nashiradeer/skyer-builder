namespace DeerSoftware.SkyerBuilder
{
    /// <summary>
    /// All targets supported by Skyer Builder.
    /// </summary>
    public enum SkyerTarget : int
    {
        /// <summary>
        /// Unknown target. (Default)
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Windows 32-bits.
        /// </summary>
        Windows32 = 1,

        /// <summary>
        /// Windows 64-bits.
        /// </summary>
        Windows = 2,

        /// <summary>
        /// Linux 64-bits.
        /// </summary>
        Linux = 3,

        /// <summary>
        /// Mac OS X, can be Intel-64, ARM64, or both, but you need to set this in the Build Settings Window.
        /// </summary>
        Mac = 4,

        /// <summary>
        /// WebGL.
        /// </summary>
        WebGL = 5,

        /// <summary>
        /// Android.
        /// </summary>
        Android = 6,

        /// <summary>
        /// iOS.
        /// </summary>
        iOS = 7,

        /// <summary>
        /// Xbox One.
        /// </summary>
        XboxOne = 8,

        /// <summary>
        /// PlayStation 4.
        /// </summary>
        PS4 = 9,

        /// <summary>
        /// PlayStation 5.
        /// </summary>
        PS5 = 10,

        /// <summary>
        /// Nintendo Switch.
        /// </summary>
        Switch = 11,

        /// <summary>
        /// Google Stadia.
        /// </summary>
        Stadia = 12,

        /// <summary>
        /// Windows Store Apps.
        /// </summary>
        WSA = 13,

        /// <summary>
        /// Linux Headless Simulation.
        /// </summary>
        LinuxHeadlessSimulation = 14,

        /// <summary>
        /// Apple tvOS.
        /// </summary>
        tvOS = 15,

#if UNITY_2022_1_OR_NEWER
        /// <summary>
        /// Apple visionOS.
        /// </summary>
        VisionOS = 16,
#endif
    }
}
