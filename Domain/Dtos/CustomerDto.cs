namespace Domain.Dtos
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
    }

    public class CreateCustomerDto
    {
        public string Name { get; set; }
    }

    public class UpdateCustomerDto
    {
        public string Name { get; set; }
    }
} 