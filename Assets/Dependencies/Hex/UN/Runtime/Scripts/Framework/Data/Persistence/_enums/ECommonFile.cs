namespace HexUN.Data
{
    /// <summary>
    /// Used to organize files of various common types. 
    /// can be used as folder names
    /// </summary>
    public enum ECommonFile : byte
    {
        /// <summary>
        /// Log files
        /// </summary>
        Logs = 0,

        /// <summary>
        /// Config files
        /// </summary>
        Configs = 1,

        /// <summary>
        /// Save files
        /// </summary>
        Saves = 2,

        /// <summary>
        /// Data fiels used in the game and stored
        /// </summary>
        Data = 3
    }

    public static class UTECommonFile
    {
        private const string cLogsFileName = "Logs";
        private const string cConfigFileName = "Config";
        private const string cSavesFileName = "Saves";
        private const string cDataFileName = "Data";


        /// <summary>
        /// Gets a PathString 
        /// </summary>
        public static UnityPath GetFilePath(ECommonFile gfl)
        {
            switch (gfl)
            {
                case ECommonFile.Logs:
                    return Folders.GetPath(ECommonFolder.Project).Path.InsertAtEnd(cLogsFileName);
                case ECommonFile.Configs:
                    return Folders.GetPath(ECommonFolder.Assets).Path.InsertAtEnd(cConfigFileName);
                case ECommonFile.Saves:
                    return Folders.GetPath(ECommonFolder.UserData).Path.InsertAtEnd(cSavesFileName);
                case ECommonFile.Data:
                    return Folders.GetPath(ECommonFolder.RuntimeFiles).Path.InsertAtEnd(cDataFileName);
            }

            return null;
        }
    }
}