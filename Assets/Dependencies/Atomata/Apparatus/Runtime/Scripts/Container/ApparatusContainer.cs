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

        public async void Trigger(ApparatusTrigger trig)
        {
            LogWriter log = new LogWriter(cLogCategory);
            await ManagedNode.Trigger(trig, log);
            log.PrintToConsole(cLogCategory);
        }

        /// <summary>
        /// This function handles requests from the apparatus and resolves them
        /// based on a Desktop platform with relevent data existing in the file system.
        /// </summary>
        public override void HandleRequest(ApparatusRequest request, LogWriter log) 
            => SceneManager.HandleRequest(request, log);
    }
}