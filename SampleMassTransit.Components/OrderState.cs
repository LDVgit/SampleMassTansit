namespace SampleMassTransit.Components
{
    using System;

    using Automatonymous;

    public class OrderState :
        SagaStateMachineInstance
    {
        /// Идентификатор эеземпляра машины состояния сага
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }

        public string CustomerNumber { get; set; }


        public DateTime? SubmitDate { get; set; }
        public DateTime? Updated { get; set; }
    }
}