using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data;
using ProjectWeb.Interfaces;
using ProjectWeb.Models.Entities;

namespace ProjectWeb.Services
{
    public class UtilizatorService:IUtilizatorService
    {
        private readonly ApplicationDB _context;

        public UtilizatorService(ApplicationDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Utilizator>> GetAllUtilizatoriAsync()
        {
            return await _context.Utilizator.ToListAsync();
        }

        public async Task<Utilizator> GetUtilizatorByIdAsync(int id)
        {
            return await _context.Utilizator.FindAsync(id);
        }

        public async Task AddUtilizatorAsync(Utilizator utilizator)
        {
            await _context.Utilizator.AddAsync(utilizator);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUtilizatorAsync(Utilizator utilizator)
        {
            _context.Utilizator.Update(utilizator);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUtilizatorAsync(int id)
        {
            var utilizator = await _context.Utilizator.FindAsync(id);
            if (utilizator != null)
            {
                _context.Utilizator.Remove(utilizator);
                await _context.SaveChangesAsync();
            }
        }
    }
}
