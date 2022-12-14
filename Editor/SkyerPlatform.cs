namespace DeerSoftware.SkyerBuilder
{
    /// <summary>
    /// A combination of a target and sub-target representing a platform.
    /// </summary>
    public struct SkyerPlatform
    {
        /// <summary>
        /// Target of the platform.
        /// </summary>
        public SkyerTarget Target { get; set; }

        /// <summary>
        /// Sub-target of the platform.
        /// </summary>
        public SkyerSubtarget Subtarget { get; set; }
    }
}
