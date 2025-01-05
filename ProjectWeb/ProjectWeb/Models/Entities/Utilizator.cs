using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWeb.Models.Entities
{
    public class Utilizator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUtilizator { get; set; }
        public string? Nume { get; set; }
        public string? Email { get; set; }

        public string? Parola { get; set; }
        public bool Pasager { get; set; }

        // Legătură cu Zbor
        public int? IdZbor { get; set; }
        [ForeignKey("IdZbor")]
        public virtual Zbor? Zbor { get; set; }
    }
}
