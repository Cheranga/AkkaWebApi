using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaWebApi.Actors;
using AkkaWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AkkaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ActorSystem _actorSystem;

        public ActorsController(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAsync([FromRoute]string name)
        {
            var customerActorProps = Props.Create<CustomerActor>();

            var customers = await _customerRepository.GetCustomerByNameAsync(name);
            if (customers.Any())
            {
                return Ok(customers);
            }

            return NotFound();
        }
    }
}