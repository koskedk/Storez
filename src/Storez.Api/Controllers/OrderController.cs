using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Storez.Api.Models;
using Storez.Contracts;

namespace Storez.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IRequestClient<SubmitOrder> _client;
        private readonly IRequestClient<CheckOrder> _requestClient;
        public OrderController(IRequestClient<SubmitOrder> client, IRequestClient<CheckOrder> requestClient)
        {
            _client = client;
            _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid orderId)
        {
            Log.Debug($"Checking {orderId}");
            try
            {
                var (status,notFound) =await  _requestClient.GetResponse<OrderStatus,OrderNotFound>(new
                {
                    OrderId=orderId
                });

                if (status.IsCompletedSuccessfully)
                {
                    var response = await status;
                    return Ok(response.Message);
                }
                else
                {
                    var response = await notFound;
                    return NotFound(response.Message);
                }
            }
            catch (Exception e)
            {
                Log.Error(e,$"Error checking {orderId}");
                return Problem($"Error checking {orderId},{e.Message}");
            }

        }


        [HttpPost]
        public async Task<IActionResult> Post(OrderViewModel order)
        {
            Log.Debug($"Posting {order}");
            try
            {
                var response =await  _client.GetResponse<OrderSubmission>(new
                {
                   OrderId=order.Id,
                   order.CustomerNumber,
                   InVar.Timestamp
                });

                return Ok(response.Message);
            }
            catch (Exception e)
            {
                Log.Error(e,$"Error posting {order}");
                return Problem($"Error posting {order},{e.Message}");
            }

        }
    }
}
