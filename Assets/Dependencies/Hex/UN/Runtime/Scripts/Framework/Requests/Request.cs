using System;
using System.Collections;
using System.Threading.Tasks;
using System.Timers;

using UnityEngine;

namespace HexUN.Framework.Request
{
    /// <summary>
    /// Implements a request object that requires a reponder to populate
    /// response.
    /// </summary>
    public class Request<TReq, TRes>
    {
        private readonly TaskCompletionSource<TRes> _src = new TaskCompletionSource<TRes>();
        
        private object _repsonderObjectLock = new object();
        private object _responderObject = null;

        /// <summary>
        /// Is the request complete
        /// </summary>
        public bool IsComplete => State == EState.Complete;

        /// <summary>
        /// Has the request been claimed by a responder
        /// </summary>
        public bool IsClaimed => State != EState.AwaitingClaim;

        /// <summary>
        /// The current state of the request
        /// </summary>
        public EState State { get; private set; } = EState.AwaitingClaim;

        /// <summary>
        /// The object sent as the request
        /// </summary>
        public TReq RequestObject { get; private set; }

        /// <summary>
        /// The object sent in response
        /// </summary>
        public TRes ResponseObject { get; private set; }

        /// <summary>
        /// Creates a request by populating the request object ONLY
        /// </summary>
        public Request(TReq request) => RequestObject = request;

        /// <summary>
        /// Await the completion of the request. Set timeout in milliseconds.
        /// Throws <see cref="TimeoutException"/> if timeout occurs
        /// </summary>
        public async Task<TRes> AwaitAsync()
        {
            return await _src.Task;
        }

        /// <summary>
        /// Awaits the completion of the request by checking that the task
        /// is completed. If it is not, wait one frame and tries again
        /// </summary>
        public IEnumerator AwaitCoroutine()
        {
            while (!IsComplete) yield return new WaitForEndOfFrame();
        }

        /// <summary>
        /// Claim the request. An object that claims a request is responsible for completeing the
        /// request and called respond. Returns false if the claim fails. 
        /// </summary>
        public bool TryClaim(object responderObject)
        {
            if (IsClaimed) return false;

            lock (_repsonderObjectLock)
            {
                if (_responderObject != null) return false;

                _responderObject = responderObject;
                State = EState.AwaitingResponse;
                return true;
            }
        }

        /// <summary>
        /// Complete the request by providing a response.
        /// </summary>
        public void Respond(TRes response, object responderObject)
        {
            if(State == EState.AwaitingClaim)
                throw new RequestException("Attempted to respond without claiming the request. Response failed");

            if(State == EState.AwaitingResponse && _responderObject != responderObject)
                throw new RequestException($"{responderObject} attempted to respond to the request, but is not the registered responder {_responderObject}");

            if(State == EState.Complete)
                throw new RequestException($"Request is already complete, cannot respond again");

            ResponseObject = response;
            State = EState.Complete;
            _src.TrySetResult(response);
        }

        /// <summary>
        /// Requests await a claimer when created. Any object can claim the request, then
        /// they become responsible for completing it
        /// </summary>
        public enum EState
        {
            /// <summary>
            /// No responder has yet claimed the request, and no one is reponding
            /// </summary>
            AwaitingClaim, 

            /// <summary>
            /// A responder has claimed the request and is in the process of responding
            /// </summary>
            AwaitingResponse,

            /// <summary>
            /// The request has been completed by the responder
            /// </summary>
            Complete
        }

        public class RequestException : Exception 
        {
            public RequestException() : base() { }
            public RequestException(string message) : base(message) { }
        }
    }
}