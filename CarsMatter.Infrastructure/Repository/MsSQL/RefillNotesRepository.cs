namespace CarsMatter.Infrastructure.Repository
{
    using CarsMatter.Infrastructure.Db;
    using CarsMatter.Infrastructure.Interfaces;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using CarsMatter.Infrastructure.Models.MsSQL;
    using System.Collections.Generic;
    using System.Linq;

    public class RefillNotesRepository : IRefillNotesRepository<RefillNote>
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<RefillNotesRepository> logger;

        public RefillNotesRepository(CarsMatterDbContext dbContext, ILogger<RefillNotesRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<RefillNote>> GetAllRefillNotes(string userId)
        {
            return await Task.Run(() => this.dbContext.RefillNotes.Where(note => note.UserId == userId).ToList());
        }

        public async Task AddRefillNote(RefillNote refillNote)
        {
            this.dbContext.RefillNotes.Add(refillNote);
            await this.SaveChanges();
        }

        public async Task UpdateRefillNote(RefillNote refillNote)
        {
            this.dbContext.RefillNotes.Update(refillNote);
            await this.SaveChanges();
        }

        public async Task DeleteRefillNote(string refillNoteId)
        {
            RefillNote refillNote = this.dbContext.RefillNotes.FirstOrDefault(note => note.Id == refillNoteId);
            this.dbContext.RefillNotes.Remove(refillNote);
            await this.SaveChanges();
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
