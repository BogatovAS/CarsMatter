﻿namespace CarsMatter.Infrastructure.Repository
{
    using CarsMatter.Infrastructure.Db;
    using CarsMatter.Infrastructure.Interfaces;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using CarsMatter.Infrastructure.Models.MsSQL;
    using System.Collections.Generic;
    using System.Linq;

    public class ConsumablesNotesRepository : IConsumablesNotesRepository<ConsumablesNote>
    {
        private readonly CarsMatterDbContext dbContext;

        private readonly ILogger<ConsumablesNotesRepository> logger;

        public ConsumablesNotesRepository(CarsMatterDbContext dbContext, ILogger<ConsumablesNotesRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<ConsumablesNote>> GetAllConsumablesNotes(string userId)
        {
            return await Task.Run(() => this.dbContext.ConsumablesNotes.Where(note => note.UserId == userId).ToList());
        }

        public async Task AddConsumablesNote(ConsumablesNote consumablesNote)
        {
            this.dbContext.ConsumablesNotes.Add(consumablesNote);
            await this.SaveChanges();
        }

        public async Task UpdateConsumablesNote(ConsumablesNote consumablesNote)
        {
            this.dbContext.ConsumablesNotes.Update(consumablesNote);
            await this.SaveChanges();
        }

        public async Task DeleteConsumablesNote(string consumablesNoteId)
        {
            ConsumablesNote consumablesNote = this.dbContext.ConsumablesNotes.FirstOrDefault(consumablesNote => consumablesNote.Id == consumablesNoteId);
            this.dbContext.ConsumablesNotes.Remove(consumablesNote);
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