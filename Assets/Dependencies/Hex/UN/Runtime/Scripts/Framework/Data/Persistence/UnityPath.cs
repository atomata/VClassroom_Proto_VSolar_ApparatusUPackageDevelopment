using HexCS.Core;
using UnityEngine;

namespace HexUN.Data
{
    /// <summary>
    /// Class that holds a path and eases path manipualtion to work with Unity APIs.
    /// The reason I made this is because different APIs require different path types. 
    /// Absolute, Resource Relative, Asset Relative etc. 
    /// </summary>
    public class UnityPath
    {
        private static UnityPath _projectPath = null;
        private static UnityPath _assetsPath = null;
        private static UnityPath _persistentDataPath = null;

        private PathString _absolutePath = null;

        private bool _resourceRelativePathInitalized = false;
        private PathString _resourceRelativePath = null;

        private bool _assetRelativePathInitalized = false;
        private PathString _assetRelativePath = null;

        private bool _assetDatabaseAssetPathInitialized = false;
        private PathString _assetDatabaseAssetPath = null;

        private bool _projectRelaitvePathInitialized = false;
        private PathString _projectRelativePath = null;

        private string _resourceApiCompatiblePath = null;

        #region Public Static Fields and Properties
        /// <summary>
        /// Name of the Resources Folder
        /// </summary>
        public const string cResourcesFolderName = "Resources";

        /// <summary>
        /// Name of the Assets folder
        /// </summary>
        public const string cAssetsFolderName = "Assets";

        /// <summary>
        /// Path to project root (so, parent folder of Assets)
        /// </summary>
        public static UnityPath ProjectPath
        {
            get
            {
                if (_projectPath == null) _projectPath = AssetsPath.Path.RemoveAtEnd();
                return _projectPath;
            }
        }

        /// <summary>
        /// The name of the project root folder
        /// </summary>
        public static string ProjectFolderName => ProjectPath.Path.End;

        /// <summary>
        /// Path to the /Assets folder
        /// </summary>
        public static UnityPath AssetsPath
        {
            get
            {
                if (_assetsPath == null) _assetsPath = Application.dataPath;
                return _assetsPath;
            }
        }

        /// <summary>
        /// Path to the persistent data path of the platform. Cannot be called during
        /// Monobehaviour constructor and must be called in Start() or Awake()
        /// </summary>
        public static UnityPath PersistentDataPath
        {
            get
            {
                if (_persistentDataPath == null) _persistentDataPath = Application.persistentDataPath;
                return _persistentDataPath;
            }
        }
        #endregion

        #region Public Instance Fields and Properties
        /// <summary>
        /// PathString given at construction time
        /// </summary>
        public PathString Path { get; private set; }
        
        /// <summary>
        /// Resource Relative version of the path. Null if does not exist
        /// </summary>
        public PathString ResourceRelativePath
        {
            get
            {
                if (!_resourceRelativePathInitalized)
                {
                    if (Path.Contains(cResourcesFolderName))
                    {
                        _resourceRelativePath = Path.RelativeTo(cResourcesFolderName);
                    }

                    _resourceRelativePathInitalized = true;
                }

                return _resourceRelativePath;
            }
        }

        /// <summary>
        /// Returns a path that is compatible with calls to the Unity Resource system. This
        /// means a path relative to the Resources folder WITHOUT the extention of the file, using / slash separator.
        /// Why, without the extention. No fucking clue. Unity has random path assumptions everywhere. 
        /// Maybe they don't want you to call a texture with many path extenions for png and jpeg. 
        /// https://docs.unity3d.com/ScriptReference/Resources.Load.html
        /// </summary>
        public string ResourceApiCompatablePath
        {
            get
            {
                if (_resourceApiCompatiblePath == null) _resourceApiCompatiblePath = ResourceRelativePath.ToString('/', false);
                return _resourceApiCompatiblePath;
            }
        }

        /// <summary>
        /// Asset database "Asset Path" version of the path. This is a project relative path including the extension. see: https://docs.unity3d.com/ScriptReference/AssetDatabase.LoadAssetAtPath.html as example. Null if does not exist. 
        /// </summary>
        public PathString AssetDatabaseAssetPath
        {
            get
            {
                if (!_assetDatabaseAssetPathInitialized)
                {
                    if (AbsolutePath.Contains(ProjectFolderName))
                    {
                        _assetDatabaseAssetPath = AbsolutePath.RelativeTo(ProjectFolderName);
                    }

                    _assetDatabaseAssetPathInitialized = true;
                }

                return _assetDatabaseAssetPath;
            }
        }

        /// <summary>
        /// Asset relative version of the path. Null if does not exist
        /// </summary>
        public PathString AssetRelativePath
        {
            get
            {
                if (!_assetRelativePathInitalized)
                {
                    if (AbsolutePath.Contains(cAssetsFolderName))
                    {
                        _assetRelativePath = AbsolutePath.RelativeTo(cAssetsFolderName);
                    }

                    _assetRelativePathInitalized = true;
                }

                return _assetRelativePath;
            }
        }

        /// <summary>
        /// Project relative version of the path
        /// </summary>
        public PathString ProjectRelativePath
        {
            get
            {
                if (!_projectRelaitvePathInitialized)
                {
                    _projectRelativePath = AbsolutePath.RelativeTo(ProjectFolderName);
                }

                return _projectRelativePath;
            }
        }

        /// <summary>
        /// Gets the path as an absolute path using the System.IO.Path.GetFullPath method.
        /// This path should mesure form the project folder.
        /// </summary>
        public PathString AbsolutePath
        {
            get
            {
                if (_absolutePath == null) _absolutePath = System.IO.Path.GetFullPath(Path);
                return _absolutePath;
            }

        }

        /// <summary>
        /// Create a new UnityPath using a PathString
        /// </summary>
        /// <param name="path"></param>
        public UnityPath(PathString path)
        {
            Path = path;
        }
        #endregion

        #region Public Instance API
        /// <inheritdoc />
        public override string ToString() => Path.ToString();
        #endregion

        #region Public Static API
        /// <summary>
        /// Converts a path created from the asset database path into a unity path.
        /// Basically gets it's absolute path and then provides this as the unity path
        /// </summary>
        public static UnityPath FromAssetDatabasePath(string assetDatabasePath)
            => ProjectPath.Path.InsertAtEnd(assetDatabasePath); 

        /// <summary>
        /// UnityPaths can implicitly convert to PathStrings using the original path 
        /// </summary>
        /// <param name="path"></param>
        public static implicit operator PathString(UnityPath path) => path.Path;

        /// <summary>
        /// UnityPaths can implicitly convert to PathStrings using the original path 
        /// </summary>
        /// <param name="path"></param>
        public static implicit operator UnityPath(PathString path) => new UnityPath(path);

        /// <summary>
        /// Creates a UnityPath from the string by infering the separator
        /// </summary>
        /// <param name="path"></param>
        public static implicit operator UnityPath(string path) => new UnityPath(path);

        /// <summary>
        /// Creates a string from a UnityPath by calling ToString() on PathString Path
        /// </summary>
        /// <param name="path"></param>
        public static implicit operator string(UnityPath path) => path.Path;
        #endregion
    }
}