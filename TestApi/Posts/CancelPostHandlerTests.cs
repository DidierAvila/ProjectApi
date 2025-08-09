using AutoMapper;
using Business.Logs;
using Business.Posts.Commands;
using Business.Posts.Handlers;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestApi.Posts
{
    public class CancelPostHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<CancelPostCommandHandler>> _loggerMock = new();
        private readonly Mock<ILogService> _logServiceMock = new();

        [Fact]
        public async Task CancelPost_Success()
        {
            // Arrange
            var handler = new CancelPostCommandHandler(_postRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CancelPostCommand { PostId = 1 };
            var post = new Post { PostId = 1, Title = "Test Post", Body = "Test Body", Status = EntityStatus.Active, CustomerId = 1, Type = 1, Category = "Test" };
            var expectedDto = new PostDto { PostId = 1, Title = "Test Post", Body = "Test Body", Status = EntityStatus.Cancelled };

            _postRepoMock.Setup(r => r.GetByID(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);
            _postRepoMock.Setup(r => r.Update(It.IsAny<Post>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns(expectedDto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(EntityStatus.Cancelled);
            result.PostId.Should().Be(1);
            _postRepoMock.Verify(r => r.Update(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancelPost_Fails_WhenPostNotFound()
        {
            // Arrange
            var handler = new CancelPostCommandHandler(_postRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CancelPostCommand { PostId = 999 };

            _postRepoMock.Setup(r => r.GetByID(999, It.IsAny<CancellationToken>())).ReturnsAsync((Post)null!);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Messages.Should().Contain("No se encontró el post con ID 999");
            _postRepoMock.Verify(r => r.Update(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CancelPost_Fails_WhenPostAlreadyCancelled()
        {
            // Arrange
            var handler = new CancelPostCommandHandler(_postRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CancelPostCommand { PostId = 1 };
            var post = new Post { PostId = 1, Title = "Test Post", Body = "Test Body", Status = EntityStatus.Cancelled, CustomerId = 1, Type = 1, Category = "Test" };

            _postRepoMock.Setup(r => r.GetByID(1, It.IsAny<CancellationToken>())).ReturnsAsync(post);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Messages.Should().Contain("ya está cancelado");
            _postRepoMock.Verify(r => r.Update(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}