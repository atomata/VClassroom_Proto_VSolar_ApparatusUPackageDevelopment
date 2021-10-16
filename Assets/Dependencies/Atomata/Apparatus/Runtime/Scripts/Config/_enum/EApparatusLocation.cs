namespace Atomata.VSolar.Apparatus.UnityEditor
{
    /// <summary>
    /// Where are the apparatus asset bundles located?
    /// </summary>
    public enum EApparatusLocation
    {
        /// <summary>
        /// Apparatus can be saved and loaded to local user storage
        /// </summary>
        Local, 

        /// <summary>
        /// Apparatus can be saved and loaded from the cloud
        /// </summary>
        Cloud
    }
}