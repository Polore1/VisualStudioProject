using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data;
using ProjectWeb.Interfaces;
using ProjectWeb.Models.Entities;

namespace ProjectWeb.Services
{
    public class CheckinService:ICheckinService
    {
        private readonly ApplicationDB _dbContext;

        public CheckinService(ApplicationDB dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Checkin>> GetAllCheckinsAsync()
        {
            return await _dbContext.Checkin
                .Include(c => c.Utilizator)
                .Include(c => c.Zbor)
                .ToListAsync();
        }

        public async Task<decimal> CalculeazaTaxaBagaj(Checkin checkin)
        {
            // Verificăm dacă greutatea bagajului depășește greutatea maximă permisă
            if (checkin.GreutateBagaj > checkin.Zbor.GreutateMaximaBagaj)
            {
                // Calculăm taxa pentru fiecare kg în exces (50 EUR pe kg)
                decimal kgExces = checkin.GreutateBagaj - checkin.Zbor.GreutateMaximaBagaj;
                decimal taxaSuplimentara = kgExces * 50; // 50 EUR pentru fiecare kg în exces
                return taxaSuplimentara;
            }

            // Nu există taxe suplimentare dacă greutatea bagajului este validă
            return 0m;
        }
        public decimal CalculeazaPretTotal(Checkin checkin)
        {
            if (checkin.Zbor == null) throw new InvalidOperationException("Zborul nu poate fi null.");

            // Prețul de bază al zborului
            decimal pretBaza = checkin.Zbor.Pret;

            // Calculăm taxa suplimentară dacă greutatea bagajului depășește limita
            decimal taxaSuplimentara = 0m;
            if (checkin.GreutateBagaj > checkin.Zbor.GreutateMaximaBagaj)
            {
                taxaSuplimentara = checkin.Zbor.TaxaSuplimentara;
            }

            // Prețul total este prețul bazei plus taxa suplimentară
            return pretBaza + taxaSuplimentara;
        }



        public async Task<Checkin> GetCheckinByIdAsync(int id)
        {
            return await _dbContext.Checkin
                .Include(c => c.Utilizator)
                .Include(c => c.Zbor)
                .FirstOrDefaultAsync(c => c.IdCheckin == id);
        }

        public async Task AddCheckinAsync(Checkin checkin)
        {
            // Adăugăm taxa suplimentară la check-in
            checkin.PretFinal = await CalculeazaTaxaBagaj(checkin);

            _dbContext.Checkin.Add(checkin);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ValidateCheckinAsync(Checkin checkin)
        {
            // Validarea check-in-ului
            if (checkin.Zbor == null || checkin.Utilizator == null)
                return false;

            bool greutateValida = checkin.GreutateBagaj <= checkin.Zbor.GreutateMaximaBagaj;
            bool dataValida = checkin.DataCheckin < checkin.Zbor.DataPlecare;
            bool locValid = !string.IsNullOrEmpty(checkin.LocRezervat);

            return greutateValida && dataValida && locValid;
        }

        public async Task DeleteCheckinByIdAsync(int id)
        {
            var checkin = await _dbContext.Checkin.FindAsync(id);
            if (checkin != null)
            {
                _dbContext.Checkin.Remove(checkin);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
