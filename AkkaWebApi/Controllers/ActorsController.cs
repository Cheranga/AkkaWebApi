using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaWebApi.Actors;
using AkkaWebApi.Messages;
using AkkaWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AkkaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ICustomerHandler _customerHandler;

        public ActorsController(ICustomerHandler customerHandler)
        {
            _customerHandler = customerHandler;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAsync([FromRoute]string name)
        {
            var customers = await _customerHandler.HandleAsync(new GetCustomerByNameMessage(name));
            if (customers.Any())
            {
                return Ok(customers);
            }

            return NotFound();
        }
    }
}