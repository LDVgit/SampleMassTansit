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
            var (accepted, rejected) = await _requestClient
                .GetResponse<IOrderSubmissionAccepted, IOrderSubmissionRejected>(new
                {
                    OrderId = id,
                    TimeStamp = InVar.Timestamp,
                    CustomerNumber = customerNumber
                });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Ok(response.Message);
            }

            else
            {
                var response = await rejected;
                return BadRequest(response.Message);
            }
        }
    }
}