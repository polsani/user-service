namespace UserService.Domain.ValueObjects
{
    public class Email
    {
        private const string EmailRegex =
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        
        public string Address { get; private set; }

        public Email(string address)
        {
            Address = address;
        }
    }
}