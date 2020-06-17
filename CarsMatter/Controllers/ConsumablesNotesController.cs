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
        private readonly IUserService userService;

        private readonly ILogger<ConsumablesNotesController> logger;

        public ConsumablesNotesController(
            IConsumablesNotesRepository<ConsumablesNote> consumablesNotesRepository,
            IUserService userService,
            ILogger<ConsumablesNotesController> logger)
        {
            this.consumablesNotesRepository = consumablesNotesRepository;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefillNote>>> GetAllConsumablesNotes()
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                MyCar selectedCar = await this.userService.GetSelectedCar(userId);

                List<ConsumablesNote> consumablesNotes = await this.consumablesNotesRepository.GetConsumablesNotesForUserCar(userId, selectedCar.Id);
                return Ok(consumablesNotes.OrderByDescending(note => note.Date));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("kindOfServices")]
        public async Task<ActionResult<IEnumerable<KindOfService>>> GetKindOfServices()
        {
            try
            {
                List<KindOfService> kindOfServices = await this.consumablesNotesRepository.GetKindOfServices();
                return Ok(kindOfServices);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("report")]
        public async Task<ActionResult<ConsumablesNotesReport>> GetReport()
        {
            try
            {
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                MyCar selectedCar = await this.userService.GetSelectedCar(userId);

                List<ConsumablesNote> consumablesNotes;

                if (selectedCar == null)
                {
                    consumablesNotes = await this.consumablesNotesRepository.GetAllConsumablesNotes(userId);
                }
                else 
                {
                    consumablesNotes = await this.consumablesNotesRepository.GetConsumablesNotesForUserCar(userId, selectedCar.Id);
                }

                consumablesNotes = consumablesNotes.OrderBy(note => note.Date).ToList();

                int totalDays = (consumablesNotes.Last().Date - consumablesNotes.First().Date).Days;

                int totalKms = consumablesNotes.Last().Odo - consumablesNotes.First().Odo;

                float totalCost = consumablesNotes.Sum(n => n.Price);

                ConsumablesNotesReport report = new ConsumablesNotesReport
                {
                    TotalCost = totalCost,
                    CostPerDay = totalCost / totalDays,
                    CostPerKm = totalCost / totalKms
                };
                
                return Ok(report);
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

                MyCar selectedCar = await this.userService.GetSelectedCar(userId);

                consumablesNote.MyCarId = selectedCar.Id;

                var allNotes = await this.consumablesNotesRepository.GetConsumablesNotesForUserCar(userId, selectedCar.Id);
                allNotes = allNotes.OrderByDescending(note => note.Date).ToList();

                if(allNotes.Any() && allNotes.First().Odo > consumablesNote.Odo && allNotes.First().Date < consumablesNote.Date)
                {
                    return this.BadRequest("Текущий пробег не может быть меньше пробега в предыдущей записи");
                }

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
                string userId = this.Request.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                MyCar selectedCar = await this.userService.GetSelectedCar(userId);

                consumablesNote.MyCarId = selectedCar.Id;

                var allNotes = await this.consumablesNotesRepository.GetConsumablesNotesForUserCar(userId, selectedCar.Id);
                allNotes = allNotes.OrderByDescending(note => note.Date).ToList();

                if (allNotes.Any() && consumablesNote.Date > allNotes.First().Date)
                {
                    if (consumablesNote.Odo < allNotes.First().Odo)
                    {
                        return this.BadRequest("Текущий пробег не может быть меньше пробега в последней записи");
                    }
                }
                if (allNotes.Any() && consumablesNote.Odo > allNotes.First().Odo)
                {
                    if (consumablesNote.Date < allNotes.First().Date)
                    {
                        return this.BadRequest("Дата текущей замены не может быть меньше даты в последней записи");
                    }
                }

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