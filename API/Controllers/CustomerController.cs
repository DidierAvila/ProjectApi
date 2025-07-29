using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Customers.Commands;
using Business.Customers.Queries;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Customer
{
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            var query = new GetAllCustomersQuery();
            var customers = await _mediator.Send(query);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(int id)
        {
            var query = new GetCustomerByIdQuery { CustomerId = id };
            var customer = await _mediator.Send(query);

            if (customer == null)
            {
                return NotFound($"No se encontró el cliente con ID {id}");
            }

            return Ok(customer);
        }

        [HttpPost()]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateCustomerCommand
            {
                Name = createDto.Name
            };
            var createdCustomer = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> Update(int id, [FromBody] UpdateCustomerDto updateDto)
        {
            if (id != updateDto.CustomerId)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cliente");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new UpdateCustomerCommand
            {
                CustomerId = updateDto.CustomerId,
                Name = updateDto.Name
            };
            var updatedCustomer = await _mediator.Send(command);
            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteCustomerCommand { CustomerId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}/posts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetCustomerPosts(int id)
        {
            var query = new GetCustomerPostsQuery { CustomerId = id };
            var posts = await _mediator.Send(query);
            return Ok(posts);
        }
    }
}