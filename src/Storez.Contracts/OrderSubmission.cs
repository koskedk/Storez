using System;

namespace Storez.Contracts
{
    public interface OrderSubmission
    {
        Guid OrderId { get; }
        DateTime Timestamp { get; }
        string CustomerNumber { get; }
    }
}
