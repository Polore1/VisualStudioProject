using ProjectWeb.Models.Entities;

namespace ProjectWeb.Models
{
    public class AddUtilizatorViewModel
    {
        public string? Nume { get; set; }
        public string? Email { get; set; }
        public string? Parola { get; set; }
        public bool Pasager { get; set; }
    }
}
