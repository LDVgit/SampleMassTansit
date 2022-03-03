namespace SampleMassTransit.Contracts
{
    using System;

    public interface IOrderStatus
    {
        Guid OrderId { get; set; }
        string State { get; set; }
    }
}