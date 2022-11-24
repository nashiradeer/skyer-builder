namespace DeerSoftware.SkyerBuilder
{
    /// <summary>
    /// All sub-targets supported by Skyer Builder.
    /// </summary>
    public enum SkyerSubtarget : int
    {
        /// <summary>
        /// None, can be Player (Standalone), Generic (texture compression; WebGL, Android), Development (Xbox One), and PCHosted (PlayStation 4).
        /// </summary>
        None = 0,

        /// <summary>
        /// Standalone Server sub-target.
        /// </summary>
        Server = 1,

        /// <summary>
        /// DXT texture compression. (WebGL, Android)
        /// </summary>
        DXT = 2,

        /// <summary>
        /// ETC texture compression. (WebGL, Android)
        /// </summary>
        ETC = 3,

        /// <summary>
        /// ETC2 texture compression. (WebGL, Android)
        /// </summary>
        ETC2 = 4,

        /// <summary>
        /// ASTC texture compression. (WebGL, Android)
        /// </summary>
        ASTC = 5,

        /// <summary>
        /// PVRTC texture compression. (WebGL, Android)
        /// </summary>
        PVRTC = 6,

        /// <summary>
        /// Xbox One sub-target, Master.
        /// </summary>
        Master = 7,

        /// <summary>
        /// Xbox One sub-target, Debug.
        /// </summary>
        Debug = 8,

        /// <summary>
        /// PlayStation 4 sub-target, Package.
        /// </summary>
        Package = 9,
    }
}
