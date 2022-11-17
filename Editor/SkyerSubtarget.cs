namespace DeerSoftware.SkyerBuilder
{
    /// <summary>
    /// All subtargets supported by Skyer Builder.
    /// </summary>
    public enum SkyerSubtarget : int
    {
        /// <summary>
        /// None, can be Player (Standalone), Generic (texture compression; WebGL, Android), Development (Xbox One), and PCHosted (PlayStation 4).
        /// </summary>
        None = 0,

        /// <summary>
        /// Standalone Server subtarget.
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
        /// Xbox One subtarget, Master.
        /// </summary>
        Master = 7,

        /// <summary>
        /// Xbox One subtarget, Debug.
        /// </summary>
        Debug = 8,

        /// <summary>
        /// PlayStation 4 subtarget, Package.
        /// </summary>
        Package = 9,
    }
}
