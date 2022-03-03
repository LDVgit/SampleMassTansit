namespace SampleMassTransit.Contracts
{
    using System;

    public interface IOrderNotFound
    {
        Guid OrderId { get; set; }
    }
}