using AutoMapper;
using Business.Customers.Commands;
using Domain.Dtos;

namespace Business.Customers.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            // Mapeo de Customer a CustomerDto
            CreateMap<Domain.Entities.Customer, CustomerDto>();
            
            // Mapeo de CustomerDto a Customer (para actualizaciones)
            CreateMap<CustomerDto, Domain.Entities.Customer>();
            
            // Mapeo de CreateCustomerCommand a Customer
            CreateMap<CreateCustomerCommand, Domain.Entities.Customer>()
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.Posts, opt => opt.Ignore());
            
            // Mapeo de UpdateCustomerCommand a Customer
            CreateMap<UpdateCustomerCommand, Domain.Entities.Customer>()
                .ForMember(dest => dest.Posts, opt => opt.Ignore());
            
            // Mapeo de CreateCustomerDto a CreateCustomerCommand
            CreateMap<CreateCustomerDto, CreateCustomerCommand>();
            
            // Mapeo de UpdateCustomerDto a UpdateCustomerCommand
            CreateMap<UpdateCustomerDto, UpdateCustomerCommand>();
        }
    }
} 