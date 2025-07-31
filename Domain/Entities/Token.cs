using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table(name: "Token")]
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "IdToken")]
        public int Id { get; set; }

        [Column(name: "IdUsuario")]
        public int IdUsuario { get; set; }

        [Column(name: "Token", TypeName = "Varchar (MAX)")]
        public string? TokenValue { get; set; }

        [Column(name: "FechaCreacion")]
        public DateTime CreatedDate { get; set; }

        [Column(name: "FechaVencimiento")]
        public DateTime ExpirationDate { get; set; }

        [Column(name: "Estado")]
        public bool Status { get; set; }
    }
}
