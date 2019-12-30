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

    public class RefillNotesRepository : IRefillNotesRepository
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<RefillNotesRepository> logger;

        public RefillNotesRepository(CarsMatterDbContext dbContext, ILogger<RefillNotesRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public Task<List<RefillNote>> GetAllRefillNotes()
        {
            return Task.Run(() => this.dbContext.RefillNotes.ToList());
        }

        public async Task<bool> AddRefillNote(RefillNote refillNote)
        {
            this.dbContext.RefillNotes.Add(refillNote);
            return await this.SaveChanges();
        }

        public async Task<bool> UpdateRefillNote(RefillNote refillNote)
        {
            this.dbContext.RefillNotes.Update(refillNote);
            return await this.SaveChanges();
        }

        public async Task<bool> DeleteRefillNote(int refillNoteId)
        {
            RefillNote refillNote = this.dbContext.RefillNotes.FirstOrDefault(note => note.Id == refillNoteId);
            this.dbContext.RefillNotes.Remove(refillNote);
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
