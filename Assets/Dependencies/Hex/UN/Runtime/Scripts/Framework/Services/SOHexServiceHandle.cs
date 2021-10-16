using UnityEngine;

namespace HexUN.Framework.Services
{
    /// <summary>
    /// SOWrapper for common service functions. Acts as a way to do
    /// hex service related commands as UnityEventResponses
    /// </summary>
    public class SoHexServiceHandle : ScriptableObject
    {
        #region API
        /// <summary>
        /// Quits the applicaiton via the IAppControl interface
        /// </summary>
        public void IAppControl_Quit()
        {
            OneHexServices.Instance.AppControl.Quit();
        }
        #endregion
    }
}