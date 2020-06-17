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
        private readonly IUserService userService;
        private readonly ILogger<RefillNotesController> logger;

        public RefillNotesController(
            IRefillNotesRepository<RefillNote> refillNotesRepository,
            IUserService userService,
            ILogger<RefillNotesController> logger)
        {
            this.refillNotesRepository = refillNotesRepository;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet("notification")]
        public async Task<ActionResult<bool>> NeedToSendNotification([FromQuery] DateTime date, [FromQuery] string userCarId)
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                MyCar selectedCar = await this.userService.GetSelectedCar(userId);

                List<RefillNote> refillNotes = await this.refillNotesRepository.GetRefillNotesForUserCar(userId, selectedCar.Id);

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

                MyCar selectedCar = await this.userService.GetSelectedCar(userId);

                List<RefillNote> refillNotes = await this.refillNotesRepository.GetRefillNotesForUserCar(userId, selectedCar.Id);
                return Ok(refillNotes.OrderByDescending(note => note.Date));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("report")]
        public async Task<ActionResult<RefillNotesReport>> GetReport()
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                MyCar selectedCar = await this.userService.GetSelectedCar(userId);

                List<RefillNote> refillNotes;

                if (selectedCar == null)
                {
                    refillNotes = await this.refillNotesRepository.GetAllRefillNotes(userId);
                }
                else
                {
                    refillNotes = await this.refillNotesRepository.GetRefillNotesForUserCar(userId, selectedCar.Id);
                }

                refillNotes = refillNotes.OrderBy(note => note.Date).ToList();

                float totalCost = refillNotes.Sum(n => n.Price);

                var totalPetrol = refillNotes.Sum(n => n.Petrol);
                var averagePetrol = totalPetrol / (refillNotes.Last().Odo - refillNotes.First().Odo) * 100;

                RefillNotesReport report = new RefillNotesReport
                {
                    TotalCost = totalCost,
                    CostPerDay = totalCost / (refillNotes.Last().Date - refillNotes.First().Date).Days,
                    CostPerKm = totalCost / (refillNotes.Last().Odo - refillNotes.First().Odo),
                    TotalVolume = totalPetrol,
                    AverageVolume = averagePetrol
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

                MyCar selectedCar = await this.userService.GetSelectedCar(userId);

                refillNote.MyCarId = selectedCar.Id;

                var allNotes = await this.refillNotesRepository.GetRefillNotesForUserCar(userId, selectedCar.Id);
                allNotes = allNotes.OrderByDescending(note => note.Date).ToList();

                if (allNotes.Any() && allNotes.First().Odo > refillNote.Odo && allNotes.First().Date < refillNote.Date)
                {
                    return this.BadRequest("Текущий пробег не может быть меньше пробега в последней записи");
                }

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
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                MyCar selectedCar = await this.userService.GetSelectedCar(userId);

                refillNote.MyCarId = selectedCar.Id;

                var allNotes = await this.refillNotesRepository.GetRefillNotesForUserCar(userId, selectedCar.Id);
                allNotes = allNotes.OrderByDescending(note => note.Date).ToList();

                if (allNotes.Any() && refillNote.Date > allNotes.First().Date)
                {
                    if (refillNote.Odo < allNotes.First().Odo)
                    {
                        return this.BadRequest("Текущий пробег не может быть меньше пробега в последней записи");
                    }
                }
                if (allNotes.Any() && refillNote.Odo > allNotes.First().Odo)
                {
                    if (refillNote.Date < allNotes.First().Date)
                    {
                        return this.BadRequest("Дата текущей заправки не может быть меньше даты в последней записи");
                    }
                }

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