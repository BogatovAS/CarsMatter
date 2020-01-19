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
    [Route("api/refill_notes")]
    [ApiController, Produces("application/json")]
    public class RefillNotesController : ControllerBase
    {
        private readonly IRefillNotesRepository refillNotesRepository;

        private readonly ILogger<RefillNotesController> logger;

        public RefillNotesController(
            IRefillNotesRepository refillNotesRepository, 
            ILogger<RefillNotesController> logger)
        {
            this.refillNotesRepository = refillNotesRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefillNote>>> GetAllRefillNotes()
        {
            try
            {
                List<RefillNote> refillNotes = await this.refillNotesRepository.GetAllRefillNotes();
                return Ok(refillNotes);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddRefillNote([FromBody] RefillNote refillNote)
        {
            try
            {
                bool response = await this.refillNotesRepository.AddRefillNote(refillNote);
                return Ok(response);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateRefillNote([FromBody] RefillNote refillNote)
        {
            try
            {
                bool response = await this.refillNotesRepository.UpdateRefillNote(refillNote);
                return Ok(response);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteRefillNote([FromRoute] int id)
        {
            try
            {
                bool response = await this.refillNotesRepository.DeleteRefillNote(id);
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