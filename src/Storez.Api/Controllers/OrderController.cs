using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public OrderController(IRequestClient<SubmitOrder> client)
        {
            _client = client;
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
                   order.CustomerNumber
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
