namespace HexUN.App
{
    /// <summary>
    /// Interface for application levle control requirements in a HexService
    /// that controls application lifecycle
    /// </summary>
    public interface IAppControl
    {
        /// <summary>
        /// Quits the application
        /// </summary>
        public void Quit();
    }
}