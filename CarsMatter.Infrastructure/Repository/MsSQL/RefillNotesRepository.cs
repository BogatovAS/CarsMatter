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
    using Microsoft.EntityFrameworkCore;

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
            return await Task.Run(() =>
            {
                var myCars = this.dbContext.Users.Include(u => u.MyCars).FirstOrDefault(u => u.Id == userId).MyCars;

                return this.dbContext.RefillNotes.Where(note => myCars.Any(myCar => myCar.Id == note.MyCarId)).ToList();
            });
        }

        public async Task<List<RefillNote>> GetRefillNotesForUserCar(string userId, string userCarId)
        {
            return await Task.Run(() => this.dbContext.RefillNotes.Where(note => userCarId == note.MyCarId).ToList());
        }

        public async Task AddRefillNote(RefillNote refillNote)
        {
            this.dbContext.RefillNotes.Add(refillNote);
            await this.SaveChanges();
        }

        public async Task UpdateRefillNote(RefillNote refillNote)
        {
            var existingNote = this.dbContext.RefillNotes.FirstOrDefault(note => note.Id == refillNote.Id);

            if (existingNote != null)
            {
                existingNote.Date = refillNote.Date;
                existingNote.Location = refillNote.Location;
                existingNote.MyCarId = refillNote.MyCarId;
                existingNote.Odo = refillNote.Odo;
                existingNote.Price = refillNote.Price;
                existingNote.Petrol = refillNote.Petrol;

                this.dbContext.RefillNotes.Update(existingNote);

                await this.SaveChanges();
            }
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
