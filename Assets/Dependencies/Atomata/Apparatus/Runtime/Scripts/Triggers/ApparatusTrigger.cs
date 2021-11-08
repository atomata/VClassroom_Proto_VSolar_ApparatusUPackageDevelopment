using HexCS.Core;

using HexUN.Framework.Debugging;

using System.Collections.Generic;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// An apparatus trigger defines a path and a trigger arg object. 
    /// The path is followed from the the apparatus root ot the receiving node. 
    /// </summary>
    public class ApparatusTrigger
    {
        /// <summary>
        /// Path the trigger should follow. If Path = *, trigger
        /// is global and is performed on each node
        /// </summary>
        public string Path;

        /// <summary>
        /// Trigger type
        /// </summary>
        public ETriggerType Type;

        /// <summary>
        /// Parameters come in pairs but for Unity serialization the parameters
        /// will always be neighbours in the list. So [key, value, key, value, ...]
        /// </summary>
        public string[] Params;

        /// <summary>
        /// Used in debugging to provide a string formated <TRIG#-HashCode> to 
        /// identify the trigger in logs
        /// </summary>
        /// <returns></returns>
        public string GetIDString()
        {
            return $"<TRIG#-{GetHashCode()}>";
        }

        private ApparatusTrigger() { }

        public Dictionary<string, string> ParameterMap
        {
            get
            {
                Dictionary<string, string> map = new Dictionary<string, string>();
                for (int i = 0; i < Params.Length; i+=2)
                {
                    map.Add(Params[i], Params[i + 1]);
                }
                return map;
            }
        }

        public static ApparatusTrigger FromPathString(PathString pathString)
        {
            string pth = pathString.RemoveAtEnd();
            string trig = pathString.End;

            if (trig.Contains("="))
            {
                string[] nameAndArgs = trig.Split('?');
                string[] argAndVal = nameAndArgs[1].Split('=');

                return Trigger_Bool(nameAndArgs[0], bool.Parse(argAndVal[1]), pth);
            }
            else
            {
                return DirectEvent_Void(trig, pth);
            }
        }

        /// <summary>
        /// Create a load trigger, which tells a target apparatus to load it's content.
        /// If shouldLoad is false, then performs an unload instead
        /// </summary>
        public static ApparatusTrigger LoadTrigger(bool shouldLoad, string path = "*")
        {
            return new ApparatusTrigger() {
                Path = path,
                Type = ETriggerType.Load,
                Params = new string[] { "load", shouldLoad.ToString() }
            };
        }

        /// <summary>
        /// Create an void trigger, which tells a target apparatus to attempt to animate its ethreal asset
        /// </summary>
        public static ApparatusTrigger DirectEvent_Void(string name, string path = "*")
        {
            return new ApparatusTrigger()
            {
                Path = path,
                Type = ETriggerType.Event,
                Params = new string[] { "name", name, "type", "void" }
            };
        }

        /// <summary>
        /// Create an void trigger, which tells a target apparatus to attempt to animate its ethreal asset
        /// </summary>
        public static ApparatusTrigger Trigger_Bool(string name, bool value, string path = "*")
        {
            return new ApparatusTrigger()
            {
                Path = path,
                Type = ETriggerType.Event,
                Params = new string[] { "name", name, "type", "bool", "value", value.ToString() }
            };
        }

        /// <summary>
        /// Get paramter if exists or return false 
        /// </summary>
        public bool TryGetParameter(string key, out string value)
        {
            if (Params == null || Params.Length == 0)
            {
                value = null;
                return false;
            }

            for(int i = 0; i<Params.Length; i+=2)
            {
                if (key == Params[i])
                {
                    value = Params[i + 1];
                    return true;
                }
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Get paramter if exists or return false 
        /// </summary>
        public bool TryGetParameters(string[] keys, out string[] values)
        {
            if (Params == null || Params.Length < keys.Length * 2)
            {
                values = null;
                return false;
            }

            values = new string[keys.Length];
            for(int j = 0; j < keys.Length; j++)
            {
                for (int i = 0; i < Params.Length; i += 2)
                {
                    if (keys[j] == Params[i])
                    {
                        values[j] = Params[i + 1];
                        if(j == values.Length - 1)return true;
                    }
                }
            }

            values = null;
            return false;
        }
    }
}