namespace AkkaWebApi.Messages
{
    public class GetCustomerByNameMessage
    {
        public GetCustomerByNameMessage(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}