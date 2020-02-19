using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.AzureTables;

namespace CarsMatter.Infrastructure.Repository.AzureTables
{
    public class ConsumablesNotesRepositoryAzureTable : IConsumablesNotesRepository<ConsumablesNote>
    {
        IAzureTable<ConsumablesNote> consumablesNoteTable;

        public ConsumablesNotesRepositoryAzureTable(IAzureTable<ConsumablesNote> consumablesNoteTable)
        {
            this.consumablesNoteTable = consumablesNoteTable;
        }

        public Task AddConsumablesNote(ConsumablesNote consumablesNote)
        {
            consumablesNote.Id = Guid.NewGuid().ToString();
            consumablesNote.PartitionKey = consumablesNote.Id;
            consumablesNote.RowKey = consumablesNote.Id;

            return this.consumablesNoteTable.Insert(consumablesNote);
        }

        public Task DeleteConsumablesNote(string consumablesNoteId)
        {
            return this.consumablesNoteTable.Delete(consumablesNoteId, consumablesNoteId);
        }

        public Task<List<ConsumablesNote>> GetAllConsumablesNotes(string userId)
        {
            return this.consumablesNoteTable.GetList(nameof(ConsumablesNote.UserId), userId);
        }

        public Task UpdateConsumablesNote(ConsumablesNote consumablesNote)
        {
            return this.consumablesNoteTable.Update(consumablesNote);
        }
    }
}
