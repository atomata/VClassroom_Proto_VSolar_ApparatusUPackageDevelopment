using HexCS.Core;

namespace HexUN.Events
{
    public class EventGenerationArgs
    {
        public string EvtType;
        public string EvtNamespace;
        public string MenuPath;
        public string EvtTypeNamespace;

        public string ReadableEvtType;
        public string UnityEvtName;
        public string ReliableEvtName;
        public string SoEvtName;
        public string SoEvtListenerName;
        public string EvtListenerName;
        public string EventProviderName;

        public EventGenerationArgs(string evtType, string evtNamespace, string menuPath, string evtTypeNamespace)
        {
            EvtType = evtType;
            EvtNamespace = evtNamespace;
            MenuPath = menuPath;
            EvtTypeNamespace = evtTypeNamespace;

            ReadableEvtType = evtType.Contains("[]") ? evtType.Replace("[]", "Array").EnforceFistCharCaptial() : evtType?.EnforceFistCharCaptial();
            UnityEvtName = $"{ReadableEvtType}UnityEvent";
            ReliableEvtName = $"{ReadableEvtType}ReliableEvent";
            SoEvtName = $"{ReadableEvtType}SoEvent";
            SoEvtListenerName = $"{ReadableEvtType}SoEventListener";
            EvtListenerName = $"{ReadableEvtType}EventListener";
            EventProviderName = $"A{ReadableEvtType}EventProvider";
        }
    }
}