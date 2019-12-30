using CarsMatter.Infrastructure.Models.Journal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IConsumablesNotesRepository
    {
        Task<List<ConsumablesNote>> GetAllConsumablesNotes();

        Task<bool> AddConsumablesNote(ConsumablesNote consumablesNote);

        Task<bool> UpdateConsumablesNote(ConsumablesNote consumablesNote);

        Task<bool> DeleteConsumablesNote(int consumablesNoteId);
    }
}
