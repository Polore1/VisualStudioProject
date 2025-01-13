using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data;
using ProjectWeb.Interfaces;
using ProjectWeb.Models.Entities;

namespace ProjectWeb.Services
{
    public class CheckinService : ICheckinService
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

        public async Task<(decimal taxaSuplimentara, string mesaj)> CalculeazaTaxaBagaj(Checkin checkin)
        {
            if (checkin.Zbor == null)
                throw new InvalidOperationException("Detaliile zborului lipsesc.");

            if (checkin.GreutateBagaj > checkin.Zbor.GreutateMaximaBagaj)
            {
                decimal kgExces = checkin.GreutateBagaj - checkin.Zbor.GreutateMaximaBagaj;
                decimal taxaSuplimentara = kgExces * checkin.Zbor.TaxaSuplimentara;

                // Asigură-te că taxa suplimentară este afișată corect (formatată în EUR)
                var taxaSuplimentaraFormatted = string.Format("{0:N2}", taxaSuplimentara); // Formatează cu două zecimale


                string mesaj = $"O taxă suplimentară de {taxaSuplimentara} EUR a fost calculată pentru bagajul în exces (exces: {kgExces} kg).";
                return (taxaSuplimentara, mesaj);
            }

            return (0m, "Fără taxe suplimentare pentru bagaj.");
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
            //verificare zbor valid
            if (checkin.Zbor == null)
                checkin.Zbor = await _dbContext.Zbor.FindAsync(checkin.IdZbor);

            if (checkin.Zbor == null)
                throw new InvalidOperationException("Zborul nu poate fi găsit.");

            //calcul taxa suprlimentara
            var (taxaSuplimentara, _) = await CalculeazaTaxaBagaj(checkin);

            checkin.PretFinal = checkin.Zbor.Pret + taxaSuplimentara;

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
        public async Task UpdateCheckinAsync(Checkin checkin)
        {
            _dbContext.Checkin.Update(checkin);
            await _dbContext.SaveChangesAsync();
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
