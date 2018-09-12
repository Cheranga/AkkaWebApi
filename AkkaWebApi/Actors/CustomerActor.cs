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
        private readonly ActorSystem _actorSystem;
        private readonly ICustomerRepository _customerRepository;
        private readonly IActorRef _customerRouterActor;

        public CustomerActor(ActorSystem actorSystem, ICustomerRepository customerRepository)
        {
            _actorSystem = actorSystem;
            _customerRepository = customerRepository;

            var props = Props.Create<CustomerActor>().WithRouter(new RoundRobinPool(5, new DefaultResizer(5, 10)));
            _customerRouterActor = _actorSystem.ActorOf(props);

            ReceiveAsync<GetCustomerByNameMessage>(HandleAsync);
        }

        private async Task<List<Customer>> HandleAsync(GetCustomerByNameMessage message)
        {
            var customers = await _customerRepository.GetCustomerByNameAsync(message.Name);
            return customers;
        }
    }
}
