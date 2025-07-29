using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        
        // Propiedad de navegación
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
