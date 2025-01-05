using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectWeb.Data;
using ProjectWeb.Interfaces;
using ProjectWeb.Models.Entities;

namespace ProjectWeb.Services
{
    public class ZborService : IZborService
    {
        private readonly ApplicationDB _dbContext;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ZborService> _logger;

        public ZborService(ApplicationDB context, ICacheService cacheService, ILogger<ZborService> logger)
        {
            _dbContext = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<IEnumerable<Zbor>> GetAllZboruriAsync()
        {
            // Cheia pentru lista completă de zboruri
            string cacheKey = "zboruri_list";

            // Verificare dacă lista este deja în cache
            if (_cacheService.IsSet(cacheKey))
            {
                return _cacheService.Get<List<Zbor>>(cacheKey);
            }

            // Dacă nu este în cache, preluăm din baza de date
            var zboruri = await _dbContext.Zbor
                .Where(z => !z.IsDeleted)
                .ToListAsync();

            // Adăugăm lista în cache cu timp de expirare de 30 de minute
            _cacheService.Set(cacheKey, zboruri, 30);

            return zboruri;
            //return await _dbContext.Zbor
            //    .Where(z => !z.IsDeleted)
            //    .ToListAsync();
        }

        public async Task<Zbor?> GetZborByIdAsync(int id)
        {
            // Cheia unică pentru cache
            string cacheKey = $"zbor_{id}";

            // Verificare dacă datele sunt în cache
            if (_cacheService.IsSet(cacheKey))
            {
                return _cacheService.Get<Zbor>(cacheKey);
            }

            // Dacă nu sunt în cache, preia din baza de date
            var zbor = await _dbContext.Zbor
                .Where(z => !z.IsDeleted && z.IdZbor == id)
                .FirstOrDefaultAsync();

            // Stochează în cache pentru utilizări viitoare
            if (zbor != null)
            {
                _cacheService.Set(cacheKey, zbor);
            }

            return zbor;
            //return await _dbContext.Zbor
            //    .Where(z => !z.IsDeleted && z.IdZbor == id)
            //    .FirstOrDefaultAsync();
        }

        public async Task AddZborAsync(Zbor zbor)
        {
            await _dbContext.Zbor.AddAsync(zbor);
            await _dbContext.SaveChangesAsync();

            // Resetare cache
            _cacheService.Remove("zboruri_list");
        }

        public async Task UpdateZborAsync(Zbor zbor)
        {
            _dbContext.Zbor.Update(zbor);
            await _dbContext.SaveChangesAsync();

            // Resetare cache
            _cacheService.Remove($"zbor_{zbor.IdZbor}");
            _cacheService.Remove("zboruri_list");
        }

        public async Task DeleteZborAsync(int id)
        {
            var zbor = await _dbContext.Zbor.FirstOrDefaultAsync(z => z.IdZbor == id);
            if (zbor != null)
            {
                _dbContext.Zbor.Remove(zbor);
                await _dbContext.SaveChangesAsync();

                // Resetare cache
                _cacheService.Remove($"zbor_{id}");
                _cacheService.Remove("zboruri_list");
            }
        }
        public async Task SoftDeleteZborAsync(int id)
        {
            try
            {
                _logger.LogInformation("Ștergerea logică a zborului cu ID {ZborId}.", id);
                var zbor = await _dbContext.Zbor.FindAsync(id);
                if (zbor != null)
                {
                    zbor.IsDeleted = true;
                    await _dbContext.SaveChangesAsync();
                    // Resetare cache
                    _cacheService.Remove("zboruri_list");
                    _logger.LogInformation("Zborul cu ID {ZborId} a fost șters cu succes.", id);
                }
                else
                {
                    _logger.LogWarning("Zborul cu ID {ZborId} nu a fost găsit pentru ștergere.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la ștergerea zborului cu ID {ZborId}.", id);
                throw;
            }
        }

        //var zbor = await _dbContext.Zbor.FindAsync(id);
        //    if (zbor != null)
        //    {
        //        zbor.IsDeleted = true;
        //        await _dbContext.SaveChangesAsync();

        //        // Resetare cache
        //        _cacheService.Remove("zboruri_list");
        //    }
        //}

        // Implementarea altor metode care nu sunt duplicate:
        public async Task AddAsync(Zbor zbor)
        {
            await AddZborAsync(zbor);
        }

        public async Task<Zbor?> GetByIdAsync(int id)
        {
            return await GetZborByIdAsync(id);
        }

        public async Task UpdateAsync(Zbor zbor)
        {
            await UpdateZborAsync(zbor);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteZborAsync(id);
        }

        public Task<List<Zbor>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
