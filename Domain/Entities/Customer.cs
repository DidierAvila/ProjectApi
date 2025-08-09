using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    [Table(name: "Customer")]
    public partial class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
        
        // Propiedad de navegación
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
