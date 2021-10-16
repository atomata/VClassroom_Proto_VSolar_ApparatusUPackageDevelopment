using HexCS.Core;
using HexCS.Data.Generation.CSharp;

namespace HexUN.Utilities
{
    public static class UTStructGen
    {
        /// <summary>
        /// Generates a serializable public property that allows a struct to the both serialize
        /// a field and have public property get and set funcitons so that interfaces can be used to define
        /// properties
        /// </summary>
        /// <param name="gtStruct">the target</param>
        /// <param name="name">name of the property. This is be modifed to use _a... for field and A... for property </param>
        /// <param name="type">The type</param>
        /// <param name="comment">The comment in the tool tip of the field and intellisense of property</param>
        /// <param name="defaultValue">the default value, null if no value</param>
        public static void Utility_SerialzablePublicProperty(
            this GTStruct gtStruct, 
            string name, 
            string type, 
            string comment, 
            string defaultValue = null)
        {
            string fieldName = $"_{name.EnforceFirstCharLowerCase()}";
            string propertyName = name.EnforceFistCharCaptial();
            
            using (GTField f = gtStruct.Generate_Field<GTField>())
            {
                f.Generate_Attribute<GTAttribute>().SetRequired("SerializeField");

                using (GTAttribute att = f.Generate_Attribute<GTAttribute>())
                {
                    att.SetRequired("Tooltip");
                    att.Add_Args(new Arg_Basic($"\"{comment}\""));
                }

                f.SetRequired(type, fieldName, EKeyword.PRIVATE);

                using (GTValue val = f.Generate_DefaultValue<GTValue>())
                {
                    val.SetRequired(defaultValue);
                }
            }

            using (GTProperty_OneLine p = gtStruct.Generate_Property<GTProperty_OneLine>())
            {
                p.Generate_Comment<GTComment>().SetRequired(comment);

                p.SetRequired(type, propertyName, EKeyword.PUBLIC);
                p.GetFunction = new GTProperty_OneLine.FunctionParams() { IsPresent = true, Statement = $"{fieldName};" };
                p.SetFunction = new GTProperty_OneLine.FunctionParams() { IsPresent = true, Statement = $"{fieldName} = value;" };
            }
        }
    }
}