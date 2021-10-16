namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// The type of request being sent. Used by environment to know how
    /// to process it
    /// </summary>
    public enum EApparatusRequestType
    {
        /// <summary>
        /// Asset request request a prefab from the environment
        /// </summary>
        LoadAsset = 0,

        /// <summary>
        /// Save the asset to the relvent database endpoint because
        /// the asset has been modifed and the user wants to save it
        /// </summary>
        SaveAsset = 1,

        /// <summary>
        /// Attempts to get an apparatus json from the database and
        /// deserailize it into an SRApparatusNode
        /// </summary>
        LoadApparatus = 2
    }
}