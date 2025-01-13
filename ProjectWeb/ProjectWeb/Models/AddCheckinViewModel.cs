using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Models
{
    public class AddCheckinViewModel
    {
        //IdCheckin

        // Selectarea utilizatorului
        [Required(ErrorMessage = "Trebuie să selectați un utilizator.")]
        public int IdUtilizator { get; set; }
        public List<SelectListItem>? Utilizatori { get; set; } // Pentru dropdown-ul de utilizatori

        // Selectarea zborului
        [Required(ErrorMessage = "Trebuie să selectați un zbor.")]
        public int IdZbor { get; set; }
        public List<SelectListItem>? Zboruri { get; set; } // Pentru dropdown-ul de zboruri


        // Greutatea bagajului
        [Required(ErrorMessage = "Greutatea bagajului este obligatorie.")]
        public decimal GreutateBagaj { get; set; }

        // Data check-in-ului (setată implicit la data curentă)
        public DateTime DataCheckin { get; set; } = DateTime.Now;


        // Loc rezervat
        [Required(ErrorMessage = "Trebuie să selectați un loc.")]
        public string? LocRezervat { get; set; }

        public decimal PretFinal { get; set; }
        public int idCheckin { get; internal set; }

        //EsteValid

    }
}
