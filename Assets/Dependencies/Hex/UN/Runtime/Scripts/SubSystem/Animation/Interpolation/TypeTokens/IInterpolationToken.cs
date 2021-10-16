using HexCS.Core;

namespace HexUN.Animation
{
    /// <summary>
    /// Obtained by something performing requesting an interpolation. Can be used to subscribe
    /// to important interpolation events or cancel the interpolation
    /// </summary>
    public interface IInterpolationToken<T> : IInterpolationTracker
    {
        /// <summary>
        /// Invoked when an interpolation occur and returns the calculated interpolation value
        /// </summary>
        IEventSubscriber<T> OnInterpolationSubscriber { get; }
    }
}