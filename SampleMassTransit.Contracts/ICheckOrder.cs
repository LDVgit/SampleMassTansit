namespace SampleMassTransit.Contracts
{
    using System;

    public interface ICheckOrder
    {
        Guid OrderId { get; set; }
        DateTime TimeStamp { get; set; }
        
        string CustomerNumber { get; set; }
    }
}