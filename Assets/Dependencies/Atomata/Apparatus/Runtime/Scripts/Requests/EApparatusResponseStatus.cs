namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Status provided by response objects
    /// </summary>
    public enum EApparatusResponseStatus
    {
        /// <summary>
        /// The response is successful and the provided object is valid
        /// </summary>
        Success,

        /// <summary>
        /// A generic fail, with no extra indicator of why it failed 
        /// </summary>
        Failed_Generic,

        /// <summary>
        /// A failure that occurs from a missing reference
        /// </summary>
        Failed_ReferenceMissing
    }
}