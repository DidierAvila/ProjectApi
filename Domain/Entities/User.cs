using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table(name: "User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "Id")]
        public int Id { get; set; }

        [Column(name: "Name", TypeName = "Varchar (255)")]
        public required string Name { get; set; }

        [Column(name: "LastName", TypeName = "Varchar (255)")]
        public required string LastName { get; set; }

        [Column(name: "Email", TypeName = "Varchar (100)")]
        public required string Email { get; set; }

        [Column(name: "Password", TypeName = "Varchar (255)")]
        public required string Password { get; set; }

        [Column(name: "Rol", TypeName = "Varchar (255)")]
        public required string Role { get; set; }

        [Column(name: "Phone", TypeName = "Varchar (20)")]
        public string? Phone { get; set; }
    }
}
