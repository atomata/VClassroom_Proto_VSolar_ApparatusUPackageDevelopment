namespace HexUN.Data
{
    /// <summary>
    /// Manages path to a central folder used to store configuration files.
    /// </summary>
    public static class Folders
    {
        /// <summary>
        /// Returns a path to a common folder
        /// </summary>
        public static UnityPath GetPath(ECommonFolder location)
        {
            return UTECommonFolder.GetFolderPath(location);
        }
    }
}