using AutoMapper;
using Business.Posts.Commands;
using Domain.Dtos;

namespace Business.Posts.Mappings
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            // Mapeo de Post a PostDto
            CreateMap<Domain.Entities.Post, PostDto>();
            
            // Mapeo de PostDto a Post (para actualizaciones)
            CreateMap<PostDto, Domain.Entities.Post>();
            
            // Mapeo de CreatePostCommand a Post
            CreateMap<CreatePostCommand, Domain.Entities.Post>()
                .ForMember(dest => dest.PostId, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());

            // Mapeo de UpdatePostCommand a Post
            CreateMap<UpdatePostCommand, Domain.Entities.Post>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore());

            // Mapeo de CreatePostRequest a Post (para múltiples posts)
            CreateMap<CreatePostRequest, Domain.Entities.Post>()
                .ForMember(dest => dest.PostId, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());

            // Mapeo de CreatePostDto a CreatePostCommand
            CreateMap<CreatePostDto, CreatePostCommand>();
            
            // Mapeo de UpdatePostDto a UpdatePostCommand
            CreateMap<UpdatePostDto, UpdatePostCommand>();
        }
    }
} 