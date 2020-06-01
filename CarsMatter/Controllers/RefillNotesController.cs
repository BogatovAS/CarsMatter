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

        [HttpGet("notification")]
        public async Task<ActionResult<bool>> NeedToSendNotification([FromQuery] DateTime date, [FromQuery] string userCarId)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                List<RefillNote> refillNotes = await this.refillNotesRepository.GetRefillNotesForUserCar(userId, userCarId);

                DateTime lastRefillNoteCreationDate = refillNotes.OrderByDescending(note => note.Date).FirstOrDefault().Date;

                if (date > lastRefillNoteCreationDate.AddDays(10))
                {
                    return Ok(true);
                }

                return Ok(false);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Возвращает список всех заметок о заправках авторизованного пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefillNote>>> GetAllRefillNotes([FromQuery] string userCarId)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                List<RefillNote> refillNotes = await this.refillNotesRepository.GetRefillNotesForUserCar(userId, userCarId);
                return Ok(refillNotes);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("report")]
        public async Task<ActionResult<RefillNotesReport>> GetReport([FromQuery] string userCarId = null)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                List<RefillNote> refillNotes;

                if (userCarId == null)
                {
                    refillNotes = await this.refillNotesRepository.GetAllRefillNotes(userId);
                }
                else
                {
                    refillNotes = await this.refillNotesRepository.GetRefillNotesForUserCar(userId, userCarId);
                }

                refillNotes = refillNotes.OrderBy(note => note.Date).ToList();

                float totalCost = refillNotes.Sum(n => n.Price);

                RefillNotesReport report = new RefillNotesReport
                {
                    TotalCost = totalCost,
                    CostPerDay = totalCost / (refillNotes.Last().Date - refillNotes.First().Date).Days,
                    CostPerKm = totalCost / refillNotes.Last().Odo - refillNotes.First().Odo
                };

                return Ok(report);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Добавляет заметку о заправке авторизованному пользователю
        /// </summary>
        /// <param name="refillNote"> Заметка для добавления пользователю</param>
        /// <returns>Возвращает результат добавления: добавлена успешно = true, не добавлено = false</returns>
        [HttpPost]
        public async Task<ActionResult<bool>> AddRefillNote([FromBody] RefillNote refillNote)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

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