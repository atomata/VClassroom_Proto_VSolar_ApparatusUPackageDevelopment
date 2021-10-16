namespace Atomata.VSolar.Apparatus
{
    public enum EApparatusNodeConnectionState
    {
        /// <summary>
        /// Active nodes are actively part of the apparatus object tree and 
        /// can be used without error
        /// </summary>
        Disconnected = 0,

        /// <summary>
        /// Inactive nodes have not yet calculated their position in the tree
        /// and have not notified parents
        /// </summary>
        Connected = 1,
    }
}