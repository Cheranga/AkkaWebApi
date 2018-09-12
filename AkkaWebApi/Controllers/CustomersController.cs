using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AkkaWebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AkkaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAsync([FromRoute]string name)
        {
            var customers = await _customerRepository.GetCustomerByNameAsync(name);
            if (customers.Any())
            {
                return Ok(customers);
            }

            return NotFound();
        }
    }
}