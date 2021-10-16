namespace Atomata.VSolar.Apparatus
{
    public enum EApparatusNodeLoadState
    {
        /// <summary>
        /// The totem is currently not managing an ethreal asset
        /// and is waiting to be told to load
        /// </summary>
        Unloaded = 0,

        /// <summary>
        /// The totem is managing an ethreal asset
        /// </summary>
        Loaded = 2
    }
}