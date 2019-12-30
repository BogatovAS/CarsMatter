using CarsMatter.Infrastructure.Models.Journal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IRefillNotesRepository
    {
        Task<List<RefillNote>> GetAllRefillNotes();

        Task<bool> AddRefillNote(RefillNote refillNote);

        Task<bool> UpdateRefillNote(RefillNote refillNote);

        Task<bool> DeleteRefillNote(int refillNoteId);
    }
}
