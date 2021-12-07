using UnityEngine;

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
        public const string cMetaInputFormat = "{0}/{1}";

        public const string cMetaTypeIdentifer = "identifier";
        public const string cMetaTypeType = "type";
        public const string cMetaTypeAssociatedNode = "associatedNode";
        public const string cMetaTypeKey = "key";
        public const string cMetaTypeInput = "input";
        public const string cMetaTypeTransform = "transform";

        public const string cMetaInputVoidType = "void";
        public const string cMetaInputBoolType = "bool";
        public const string cMetaInputVector3Type = "vec3";

        public static string AssociatedNodeMeta(string associatedNode)
          => string.Format(cMetaInfoFormat, cMetaTypeAssociatedNode, associatedNode);

        public static string IdentifierMeta(string identifier)
            => string.Format(cMetaInfoFormat, cMetaTypeIdentifer, identifier);

        public static string TypeMeta(string type)
            => string.Format(cMetaInfoFormat, cMetaTypeType, type);

        public static string InputMeta(string inputType, string inputName)
            => string.Format(cMetaInfoFormat, cMetaTypeInput, string.Format(cMetaInputFormat, inputType, inputName));

        public static string KeyMeta(string key) 
            => string.Format(cMetaInfoFormat, cMetaTypeKey, key);

        public static string TransformMeta(Transform transform)
            => string.Format(cMetaInfoFormat, cMetaTypeTransform, $"{transform.position.x},{transform.position.y},{transform.position.z},{transform.rotation.x},{transform.rotation.y},{transform.rotation.z},{transform.rotation.w}, {transform.localScale.x},{transform.localScale.y},{transform.localScale.z}");
    }
}