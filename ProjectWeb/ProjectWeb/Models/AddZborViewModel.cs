using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWeb.Models
{
    public class AddZborViewModel
    {
        public string? NumeCompanie { get; set; }
        public string? Imbarcare { get; set; }
        public string? Destinatie { get; set; }
        public DateTime? DataPlecare { get; set; }
        
        public decimal GreutateMaximaBagaj { get; set; }
        public int LocuriDisponibile { get; set; }
        public decimal Pret {  get; set; }

        //"In Asteptare", "Anulat", "Confirmat"
        public string? Status { get; set; }
    }
}
