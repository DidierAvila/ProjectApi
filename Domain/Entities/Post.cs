using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities
{
    [Table(name: "Post")]
    public partial class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public string Category { get; set; }
        public int CustomerId { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
        
        // Propiedad de navegación
        public virtual Customer Customer { get; set; }
    }
}
