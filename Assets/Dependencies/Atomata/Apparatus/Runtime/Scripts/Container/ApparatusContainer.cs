using HexUN.Framework.Debugging;

namespace Atomata.VSolar.Apparatus.Example
{
    /// <summary>
    /// This is an example container, that has only been created to show how
    /// a container can load an apparatus from byte[] data that is stored in azure blob storage as assetbundle. 
    /// </summary>
    public class ApparatusContainer : AApparatusContainer
    {
        protected override string cLogCategory { get; set; } = nameof(ApparatusContainer);

        public AtomataSceneManager SceneManager;

        public async void Trigger(ApparatusTrigger trig, LogWriter log) => await ManagedNode.Trigger(trig, log);
        
        /// <summary>
        /// Handles boolean triggers, sent as strings with following format 
        /// path/to/node@eventName?(True|False). 
        /// </summary>
        public override async void BoolTrigger(string trigger)
        {
            if (ManagedNode != null)
            {
                LogWriter log = new LogWriter(cLogCategory);
                // unpack the info
                string[] pathAndArgs = trigger.Split('@');
                string[] args = pathAndArgs[1].Split('?');

                // convert the info to a bool trigger object
                await ManagedNode.Trigger(
                    ApparatusTrigger.Trigger_Bool(args[0], bool.Parse(args[1]), pathAndArgs[0]), log
                );

            }
        }

        /// <summary>
        /// Handles void triggers, sent as strings with following format 
        /// path/to/node@eventName. 
        /// </summary>
        public override async void VoidTrigger(string trigger)
        {
            if (ManagedNode != null)
            {
                LogWriter log = new LogWriter(cLogCategory);
                // unpack the info
                string[] pathAndName = trigger.Split('@');

                //convert the info to a void trigger object
                await ManagedNode.Trigger(
                    ApparatusTrigger.DirectEvent_Void(pathAndName[1], pathAndName[0]), log
                );
            }
        }

        /// <summary>
        /// This function handles requests from the apparatus and resolves them
        /// based on a Desktop platform with relevent data existing in the file system.
        /// </summary>
        public override void HandleRequest(ApparatusRequest request, LogWriter log) 
            => SceneManager.HandleRequest(request, log);
    }
}