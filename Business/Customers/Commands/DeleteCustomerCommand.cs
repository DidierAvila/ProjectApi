using MediatR;

namespace Business.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public int CustomerId { get; set; }
    }
} 