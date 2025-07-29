using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Posts.Commands;
using Business.Posts.Queries;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Post
{
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetAll()
        {
            var query = new GetAllPostsQuery();
            var posts = await _mediator.Send(query);
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetById(int id)
        {
            var query = new GetPostByIdQuery { PostId = id };
            var post = await _mediator.Send(query);

            if (post == null)
            {
                return NotFound($"No se encontró el post con ID {id}");
            }

            return Ok(post);
        }

        [HttpPost()]
        public async Task<ActionResult<PostDto>> Create([FromBody] CreatePostCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPost = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = createdPost.PostId }, createdPost);
        }

        [HttpPost("multiple")]
        public async Task<ActionResult<IEnumerable<PostDto>>> CreateMultiple([FromBody] CreateMultiplePostsCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPosts = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), createdPosts);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PostDto>> Update(int id, [FromBody] UpdatePostCommand command)
        {
            if (id != command.PostId)
            {
                return BadRequest("El ID de la URL no coincide con el ID del post");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPost = await _mediator.Send(command);
            return Ok(updatedPost);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeletePostCommand { PostId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetByCustomerId(int customerId)
        {
            var query = new GetPostsByCustomerIdQuery { CustomerId = customerId };
            var posts = await _mediator.Send(query);
            return Ok(posts);
        }
    }
}
