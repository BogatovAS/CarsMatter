using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsMatter.Infrastructure.Models.MsSQL
{
    public class MyCar
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    
        public ICollection<ConsumablesNote> ConsumablesNotes { get; set; }

        public ICollection<RefillNote> RefillNotes { get; set; }

        public MyCar()
        {
            this.ConsumablesNotes = new List<ConsumablesNote>();
            this.RefillNotes = new List<RefillNote>();
        }
    }
}
