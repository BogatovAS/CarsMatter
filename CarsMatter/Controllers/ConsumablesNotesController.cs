namespace CarsMatter.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using CarsMatter.Infrastructure.Interfaces;
    using System.Collections.Generic;
    using System;
    using Microsoft.Extensions.Logging;
    using CarsMatter.Infrastructure.Models.Journal;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [Route("api/consumables_notes")]
    [ApiController, Produces("application/json")]
    public class ConsumablesNotesController : ControllerBase
    {
        private readonly IConsumablesNotesRepository consumablesNotesRepository;

        private readonly ILogger<ConsumablesNotesController> logger;

        public ConsumablesNotesController(
            IConsumablesNotesRepository consumablesNotesRepository, 
            ILogger<ConsumablesNotesController> logger)
        {
            this.consumablesNotesRepository = consumablesNotesRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefillNote>>> GetAllConsumablesNotes()
        {
            try
            {
                List<ConsumablesNote> consumablesNotes = await this.consumablesNotesRepository.GetAllConsumablesNotes();
                return Ok(consumablesNotes);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddConsumablesNote([FromBody] ConsumablesNote consumablesNote)
        {
            try
            {
                bool response = await this.consumablesNotesRepository.AddConsumablesNote(consumablesNote);
                return Ok(response);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateConsumablesNote([FromBody] ConsumablesNote consumablesNote)
        {
            try
            {
                bool response = await this.consumablesNotesRepository.UpdateConsumablesNote(consumablesNote);
                return Ok(response);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteConsumablesNote([FromRoute] int id)
        {
            try
            {
                bool response = await this.consumablesNotesRepository.DeleteConsumablesNote(id);
                return Ok(response);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}