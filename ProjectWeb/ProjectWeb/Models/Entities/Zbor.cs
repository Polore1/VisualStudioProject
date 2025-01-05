using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace ProjectWeb.Models.Entities
{
    [Serializable]
    public class Zbor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdZbor { get; set; }
        public string? NumeCompanie { get; set; }
      
        public string? Imbarcare { get; set; }
        public string? Destinatie { get; set; }
        public DateTime? DataPlecare { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal GreutateMaximaBagaj { get; set; } 

        public int LocuriDisponibile { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Pret { get; set; } // Prețul zborului

        public decimal TaxaSuplimentara { get; set; }

        //Anulat, Finalizat și Planificat
        public string? Status { get; set; }


        // Proprietate pentru ștergerea logică
        public bool IsDeleted { get; set; } = false;
    }
}
