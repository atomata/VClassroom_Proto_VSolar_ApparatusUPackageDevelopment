namespace HexUN.Data
{
    /// <summary>
    /// Manages path to a central folder used to store configuration files.
    /// </summary>
    public static class Files
    {
        /// <summary>
        /// Constructs path to common file based on type
        /// </summary>
        public static UnityPath GetCommonFile(ECommonFile file)
        {
            return UTECommonFile.GetFilePath(file);
        }
    }
}