using System;
using Rebus;

namespace RebusWorker
{
    /// <summary>
    /// All messages are sendt to the same endpoint (back to the sender)
    /// In a real world scenario there will be multiple endpoints handling different messages
    /// </summary>
    public class OwnershipResolver : IDetermineMessageOwnership
    {
        public string GetEndpointFor(Type messageType)
        {
            return "Acos.Websak.Ekspederingservice.Input";
        }
    }
}