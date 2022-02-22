namespace SampleMassTransit.Contracts
{
    using System;

    public interface IOrderSubmissionAccepted
    {
        Guid OrderId { get; set; }
        DateTime TimeStamp { get; set; }

        string CustomerNumber { get; set; }
    }
}