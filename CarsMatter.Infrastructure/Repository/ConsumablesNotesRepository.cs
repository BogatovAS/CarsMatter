namespace CarsMatter.Infrastructure.Repository
{
    using CarsMatter.Infrastructure.Db;
    using CarsMatter.Infrastructure.Interfaces;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using CarsMatter.Infrastructure.Models.Journal;
    using System.Collections.Generic;
    using System.Linq;

    public class ConsumablesNotesRepository : IConsumablesNotesRepository
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<ConsumablesNotesRepository> logger;

        public ConsumablesNotesRepository(CarsMatterDbContext dbContext, ILogger<ConsumablesNotesRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<ConsumablesNote>> GetAllConsumablesNotes()
        {
            return await Task.Run(() => this.dbContext.ConsumablesNotes.ToList());
        }

        public async Task<bool> AddConsumablesNote(ConsumablesNote consumablesNote)
        {
            this.dbContext.ConsumablesNotes.Add(consumablesNote);
            return await this.SaveChanges();
        }

        public async Task<bool> UpdateConsumablesNote(ConsumablesNote consumablesNote)
        {
            this.dbContext.ConsumablesNotes.Update(consumablesNote);
            return await this.SaveChanges();
        }

        public async Task<bool> DeleteConsumablesNote(int consumablesNoteId)
        {
            ConsumablesNote consumablesNote = this.dbContext.ConsumablesNotes.FirstOrDefault(consumablesNote => consumablesNote.Id == consumablesNoteId);
            this.dbContext.ConsumablesNotes.Remove(consumablesNote);
            return await this.SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
