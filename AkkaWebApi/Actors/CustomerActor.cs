using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;
using AkkaWebApi.Messages;
using AkkaWebApi.Models;
using AkkaWebApi.Repositories;

namespace AkkaWebApi.Actors
{
    public class CustomerActor : ReceiveActor
    {   
        private readonly ICustomerRepository _customerRepository;

        public CustomerActor(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            ReceiveAsync<GetCustomerByNameMessage>(HandleAsync);
        }

        private async Task<List<Customer>> HandleAsync(GetCustomerByNameMessage message)
        {
            var customers = await _customerRepository.GetCustomerByNameAsync(message.Name);
            return customers;
        }
    }

    public interface ICustomerHandler
    {
        Task<List<Customer>> HandleAsync(GetCustomerByNameMessage message);
    }

    public class CustomerHandler : ICustomerHandler
    {
        private readonly IActorRef _customerRouterActor;

        public CustomerHandler(ActorSystem actorSystem)
        {
            var props = Props.Create<CustomerActor>().WithRouter(new RoundRobinPool(5, new DefaultResizer(5, 10)));
            _customerRouterActor = actorSystem.ActorOf(props, "customer-actor");
        }

        public Task<List<Customer>> HandleAsync(GetCustomerByNameMessage message)
        {
            return _customerRouterActor.Ask<List<Customer>>(message);
        }


    }
}
