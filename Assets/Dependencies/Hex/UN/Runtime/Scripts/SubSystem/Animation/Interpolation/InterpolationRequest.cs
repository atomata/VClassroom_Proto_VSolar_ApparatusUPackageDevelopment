using HexCS.Core;

namespace HexUN.Animation
{
    /// <summary>
    /// Token used to track an instance of an interpolation
    /// </summary>
    public class InterpolationRequest : IInterpolationToken<float[]>
    {
        /// <summary>
        /// The identifier for the interpolation
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The interpolation being performed
        /// </summary>
        public SInterpolation[] Interpolations {get; private set;}

        /// <summary>
        /// The duration in seconds of the interpolation
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// Invoked when interpolation ends
        /// </summary>
        public Event OnInterpolationEnd = new Event();

        /// <summary>
        /// Invoked when interpolation is canceled
        /// </summary>
        public Event OnInterpolationCanceled = new Event();

        /// <summary>
        /// Invoked then the interpolation is performed. Returns the new value
        /// </summary>
        public Event<float[]> OnInterpolation = new Event<float[]>();

        /// <summary>
        /// Indicates if the interpolation is canceled
        /// </summary>
        public bool Cancelled { get; private set; } = false;

        /// <inheritdoc />
        public IEventSubscriber OnInterpolationEndSubscriber => OnInterpolationEnd;

        /// <inheritdoc />
        public IEventSubscriber OnInterpolationCanceledSubscriber => OnInterpolationCanceled;

        /// <inheritdoc />
        public IEventSubscriber<float[]> OnInterpolationSubscriber => OnInterpolation;

        /// <summary>
        /// Constructor
        /// </summary>
        public InterpolationRequest(int id, float duration, SInterpolation[] interpolations)
        {
            Id = id;
            Interpolations = interpolations;
            Duration = duration;
        }

        /// <inheritdoc />
        public void Cancel()
        {
            Cancelled = true;
        }
    }
}
