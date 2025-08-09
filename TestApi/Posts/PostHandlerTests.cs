using AutoMapper;
using Business.Logs;
using Business.Posts.Commands;
using Business.Posts.Handlers;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestApi.Posts
{
    public class PostHandlerTests
    {
        private readonly Mock<IPostRepository> _postRepoMock = new();
        private readonly Mock<ICustomerRepositoy> _customerRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<CreatePostCommandHandler>> _loggerMock = new();
        private readonly Mock<ILogService> _logServiceMock = new();

        [Fact]
        public async Task CreatePost_Success()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CreatePostCommand { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 1, Category = "Farándula" };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { CustomerId = 1 });
            _mapperMock.Setup(m => m.Map<Post>(It.IsAny<CreatePostCommand>())).Returns(new Post { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 1, Category = "Farándula" });
            _postRepoMock.Setup(r => r.Create(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 1, Category = "Farándula" });
            _mapperMock.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns(new PostDto { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 1, Category = "Farándula" });
            var result = await handler.Handle(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Title.Should().Be("Test Post");
        }

        [Fact]
        public async Task CreatePost_Fails_WhenCustomerNotFound()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CreatePostCommand { Title = "Test Post", Body = "Body", CustomerId = 99 };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null!);
            await Assert.ThrowsAsync<System.InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreatePost_Fails_WhenTitleIsEmpty()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CreatePostCommand { Title = "", Body = "Body", CustomerId = 1 };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { CustomerId = 1 });
            await Assert.ThrowsAsync<System.InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreatePost_Fails_WhenBodyIsEmpty()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CreatePostCommand { Title = "Test Post", Body = "", CustomerId = 1 };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { CustomerId = 1 });
            await Assert.ThrowsAsync<System.InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreatePost_MapsCorrectly()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CreatePostCommand { Title = "MapTest", Body = "Body", CustomerId = 1, Type = 2, Category = "Política" };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { CustomerId = 1 });
            _mapperMock.Setup(m => m.Map<Post>(It.IsAny<CreatePostCommand>())).Returns(new Post { Title = "MapTest", Body = "Body", CustomerId = 1, Type = 2, Category = "Política" });
            _postRepoMock.Setup(r => r.Create(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 1, Category = "Farándula" });
            _mapperMock.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns(new PostDto { Title = "MapTest", Body = "Body", CustomerId = 1, Type = 2, Category = "Política" });
            var result = await handler.Handle(command, CancellationToken.None);
            result.Title.Should().Be("MapTest");
        }

        [Fact]
        public async Task CreatePost_Throws_OnRepositoryException()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CreatePostCommand { Title = "RepoError", Body = "Body", CustomerId = 1 };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { CustomerId = 1 });
            _ = _mapperMock.Setup(m => m.Map<Post>(It.IsAny<CreatePostCommand>())).Returns(new Post { Title = "RepoError", Body = "Body", CustomerId = 1 });
            _postRepoMock.Setup(r => r.Create(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("DB Error"));
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreatePost_ValidatesBodyTruncation()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var longBody = new string('a', 120);
            var command = new CreatePostCommand { Title = "Test Post", Body = longBody, CustomerId = 1, Type = 1, Category = "Farándula" };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { CustomerId = 1 });
            _mapperMock.Setup(m => m.Map<Post>(It.IsAny<CreatePostCommand>())).Returns(new Post { Title = "Test Post", Body = longBody, CustomerId = 1, Type = 1, Category = "Farándula" });
            _postRepoMock.Setup(r => r.Create(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 1, Category = "Farándula" });
            _mapperMock.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns((Post p) => new PostDto { Title = p.Title, Body = p.Body, CustomerId = p.CustomerId, Type = p.Type, Category = p.Category });
            var result = await handler.Handle(command, CancellationToken.None);
            result.Body.Should().HaveLength(100);
            result.Body.Should().EndWith("...");
        }

        [Fact]
        public async Task CreatePost_AutoAssignsCategory()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CreatePostCommand { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 2, Category = "" };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { CustomerId = 1 });
            _mapperMock.Setup(m => m.Map<Post>(It.IsAny<CreatePostCommand>())).Returns(new Post { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 2, Category = "" });
            _postRepoMock.Setup(r => r.Create(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 1, Category = "Farándula" });
            _mapperMock.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns((Post p) => new PostDto { Title = p.Title, Body = p.Body, CustomerId = p.CustomerId, Type = p.Type, Category = p.Type == 1 ? "Farándula" : p.Type == 2 ? "Política" : p.Type == 3 ? "Futbol" : p.Category });
            var result = await handler.Handle(command, CancellationToken.None);
            result.Category.Should().Be("Política");
        }

        [Fact]
        public async Task CreatePost_RepositoryCalledOnce()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CreatePostCommand { Title = "RepoCall", Body = "Body", CustomerId = 1 };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { CustomerId = 1 });
            _mapperMock.Setup(m => m.Map<Post>(It.IsAny<CreatePostCommand>())).Returns(new Post { Title = "RepoCall", Body = "Body", CustomerId = 1 });
            _postRepoMock.Setup(r => r.Create(It.IsAny<Post>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Post { Title = "Test Post", Body = "Body", CustomerId = 1, Type = 1, Category = "Farándula" });
            _mapperMock.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns(new PostDto { Title = "RepoCall", Body = "Body", CustomerId = 1 });
            await handler.Handle(command, CancellationToken.None);
            _postRepoMock.Verify(r => r.Create(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePost_Throws_OnMapperException()
        {
            var handler = new CreatePostCommandHandler(_postRepoMock.Object, _customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CreatePostCommand { Title = "MapError", Body = "Body", CustomerId = 1 };
            _customerRepoMock.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { CustomerId = 1 });
            _mapperMock.Setup(m => m.Map<Post>(It.IsAny<CreatePostCommand>())).Throws(new System.Exception("Map error"));
            await Assert.ThrowsAsync<System.Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
} 