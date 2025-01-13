using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Models.Entities
{
    public class Checkin
    {
        [Key]
        public int IdCheckin { get; set; }

        // Legătura cu Utilizator
        public int IdUtilizator { get; set; }
        [ForeignKey("IdUtilizator")]
        public virtual Utilizator? Utilizator { get; set; }

        // Legătura cu Zbor
        
        public int IdZbor { get; set; }
        [ForeignKey("IdZbor")]
        public virtual Zbor? Zbor { get; set; }

       

        [Column(TypeName = "decimal(5,2)")]
        public decimal GreutateBagaj { get; set; }


        public DateTime DataCheckin { get; set; }

        // Locul rezervat
        public string? LocRezervat { get; set; }


        //Validează dacă check-in-ul este complet
        [NotMapped] // Nu este mapată în baza de date
        public bool EsteValid { get; set; }
        //{
        //    get
        //    {
        //        // Dacă Zbor sau Utilizator nu sunt încărcate, validarea nu se poate face
        //        if (Zbor == null || Utilizator == null)
        //        {
        //            return false;
        //        }

        //        // Validare greutate bagaj
        //        bool greutateValida = GreutateBagaj <= Zbor.GreutateMaximaBagaj;

        //        // Validare data check-in (trebuie să fie înainte de plecare)
        //        bool dataValida = DataCheckin < Zbor.DataPlecare;

        //        // Loc rezervat trebuie să fie valid (dacă locul e null, e invalid)
        //        bool locValid = !string.IsNullOrEmpty(LocRezervat);

        //        // Check-in este valid dacă toate condițiile sunt îndeplinite
        //        return greutateValida && dataValida && locValid;
        //    }
        //}


        [Column(TypeName = "decimal(5,2)")]
        public decimal PretFinal { get; set; }
        public bool IsValid { get; internal set; }
    }
}
