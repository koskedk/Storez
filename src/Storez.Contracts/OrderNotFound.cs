using System;

namespace Storez.Contracts
{
    public interface OrderNotFound
    {
        Guid OrderId { get; }
    }
}