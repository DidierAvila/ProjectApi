namespace Domain.Entities
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public string Category { get; set; }
        public int CustomerId { get; set; }
        
        // Propiedad de navegación
        public virtual Customer Customer { get; set; }
    }
}
