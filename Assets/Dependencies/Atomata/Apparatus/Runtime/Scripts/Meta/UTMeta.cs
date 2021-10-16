namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Utility functions used to manage the format of meta data
    /// </summary>
    public static class UTMeta
    {
        /// <summary>
        /// Fully qualifed meta is used in meta lists. 0 is the apparatus qualified
        /// path, or an index to a path stored extenerally. 1 is meta info <see cref="cMetaInfoFormat"/>
        /// </summary>
        public const string cMetaQualifiedFormat = "{0}@{1}";

        /// <summary>
        /// This is the format of a meta info, noramlly applied to some address. 
        /// 0 is the type associated with the piexes of meta, like identifier, or input.
        /// 1 is the meta data, in any format, determined by the type. 
        /// </summary>
        public const string cMetaInfoFormat = "{0}:{1}";

        public const string cMetaTypeIdentifer = "identifier";
        
        public const string cMetaTypeInput = "input";
        public const string cMetaInputFormat = "{0}/{1}"; // 0 is input type, 1 is input name
        public const string cMetaInputVoidType = "void";
        public const string cMetaInputBoolType = "bool";
        public const string cMetaInputVector3Type = "vec3";

        public static string IdentifierMeta(string identifier)
            => string.Format(cMetaInfoFormat, cMetaTypeIdentifer, identifier);

        public static string InputMeta(string inputType, string inputName)
            => string.Format(cMetaInfoFormat, cMetaTypeInput, string.Format(cMetaInfoFormat, inputType, inputName));
    }
}