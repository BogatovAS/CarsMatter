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
    [Route("api/refill_notes")]
    [ApiController, Produces("application/json")]
    public class RefillNotesController : ControllerBase
    {
        private readonly IRefillNotesRepository<RefillNote> refillNotesRepository;

        private readonly ILogger<RefillNotesController> logger;

        public RefillNotesController(
            IRefillNotesRepository<RefillNote> refillNotesRepository,
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
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                List<RefillNote> refillNotes = await this.refillNotesRepository.GetAllRefillNotes(userId);
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
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                refillNote.UserId = userId;

                await this.refillNotesRepository.AddRefillNote(refillNote);
                return Ok(true);
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
                await this.refillNotesRepository.UpdateRefillNote(refillNote);
                return Ok(true);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteRefillNote([FromRoute] string id)
        {
            try
            {
                await this.refillNotesRepository.DeleteRefillNote(id);
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