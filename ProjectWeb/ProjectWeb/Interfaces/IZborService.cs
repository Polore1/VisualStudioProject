using ProjectWeb.Models.Entities;

namespace ProjectWeb.Interfaces
{
    public interface IZborService
    {
        Task<IEnumerable<Zbor>> GetAllZboruriAsync(); // Adăugarea metodei
        Task<Zbor> GetZborByIdAsync(int id);
        Task AddZborAsync(Zbor zbor);
        Task UpdateZborAsync(Zbor zbor);
        Task DeleteZborAsync(int id);
        Task<List<Zbor>> GetAllAsync();
        Task AddAsync(Zbor zbor);
        Task<Zbor?> GetByIdAsync(int id);
        Task UpdateAsync(Zbor zbor);
        Task DeleteAsync(int id);
        Task SoftDeleteZborAsync(int id);
    }
}
