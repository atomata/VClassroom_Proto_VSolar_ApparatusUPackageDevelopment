using HexCS.Core;

namespace HexUN.Utilities
{
    public interface IUpdateable<T>
    {
        IEventSubscriber<T> OnUpdate { get; }
    }
}
