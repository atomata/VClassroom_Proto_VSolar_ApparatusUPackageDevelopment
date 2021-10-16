using HexCS.Core;
using HexCS.Data.Persistence;

using System;

namespace HexUN.Data
{
    /// <summary>
    /// Enumeration containing label locations that map to common paths in unity projects
    /// </summary>
    public enum ECommonFolder : byte
    {
        /// <summary>
        /// The ProjectName/Assets folder.
        /// </summary>
        Assets = 0,

        /// <summary>
        /// The ProjectName folder
        /// </summary>
        Project = 1,

        /// <summary>
        /// ProjectName/Assets/Config. Used for editor configurations that
        /// need to be stored in the project. Saves for editor menus and what not
        /// </summary>
        Config = 2,

        /// <summary>
        /// The persistentDataPath/_RuntimeFiles used to save runtime files for
        /// the game
        /// </summary>
        RuntimeFiles = 3,

        /// <summary>
        /// Used when saving to the user data folder on a persons computer. This is normally
        /// used at editor time and points to %USERPROFILE% on windows a ~/ on linux/mac
        /// </summary>
        UserData = 4
    }

    public static class UTECommonFolder
    {
        private const string cConfigFolderName = "Config";
        private static UnityPath _configUnityPath = null;
        private static UnityPath ConfigUnityPath
        {
            get
            {
                if(_configUnityPath == null) _configUnityPath = UnityPath.AssetsPath.Path.InsertAtEnd(cConfigFolderName);
                return _configUnityPath;
            }
        }

        private const string cRuntimeFilesFolderName = "_RuntimeFiles";
        private static UnityPath _runtimeFilesUnityPath = null;
        private static UnityPath RuntimeFilesUnityPath
        {
            get
            {
                if (_runtimeFilesUnityPath == null) _runtimeFilesUnityPath = UnityPath.PersistentDataPath.Path.InsertAtEnd(cRuntimeFilesFolderName);
                return _runtimeFilesUnityPath;
            }
        }

        private static UnityPath _userDataUnityPath = null;
        private static UnityPath UserDataUnityPath
        {
            get
            {
                if (_userDataUnityPath == null)
                    _userDataUnityPath = new UnityPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).Path;
                return _userDataUnityPath;
            }
        }

        /// <summary>
        /// Returns the UnityPath to the target folder
        /// </summary>
        public static UnityPath GetFolderPath(ECommonFolder gfl)
        {
            switch (gfl)
            {
                case ECommonFolder.Assets:
                    return UnityPath.AssetsPath;
                case ECommonFolder.Project:
                    return UnityPath.ProjectPath;
                case ECommonFolder.Config:
                    return ConfigUnityPath;
                case ECommonFolder.RuntimeFiles:
                    return RuntimeFilesUnityPath;
                case ECommonFolder.UserData:
                    return UserDataUnityPath;
            }

            return null;
        }
    }
}
