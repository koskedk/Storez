using System;

namespace Storez.Contracts
{
    public interface OrderStatus
    {
        Guid OrderId { get; }
        string State { get; }
        string CustomerNumber { get; }
    }
}