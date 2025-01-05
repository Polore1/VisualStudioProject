using ProjectWeb.Models.Entities;

namespace ProjectWeb.Interfaces
{
    public interface IUtilizatorService
    {
        Task<IEnumerable<Utilizator>> GetAllUtilizatoriAsync(); 
        Task<Utilizator> GetUtilizatorByIdAsync(int id);         
        Task AddUtilizatorAsync(Utilizator utilizator);          
        Task UpdateUtilizatorAsync(Utilizator utilizator);      
        Task DeleteUtilizatorAsync(int id);
    }
}
