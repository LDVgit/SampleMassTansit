namespace SampleMassTransit.API.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Contracts;

    using MassTransit;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        private readonly IRequestClient<ISubmitOrder> _requestClient;
        //private readonly IPublishEndpoint _publishEndpoint;
        //private readonly ISendEndpointProvider _sendEndpointProvider;

        public OrderController(
            ILogger<OrderController> logger,
            IRequestClient<ISubmitOrder> requestClient)
        {
            _logger = logger;
            _requestClient = requestClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            var response = await _requestClient
                .GetResponse<IOrderSubmissionAccepted, IOrderSubmissionRejected>(new
                {
                    OrderId = id,
                    TimeStamp = InVar.Timestamp,
                    CustomerNumber = customerNumber
                });

            if (response.Is(out Response<IOrderSubmissionAccepted> responseA))
            {
                var result = responseA.Message;
                return Accepted(result);
            }

            if(response.Is(out Response<IOrderSubmissionRejected> responseB))
            {
                var result = responseB.Message;
                return BadRequest(result);
            }

            return Problem();
        }
        
        
    }
}