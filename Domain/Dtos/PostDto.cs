namespace Domain.Dtos
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public string Category { get; set; }
        public int CustomerId { get; set; }
    }

    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public string Category { get; set; }
        public int CustomerId { get; set; }
    }

    public class UpdatePostDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public string Category { get; set; }
        public int CustomerId { get; set; }
    }
} 