using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IConsumablesNotesRepository<T>
    {
        Task<List<T>> GetAllConsumablesNotes(string userId);

        Task<List<T>> GetConsumablesNotesForUserCar(string userId, string userCarId);

        Task AddConsumablesNote(T consumablesNote);

        Task UpdateConsumablesNote(T consumablesNote);

        Task DeleteConsumablesNote(string consumablesNoteId);
    }
}
