using ProjectWeb.Models.Entities;

namespace ProjectWeb.Interfaces
{
    public interface ICheckinService
    {
        Task<IEnumerable<Checkin>> GetAllCheckinsAsync(); // Listarea check-in-urilor
        Task<Checkin> GetCheckinByIdAsync(int id); // Obținerea unui check-in pe baza ID-ului
        Task AddCheckinAsync(Checkin checkin); // Adăugarea unui check-in
        Task<bool> ValidateCheckinAsync(Checkin checkin); // Validarea check-in-ului

        Task DeleteCheckinByIdAsync(int id);
    }
}
