using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IRefillNotesRepository<T>
    {
        Task<List<T>> GetAllRefillNotes(string userId);

        Task AddRefillNote(T refillNote);

        Task UpdateRefillNote(T refillNote);

        Task DeleteRefillNote(string refillNoteId);
    }
}
