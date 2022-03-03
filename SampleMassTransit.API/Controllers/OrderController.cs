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
        private readonly IRequestClient<ICheckOrder> _checkOrderClient;

        public OrderController(
            ILogger<OrderController> logger,
            IRequestClient<ISubmitOrder> requestClient, 
            IRequestClient<ICheckOrder> checkOrderClient)
        {
            _logger = logger;
            _requestClient = requestClient;
            _checkOrderClient = checkOrderClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _checkOrderClient.GetResponse<IOrderStatus, IOrderNotFound>(
                new
                {
                    OrderId = id
                });

            if (response.Is(out Response<IOrderStatus> accepted))
            {
                var result = accepted.Message;
                return Ok(accepted.Message);
            }
             
            if(response.Is(out Response<IOrderNotFound> notFound))
            {
                var result = notFound.Message;
                return NotFound(result);
            }

            return Problem();
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