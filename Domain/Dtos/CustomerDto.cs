namespace Domain.Dtos
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Messages { get; set; }
    }

    public class CreateCustomerDto
    {
        public required string Name { get; set; }
    }

    public class UpdateCustomerDto
    {
        public int CustomerId { get; set; }
        public required string Name { get; set; }
    }
} 