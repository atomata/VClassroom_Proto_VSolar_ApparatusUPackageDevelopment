using HexUN.Framework;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Utility functions and constants used in the <see cref="AApparatusNode"/>
    /// triggering system
    /// </summary>
    public static class UTTriggers
    {
        private const string cLogCategory = nameof(UTTriggers);

        public static bool TryUnpackTrigger_Load(this ApparatusTrigger trigger, out bool shouldLoad)
        {
            if (trigger.Type == ETriggerType.Load && trigger.TryGetParameter("load", out string loadArg))
            {
                if (bool.TryParse(loadArg, out shouldLoad))
                {
                    return true;
                }
                else
                {
                    OneHexServices.Instance.Log.Warn(cLogCategory, "Received load trigger with malformed arguments. Load triggers require a load argument with a bool value which indicates whether a load or unload is being triggered");
                }
            }

            shouldLoad = default;
            return false;
        }

        public static bool TryUnpackTrigger_Input(this ApparatusTrigger trigger, out string type, out string name, out string value)
        {
            if (trigger.Type == ETriggerType.Event && trigger.TryGetParameter("type", out type) && trigger.TryGetParameter("name", out name))
            {
                if(type != UTMeta.cMetaInputVoidType)
                {
                    if (trigger.TryGetParameter("value", out value)) return true;
                }

                else
                {
                    value = default;
                    return true;
                }
            }

            type = default;
            name = default;
            value = default;
            return false;
        }
    }
}