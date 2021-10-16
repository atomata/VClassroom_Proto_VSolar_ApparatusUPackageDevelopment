namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// The type of trigger, indicating how the trigger should be interpreted
    /// </summary>
    public enum ETriggerType
    {
        /// <summary>
        /// Indicates that the trigger is indicating that some load action should be performed.
        /// This means that it's more about instantiating resources, than performing animations, etc.
        /// </summary>
        Load,

        /// <summary>
        /// The trigger trigger indicates that the environment wants the apparatus node to 
        /// activate a trigger from a trigger group present on the node
        /// </summary>
        Event
    }
}