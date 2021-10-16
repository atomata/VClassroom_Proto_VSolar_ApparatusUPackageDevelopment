namespace HexUN.Serialization
{
    /// <summary>
    /// Can be converted to and from Json
    /// </summary>
    public interface IJsonable
    {
        /// <summary>
        /// return the json string representation
        /// </summary>
        /// <returns></returns>
        string ToJson();

        /// <summary>
        /// Use json to populate fields. returns true if run wthout error
        /// </summary>
        bool FromJson(string json);
    }
}
