namespace SampleMassTransit.Components
{
    using System;
    using System.Threading.Tasks;

    using Contracts;

    using MassTransit;

    public class SubmitOrderConsumer :
        IConsumer<ISubmitOrder>
    {
        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            if (context.Message.CustomerNumber.Contains("TEST"))
            {
                await context.RespondAsync<IOrderSubmissionRejected>
                (new {
                    OrderId = context.Message.OrderId,
                    TimeStamp = DateTime.Now,
                    CustomerNumber = context.Message.CustomerNumber,
                    Reason = $"Test customer {context.Message.CustomerNumber}"
                });

                return;
            }

            await context.RespondAsync<IOrderSubmissionAccepted>
            (new
            {
                context.Message.OrderId,
                TimeStamp = InVar.Timestamp,
                context.Message.CustomerNumber
            });
        }
    }
}