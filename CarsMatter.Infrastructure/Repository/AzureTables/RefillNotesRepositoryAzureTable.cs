using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.AzureTables;

namespace CarsMatter.Infrastructure.Repository.AzureTables
{
    public class RefillNotesRepositoryAzureTable : IRefillNotesRepository<RefillNote>
    {
        IAzureTable<RefillNote> refillNotesTable;

        public RefillNotesRepositoryAzureTable(IAzureTable<RefillNote> refillNotesTable)
        {
            this.refillNotesTable = refillNotesTable;
        }

        public Task AddRefillNote(RefillNote refillNote)
        {
            refillNote.Id = Guid.NewGuid().ToString();
            refillNote.PartitionKey = refillNote.Id;
            refillNote.RowKey = refillNote.Id;

            return this.refillNotesTable.Insert(refillNote);
        }

        public Task DeleteRefillNote(string refillNoteId)
        {
            return this.refillNotesTable.Delete(refillNoteId, refillNoteId);
        }

        public Task<List<RefillNote>> GetAllRefillNotes(string userId)
        {
            return this.refillNotesTable.GetList(nameof(RefillNote.UserId), userId);
        }

        public Task UpdateRefillNote(RefillNote refillNote)
        {
            return this.refillNotesTable.Update(refillNote);
        }
    }
}
