namespace CarsMatter.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using CarsMatter.Infrastructure.Interfaces;
    using System.Collections.Generic;
    using System;
    using Microsoft.Extensions.Logging;
    using CarsMatter.Infrastructure.Models.MsSQL;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using System.Security.Claims;

    [Authorize]
    [Route("api/consumables_notes")]
    [ApiController, Produces("application/json")]
    public class ConsumablesNotesController : ControllerBase
    {
        private readonly IConsumablesNotesRepository<ConsumablesNote> consumablesNotesRepository;

        private readonly ILogger<ConsumablesNotesController> logger;

        public ConsumablesNotesController(
            IConsumablesNotesRepository<ConsumablesNote> consumablesNotesRepository, 
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
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                List<ConsumablesNote> consumablesNotes = await this.consumablesNotesRepository.GetAllConsumablesNotes(userId);
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
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                consumablesNote.UserId = userId;

                await this.consumablesNotesRepository.AddConsumablesNote(consumablesNote);
                return Ok(true);
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
                await this.consumablesNotesRepository.UpdateConsumablesNote(consumablesNote);
                return Ok(true);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteConsumablesNote([FromRoute] string id)
        {
            try
            {
                await this.consumablesNotesRepository.DeleteConsumablesNote(id);
                return Ok(true);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}