namespace SampleMassTransit.Contracts
{
    using System;

    public interface ISubmitOrder
    {
        Guid OrderId { get; set; }
        DateTime TimeStamp { get; set; }
        
        string CustomerNumber { get; set; }
    }
}